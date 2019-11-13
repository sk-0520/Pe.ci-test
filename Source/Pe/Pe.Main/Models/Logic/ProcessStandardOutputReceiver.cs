using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Logic
{
    public enum StandardOutputMode
    {
        Output,
        Error
    }
    public class ProcessStandardOutputReceivedEventArgs : EventArgs
    {
        public ProcessStandardOutputReceivedEventArgs(StandardOutputMode mode, string value)
        {
            Mode = mode;
            Value = value;
        }

        #region property

        public StandardOutputMode Mode { get; }
        public string Value { get; }

        #endregion
    }

    public class StandardReader
    {
        public StandardReader(StandardOutputMode mode, StreamReader reader)
        {
            Mode = mode;
            Reader = reader;
        }

        #region property

        public StandardOutputMode Mode { get; }
        public StreamReader Reader { get; }

        #endregion
    }

    public class ProcessStandardOutputReceiver
    {
        #region event

        public event EventHandler<ProcessStandardOutputReceivedEventArgs>? StandardOutputReceived;

        #endregion

        public ProcessStandardOutputReceiver(Process process, ILoggerFactory loggerFactory)
        {
            Process = process;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        Process Process { get; }
        ILogger Logger { get; }

        public int BufferSize { get; set; } = 1024;
        public TimeSpan PeekTime { get; set; } = TimeSpan.FromMilliseconds(50);
        public TimeSpan WaitTime { get; set; } = TimeSpan.FromMilliseconds(250);
        CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        Task RunningTask { get; set; } = Task.CompletedTask;
        #endregion

        #region function

        private void OnStandardOutputReceived(StandardOutputMode mode, string value)
        {
            var e = new ProcessStandardOutputReceivedEventArgs(mode, value);
            StandardOutputReceived?.Invoke(this, e);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="mode"></param>
        /// <param name="reader"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>再待機可能か</returns>
        bool Receive(StandardReader standardReader, CancellationToken cancellationToken)
        {
            char[] buffer = new char[BufferSize];

            var readTask = standardReader.Reader.ReadAsync(buffer, 0, buffer.Length);
            try {
                var readWaitResult = readTask.Wait((int)WaitTime.TotalMilliseconds, cancellationToken);
                if(!readWaitResult) {
                    Logger.LogTrace("再待機");
                    return true;
                }
            } catch(OperationCanceledException ex) {
                Logger.LogTrace(ex, "待機終了(キャンセル)");
                return false;
            }
            var readLength = readTask.Result;
            if(readLength == 0) {
                Logger.LogTrace("読み込み長が 0 なんで終了");
                return false;
            }

            var value = new string(buffer, 0, readLength);
            OnStandardOutputReceived(standardReader.Mode, value);

            if(standardReader.Reader.EndOfStream) {
                return false;
            }

            return true;
        }

        private void StartReceiveCore()
        {
            var token = CancellationTokenSource.Token;
            var standardReaders = new List<StandardReader>() {
                new StandardReader(StandardOutputMode.Output, Process.StandardOutput),
                new StandardReader(StandardOutputMode.Error, Process.StandardError),
            };

            while(true) {
                Thread.Sleep(PeekTime);
                var standardReader = standardReaders.FirstOrDefault(i => 0 <= i.Reader.Peek());
                if(standardReader == null) {
                    Logger.LogTrace("出力・エラー共に読み込みできない");
                    continue;
                }

                if(Receive(standardReader, token)) {
                    if(standardReaders.Count == 1) {
                        Logger.LogTrace("残りのストリームはいない");
                        continue;
                    }

                    var nextStandardReader = standardReaders.First(i => i != standardReader);
                    if(nextStandardReader.Reader.Peek() == -1) {
                        Logger.LogTrace("沈黙");
                        continue;
                    }
                    if(!Receive(nextStandardReader, token)) {
                        Logger.LogTrace("天命を全うした");
                        standardReaders.Remove(nextStandardReader);
                    }
                } else {
                    standardReaders.Remove(standardReader);
                    if(standardReaders.Count == 0) {
                        Logger.LogTrace("読み取り可能なやつはなくなった");
                        break;
                    }
                }
            }
        }

        public void StartReceive()
        {
            RunningTask = Task.Run(StartReceiveCore);
        }

        #endregion
    }
}
