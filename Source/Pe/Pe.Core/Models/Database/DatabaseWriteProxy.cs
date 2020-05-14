using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Core.Models.Database
{
    /// <summary>
    /// DB 書き込み処理を責任者不在な形で行う。
    /// </summary>
    public interface IDatabaseWriteProxy
    {
        #region property

        /// <summary>
        /// 非同期処理を主とするか。
        /// <para>この値に準拠するかどうかは実装次第。</para>
        /// </summary>
        bool IsAsync { get; }

        #endregion

        #region function

        /// <summary>
        /// 書き込み処理を実施。
        /// <para><see cref="IsAsync"/>がどうであれ実装側が即時か非同期化は知ったこっちゃない。</para>
        /// </summary>
        /// <param name="statement"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        Task<int> WriteAsync(string statement, object? parameter);

        #endregion
    }


    /// <inheritdoc cref="IDatabaseWriteProxy"/>
    public class DatabaseWriteProxy: IDatabaseWriteProxy
    {
        #region define

        /// <summary>
        /// 書き込み方法。
        /// </summary>
        enum WriteMode
        {
            /// <summary>
            /// 即時。
            /// </summary>
            Writer,
            /// <summary>
            /// 待機。
            /// </summary>
            Barrier,
            /// <summary>
            /// 遅延。
            /// </summary>
            Lazy,
        }

        #endregion

        /// <summary>
        /// 即時実行が行われる書き込み処理。
        /// <para>コミットそのものは関与しない。</para>
        /// </summary>
        /// <param name="databaseWriter"></param>
        /// <param name="loggerFactory"></param>
        public DatabaseWriteProxy(IDatabaseWriter databaseWriter, ILoggerFactory loggerFactory)
        {
            IsAsync = false;
            Mode = WriteMode.Writer;
            Logger = loggerFactory.CreateLogger(GetType());

            Writer = databaseWriter;
        }

        /// <summary>
        /// DBアクセスが存在しない場合に書き込みを行う。
        /// <para>コミットまで行う。</para>
        /// </summary>
        /// <param name="databaseBarrier"></param>
        /// <param name="loggerFactory"></param>
        public DatabaseWriteProxy(IDatabaseBarrier databaseBarrier, ILoggerFactory loggerFactory)
        {
            IsAsync = false;
            Mode = WriteMode.Barrier;
            Logger = loggerFactory.CreateLogger(GetType());

            Barrier = databaseBarrier;
        }

        /// <summary>
        /// 遅延書き込みを実施する。
        /// <para>書き込み処理はトランザクション中に行われ、結果件数は多分コミットに近いタイミングで返ってくる、はず。。。</para>
        /// </summary>
        /// <param name="databaseLazyWriter"></param>
        /// <param name="loggerFactory"></param>
        public DatabaseWriteProxy(IDatabaseLazyWriter databaseLazyWriter, ILoggerFactory loggerFactory)
        {
            IsAsync = true;
            Mode = WriteMode.Lazy;
            Logger = loggerFactory.CreateLogger(GetType());

            Lazy = databaseLazyWriter;
        }

        #region property

        ILogger Logger { get; }
        /// <inheritdoc cref="WriteMode"/>
        WriteMode Mode { get; }

        IDatabaseWriter? Writer { get; }
        IDatabaseBarrier? Barrier { get; }
        IDatabaseLazyWriter? Lazy { get; }

        #endregion

        #region IDatabaseWriteProxy

        /// <inheritdoc cref="IDatabaseWriteProxy.IsAsync"/>
        public bool IsAsync { get; }

        /// <inheritdoc cref="IDatabaseWriteProxy.WriteAsync(string, object?)"/>
        public Task<int> WriteAsync(string statement, object? parameter = null)
        {
            switch(Mode) {
                case WriteMode.Writer: {
                        Debug.Assert(Writer != null);

                        var result = Writer.Execute(statement, parameter);
                        return Task.FromResult(result);
                    }

                case WriteMode.Barrier: {
                        Debug.Assert(Barrier != null);

                        using var context = Barrier.WaitWrite();
                        var result = context.Execute(statement, parameter);
                        return Task.FromResult(result);
                    }

                case WriteMode.Lazy: {
                        Debug.Assert(Lazy != null);

                        return Task.Run(() => {
                            var wait = new ManualResetEventSlim();
                            var result = 0;
                            Lazy.Stock(c => {
                                result = c.Execute(statement, parameter);
                                wait.Set();
                            });
                            wait.Wait();
                            return result;
                        });
                    }

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
