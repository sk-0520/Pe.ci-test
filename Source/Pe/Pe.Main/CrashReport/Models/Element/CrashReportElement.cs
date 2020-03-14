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
        public CrashReportElement(CrashReportOptions options, EnvironmentParameters environmentParameters, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options;
            EnvironmentParameters = environmentParameters;
        }

        #region property

        EnvironmentParameters EnvironmentParameters { get; }
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
