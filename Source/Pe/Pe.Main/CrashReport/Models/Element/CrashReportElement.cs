using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Element;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Element
{
    class CrashReportElement : ElementBase
    {
        public CrashReportElement(ILoggerFactory loggerFactory) : base(loggerFactory)
        {
        }

        #region property

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
