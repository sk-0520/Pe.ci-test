using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    /// <summary>
    /// ログをためる。
    /// </summary>
    public class StockLogger : LoggerBase
    {
        public StockLogger()
        {
            Items = new ReadOnlyCollection<LogItem>(LogItemList);
        }

        #region property

        List<LogItem> LogItemList { get; } = new List<LogItem>();
        public ReadOnlyCollection<LogItem> Items { get; }

        #endregion

        #region LoggerBase

        protected override ILogger CreateLoggerCore(string header)
        {
            return new ChildLogger(header, this);
        }

        protected override void PutCore(LogItem logItem)
        {
            LogItemList.Add(logItem);
        }


        #endregion
    }

    /// <summary>
    /// ログを扱う側へ非同期でログ情報を安全になんやかんやする。
    /// <para>渡される側は単一スレッドを保証する(それがどのスレッドかまでは面倒見ない)</para>
    /// </summary>
    public abstract class AsyncLoggerBase : LoggerBase, IDisposable
    {
        public AsyncLoggerBase()
            : base()
        {
            Constructor();
        }

        public AsyncLoggerBase(string header)
            : base(header)
        {
            Constructor();
        }

        #region property

        CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        ManualResetEventSlim LogAddEvent { get; } = new ManualResetEventSlim(false);

        ConcurrentQueue<LogItem> Items { get; } = new ConcurrentQueue<LogItem>();

        public bool IsCompleted { get; private set; }

        #endregion

        #region function

        void Constructor()
        {
            PutThreadAsync(CancellationTokenSource.Token).ConfigureAwait(false);
        }

        /// <summary>
        /// ログをなんやかんやするスレッド。
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        void PutThreadCore(CancellationToken cancellationToken)
        {
            while(true) {
                IsCompleted = true;
                LogAddEvent.Wait(cancellationToken);
                IsCompleted = false;
                LogAddEvent.Reset();

                var outputItems = new List<LogItem>(Items.Count);
                while(Items.TryDequeue(out var item)) {
                    outputItems.Add(item);
                }

                if(0 < outputItems.Count) {
                    PutItems(outputItems);
                }

                if(cancellationToken.IsCancellationRequested) {
                    IsCompleted = true;
                    return;
                }
            }
        }

        protected Task PutThreadAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() => PutThreadCore(cancellationToken));
        }

        /// <summary>
        /// 単一のスレッドにて渡されたデータをなんやかんやする。
        /// <para>少なくともログ生成スレッドとは異なる。</para>
        /// <para>本処理は単一スレッドにて呼び出されるので呼び出され側は好きにすればいいけどそれが GUI スレッドでないのは間違いないよ。</para>
        /// </summary>
        /// <param name="logItems">古い順。</param>
        protected abstract void PutItems(IReadOnlyList<LogItem> logItems);

        #endregion

        #region LoggerBase

        protected override ILogger CreateLoggerCore(string header) => new ChildLogger(header, this);

        protected sealed override void PutCore(LogItem logItem)
        {
            Items.Enqueue(logItem);

            LogAddEvent.Set();
        }

        #endregion

        #region IDisposable Support

        public bool IsDisposed { get; private set; } // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this.IsDisposed) {
                if(disposing) {
                    // TODO: マネージド状態を破棄します (マネージド オブジェクト)。
                    CancellationTokenSource.Cancel(true);
                    CancellationTokenSource.Dispose();
                    if(LogAddEvent.IsSet) {
                        LogAddEvent.Dispose();
                    }
                }

                // TODO: アンマネージド リソース (アンマネージド オブジェクト) を解放し、下のファイナライザーをオーバーライドします。
                // TODO: 大きなフィールドを null に設定します。

                this.IsDisposed = true;
            }
        }

        // TODO: 上の Dispose(bool disposing) にアンマネージド リソースを解放するコードが含まれる場合にのみ、ファイナライザーをオーバーライドします。
        // ~AsyncLoggerBase() {
        //   // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
        //   Dispose(false);
        // }

        // このコードは、破棄可能なパターンを正しく実装できるように追加されました。
        public void Dispose()
        {
            // このコードを変更しないでください。クリーンアップ コードを上の Dispose(bool disposing) に記述します。
            Dispose(true);
            // TODO: 上のファイナライザーがオーバーライドされる場合は、次の行のコメントを解除してください。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }

    public sealed class ActionAsyncLogger : AsyncLoggerBase
    {
        public ActionAsyncLogger(Action<IReadOnlyList<LogItem>> action)
        {
            Action = action;
        }

        public ActionAsyncLogger(Action<IReadOnlyList<LogItem>> action, string header) : base(header)
        {
            Action = action;
        }

        #region property

        Action<IReadOnlyList<LogItem>> Action { get; }

        #endregion

        #region ActionAsyncLogger

        protected override void PutItems(IReadOnlyList<LogItem> logItems)
        {
            Action(logItems);
        }

        #endregion
    }

    public static class LoggingUtility
    {
        #region define

        const string indent = "    ";
        const string messageFormat = " [MSG] {0}";
        const string detailPadding = "\t";

        #endregion

        #region function

        public static string ToSimpleMessage(LogItem logItem)
        {
            var buffer = new StringBuilder();
            buffer.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.fff}", logItem.Timestamp);
            buffer.Append(' ');
            buffer.AppendFormat("{0}", logItem.Kind);
            buffer.Append(' ');
            buffer.AppendFormat("<{0}>", logItem.Caller.MemberName);
            buffer.Append(logItem.Message);

            return logItem.ToString();
        }

        public static string ToTraceMessage(LogItem logItem)
        {
            var buffer = new StringBuilder();
            buffer.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.fff}", logItem.Timestamp);
            buffer.Append(' ');
            //buffer.AppendFormat("[{0}]", KindMap[(int)logItem.Kind]);
            buffer.AppendFormat("{0}", logItem.Kind);
            buffer.Append(' ');
            buffer.Append(logItem.Message);
            buffer.Append(' ');
            buffer.Append('[');
            buffer.Append(logItem.Caller.Thread.ManagedThreadId);
            buffer.Append(']');
            buffer.Append(' ');
            buffer.Append(logItem.Header);
            buffer.Append(' ');
            buffer.AppendFormat("<{0}.{1}>", logItem.StackTrace.GetFrame(0).GetMethod().DeclaringType.Name, logItem.Caller.MemberName);
            buffer.Append(' ');
            //var detailIndentWidth = buffer.Length;

            buffer.Append(logItem.ShortFilePath);
            buffer.AppendFormat("({0})", logItem.Caller.LineNumber);

            if(logItem.HasDetail) {
                //var indent = new string(' ', detailIndentWidth);
                foreach(var line in TextUtility.ReadLines(logItem.Detail.ToString())) {
                    buffer.AppendLine();
                    buffer.Append('\t');
                    buffer.Append(line);
                }
            }

            return buffer.ToString();
        }

        static string ToShowText(MethodBase method)
        {
            var parameters = string.Join(
                ", ",
                method.GetParameters()
                    .OrderBy(p => p.Position)
                    .Select(p => p.ToString())
            );

            return (method?.ReflectedType?.Name ?? "(null)") + "." + (method?.Name ?? "null") + "(" + parameters + ")";
        }

        public static string ToDetailMessage(LogItem item)
        {
            var header = string.Format(
                "{0}[{1}] {2} <{3}({4})> , Thread: {5}/{6}, Assembly: {7}",
                item.Timestamp.ToString("yyyy-MM-ddTHH:mm:ss.fff"),
                item.Kind.ToString().ToUpper().Substring(0, 1),
                item.Caller.MemberName,
                item.ShortFilePath,
                item.Caller.LineNumber,
                item.Caller.Thread.ManagedThreadId,
                item.Caller.Thread.ThreadState,
                item.Caller.Assembly.GetName()
            );

            var detailIndent = "\t";

            var message = string.Format(messageFormat, item.Message);
            var detail = item.Detail;
            if(!string.IsNullOrEmpty(detail)) {
                detail = Environment.NewLine + string.Join(Environment.NewLine, TextUtility.ReadLines(detail).Select(s => detailIndent + detailPadding + s));
            }
            var stack = string.Join(
                Environment.NewLine,
                item.StackTrace.GetFrames()
                    .Select(sf => string.Format(
                        indent + "-[{0:x8}][{1:x8}] {2}[{3}]",
                        sf.GetNativeOffset(),
                        sf.GetILOffset(),
                        ToShowText(sf.GetMethod()),
                        sf.GetFileLineNumber()
                    )
                )
            );

            var result
                = header
                + Environment.NewLine
                + message
                + detail
                + Environment.NewLine
                + " [STK]"
                + Environment.NewLine
                + indent + "+[ Native ][   IL   ] Method[line]"
                + Environment.NewLine
                + stack
                + Environment.NewLine
            ;
            return result;
        }

        #endregion
    }

}
