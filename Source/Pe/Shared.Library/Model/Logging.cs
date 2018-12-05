using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public class StockLogger : LoggerBase
    {
        public StockLogger()
        {
            Items = new ReadOnlyCollection<LogItem>(LogItemList);
        }

        private StockLogger(string header, LoggerBase parentLogger)
            : base(header, parentLogger)
        { }

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
}
