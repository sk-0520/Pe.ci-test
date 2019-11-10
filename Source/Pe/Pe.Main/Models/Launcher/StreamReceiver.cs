using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class StreamReceivedEventArgs : EventArgs
    {
        public StreamReceivedEventArgs(string value)
        {
            Value = value;
        }

        #region property

        public string Value { get; }

        #endregion
    }

    public class StreamReceiver
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

        public int BufferSize { get; set; } = 1024;
        public TimeSpan WaitTime { get; set; } = TimeSpan.FromMilliseconds(1000);

        StreamReader Reader { get; }
        ILogger Logger { get; }

        CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();
        Task RunningTask { get; set; } = Task.CompletedTask;

        #endregion

        #region function

        private void OnStreamReceived(string value)
        {
            var e = new StreamReceivedEventArgs(value);
            StreamReceived?.Invoke(this, e);
        }

        public void StartReceive()
        {
            RunningTask = Task.Run(() => {
                Logger.LogTrace("start");
                char[] buffer = new char[BufferSize];

                while(true) {
                    var readTask = Reader.ReadAsync(buffer, 0, buffer.Length);
                    try {
                        var readWaitResult = readTask.Wait((int)WaitTime.TotalMilliseconds, CancellationTokenSource.Token);
                        if(!readWaitResult) {
                            Logger.LogTrace("再待機");
                            break;
                        }
                    } catch(OperationCanceledException ex) {
                        Logger.LogTrace(ex, "待機終了(キャンセル)");
                    }
                    var readLength = readTask.Result;
                    if(readLength == 0) {
                        Logger.LogTrace("読み込み長が 0 なんで終了");
                        return;
                    }

                    var value = new string(buffer, 0, readLength);
                    OnStreamReceived(value);

                    if(Reader.EndOfStream) {
                        break;
                    }
                }
                Logger.LogTrace("end");
            });
        }

        #endregion
    }
}
