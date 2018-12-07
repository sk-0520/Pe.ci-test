using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
    /// ログを扱う側へログ情報を安全になんやかんやする。
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
        ///
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

        #region IDisposable Support

        private bool disposedValue = false; // 重複する呼び出しを検出するには

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposedValue) {
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

                this.disposedValue = true;
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

}
