using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class StockLogger : LoggerBase
    {
        public StockLogger()
        { }

        public StockLogger(string header)
            : base(header)
        { }

        public StockLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

        #region LoggerBase

        protected override ILogger CreateLoggerCore(string header)
        {
            return new StockLogger(header);
        }

        protected override void PutCore(LogItem logItem)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
