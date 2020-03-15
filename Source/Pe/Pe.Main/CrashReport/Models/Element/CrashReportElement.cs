using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models.Element
{
    class CrashReportElement : ElementBase
    {
        public CrashReportElement(CrashReportOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
        }

        #region property

        CrashReportOptions Options { get; }

        #endregion

        #region function

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
        }

        #endregion
    }
}
