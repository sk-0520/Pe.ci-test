using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Embedded.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Applications
{
    public class LogEventArgs: EventArgs
    {
        public LogEventArgs(LogItem logItem)
        {
            LogItem = logItem;
        }

        #region property

        LogItem LogItem { get; }

        #endregion
    }

    public class ApplicationLogger : AsyncLoggerBase
    {
        public ApplicationLogger()
        { }

        #region event

        public event EventHandler<LogEventArgs> ReceivedLog;

        #endregion

        #region property

        List<TextWriter> TextWriters { get; set; }

        #endregion

        #region function

        void WriteConsole(LogItem logItem)
        {
            var message = LoggingUtility.ToTraceMessage(logItem);
            System.Diagnostics.Debug.WriteLine(message);
        }

        void WriteTextWriters(LogItem logItem)
        {
            var message = LoggingUtility.ToDetailMessage(logItem);
            foreach(var writer in TextWriters) {
                writer.WriteLine(message);
            }
        }

        void OnReceivedLog(LogItem logItem)
        {
            if(ReceivedLog == null) {
                return;
            }

            var eventArgs = new LogEventArgs(logItem);
            ReceivedLog(this, eventArgs);
        }

        void Write(LogItem logItem)
        {
            WriteConsole(logItem);
            WriteTextWriters(logItem);
            OnReceivedLog(logItem);
        }

        public void AttachWriter(TextWriter writer)
        {
            if(writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if(TextWriters == null) {
                TextWriters = new List<TextWriter>();
            }
            TextWriters.Add(writer);
        }

        public bool DetachWriter(TextWriter writer)
        {
            if(writer == null) {
                throw new ArgumentNullException(nameof(writer));
            }

            if(TextWriters == null) {
                return false;
            }

            var result = TextWriters.Remove(writer);
            if(!TextWriters.Any()) {
                TextWriters = null;
            }

            return result;
        }

        #endregion

        #region AsyncLoggerBase

        protected override void PutItems(IReadOnlyList<LogItem> logItems)
        {
            foreach(var logItem in logItems) {
                Write(logItem);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var writer in TextWriters) {
                        writer.Dispose();
                    }
                    TextWriters = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }


}
