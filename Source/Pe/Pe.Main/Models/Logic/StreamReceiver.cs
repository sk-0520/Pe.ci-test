using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public class StreamReceivedEventArgs: EventArgs
    {
        public StreamReceivedEventArgs(string value, bool exited)
        {
            Value = value;
            Exited = exited;
        }

        #region property

        public string Value { get; }
        public bool Exited { get; }

        #endregion
    }

    public class StreamReceiver: DisposerBase
    {
        #region event

        public event EventHandler<StreamReceivedEventArgs>? StreamReceived;

        #endregion

        public StreamReceiver(StreamReader reader, ILoggerFactory loggerFactory)
        {
            Reader = reader;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        public int BufferSize { get; set; } = 160;
        public TimeSpan WaitTime { get; set; } = TimeSpan.FromMilliseconds(250);

        private StreamReader Reader { get; }
        private ILogger Logger { get; }

        private CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        private Task RunningTask { get; set; } = Task.CompletedTask;
        private ManualResetEventSlim LastWorkWait { get; } = new ManualResetEventSlim(false);

        #endregion

        #region function

        private void OnStreamReceived(string value, bool exited)
        {
            var e = new StreamReceivedEventArgs(value, exited);
            StreamReceived?.Invoke(this, e);
        }

        public void StartReceive()
        {
            RunningTask = Task.Run(() => {
                Logger.LogTrace("start");
                char[] buffer = new char[BufferSize];
                var token = CancellationTokenSource.Token;
                while(0 < Reader.Peek()) {
                    var readTask = Reader.ReadAsync(buffer, 0, buffer.Length);
                    try {
                        var readWaitResult = readTask.Wait((int)WaitTime.TotalMilliseconds, token);
                        if(!readWaitResult) {
                            Logger.LogTrace("再待機");
                            continue;
                        }
                    } catch(OperationCanceledException ex) {
                        Logger.LogTrace(ex, "待機終了(キャンセル)");
                        break;
                    }

                    var readLength = readTask.Result;
                    if(readLength == 0) {
                        Logger.LogTrace("読み込み長が 0 なんで終了");
                        break;
                    }

                    var value = new string(buffer, 0, readLength);
                    OnStreamReceived(value, false);

                    if(Reader.EndOfStream) {
                        Logger.LogTrace("もう読めない");
                        break;
                    }
                    if(token.IsCancellationRequested) {
                        Logger.LogTrace("タスクキャンセル要求あり");
                        // 読めるなら読んどく
                        if(0 <= Reader.Peek()) {
                            Logger.LogTrace("キャンセル前の一仕事");
                            var fullValue = Reader.ReadToEnd();
                            OnStreamReceived(fullValue, false);
                        }
                        break;
                    }
                }
                OnStreamReceived(string.Empty, true);
                LastWorkWait.Set();
                Logger.LogTrace("reader end");
            });
        }

        #endregion

        #region DisposerBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(!RunningTask.IsCompleted) {
                        CancellationTokenSource.Cancel(true);
                    }

                    //LastWorkWait.Wait();
                    LastWorkWait.Dispose();

                    CancellationTokenSource.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
