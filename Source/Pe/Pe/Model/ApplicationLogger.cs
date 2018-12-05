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
    public class ApplicationLogger : LoggerBase
    {
        public ApplicationLogger()
        { }

        private ApplicationLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

        #region property

        IDictionary<int, char> KindMap { get; } = new Dictionary<int, char>() {
            [(int)LogKind.Trace] = 'T',
            [(int)LogKind.Debug] = 'D',
            [(int)LogKind.Information] = 'I',
            [(int)LogKind.Warning] = 'W',
            [(int)LogKind.Error] = 'E',
            [(int)LogKind.Fatal] = 'F',
        };

        #endregion

        #region function

        void Write(LoggerBase sender, LogItem logItem)
        {
            var buffer = new StringBuilder();
            buffer.AppendFormat("{0:yyyy-MM-dd HH:mm:ss.fff}", logItem.Timestamp);
            buffer.Append(' ');
            //buffer.AppendFormat("[{0}]", KindMap[(int)logItem.Kind]);
            buffer.AppendFormat("{0}", logItem.Kind);
            buffer.Append(' ');
            buffer.Append(sender.Header);
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

        #endregion

        #region LoggerBase

        protected override ILogger CreateLoggerCore(string header)
        {
            return new ApplicationLogger(header, this);
        }

        protected override void PutCore(LogItem logItem)
        {
            Write(this, logItem);
        }

        #endregion
    }


}
