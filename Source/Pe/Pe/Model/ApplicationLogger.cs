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

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationLogger : AsyncLoggerBase
    {
        public ApplicationLogger()
        { }

        #region property

        List<TextWriter> TextWriters { get; set; }

        #endregion

        #region function

        void WriteConsole(LogItem logItem)
        {
            var buffer = new StringBuilder();
            buffer.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.fff}", logItem.Timestamp);
            buffer.Append(' ');
            //buffer.AppendFormat("[{0}]", KindMap[(int)logItem.Kind]);
            buffer.AppendFormat("{0}", logItem.Kind);
            buffer.Append(' ');
            buffer.Append(logItem.Header);
            buffer.Append(' ');
            buffer.AppendFormat("<{0}>", logItem.Caller.memberName);
            buffer.Append(' ');
            var detailIndentWidth = buffer.Length;

            buffer.Append(logItem.Message);
            buffer.Append(' ');
            buffer.Append(logItem.ShortFilePath);
            buffer.AppendFormat("({0})", logItem.Caller.lineNumber);

            if(logItem.HasDetail) {
                var indent = new string(' ', detailIndentWidth);
                foreach(var line in TextUtility.ReadLines(logItem.Detail.ToString())) {
                    buffer.AppendLine();
                    buffer.Append(indent);
                    buffer.Append(line);
                }
            }

            System.Diagnostics.Debug.WriteLine(buffer.ToString());
        }

        void WriteTextWriters(LogItem logItem)
        {
            foreach(var writer in TextWriters) {
                writer.WriteLine(logItem);
            }
        }

        void Write(LogItem logItem)
        {
            WriteConsole(logItem);
            WriteTextWriters(logItem);
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
