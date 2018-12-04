using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model
{
    public class ApplicationLogger : LoggerBase
    {
        public ApplicationLogger()
        { }

        public ApplicationLogger(string header)
            : base(header)
        { }

        public ApplicationLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

        #region event

        public event EventHandler<LogItem> Output;

        #endregion

        #region function

        protected void OnPutput(LogItem logItem)
        {
            if(Output != null) {
                Output(this, logItem);
            }
            if(ParentLogger is ApplicationLogger parentAppLogger) {
                parentAppLogger.OnPutput(logItem);
            }

        }

        #endregion

        #region ApplicationLogger

        protected override ILogger CreateChildCore(string header)
        {
            return new ApplicationLogger(header, this);
        }

        protected override void PutCore(LogItem logItem)
        {
            OnPutput(logItem);
        }

        #endregion
    }

    public class DevelopmentLogging
    {
        #region property

        static DevelopmentLogging Instance { get; set; }

        #endregion

        #region function

        public static void Initialize(ApplicationLogger logger)
        {
            var instance = new DevelopmentLogging();
            logger.Output += instance.Logger_Output;
        }

        public void Write(LogItem logItem)
        {
            Debug.WriteLine($"{logItem.Kind} {logItem.Message}");
        }

        #endregion

        private void Logger_Output(object sender, LogItem e)
        {
            Write(e);
        }

    }
}
