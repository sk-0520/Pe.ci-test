using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Data;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Element;
using ContentTypeTextNet.Pe.Main.CrashReport.ViewModels;

namespace ContentTypeTextNet.Pe.Main.CrashReport.Models
{
    internal class CrashReportInitializer
    {
        public CrashReportInitializer(CrashReportOptions options)
        {
            Options = options;
        }

        #region property

        CrashReportOptions Options { get; }

        #endregion

        #region function

        public CrashReportElement CreateWorker()
        {
            //throw new NotImplementedException();
            return null!;
        }

        public CrashReportViewModel CreateViewModel(CrashReportElement model)
        {
            //throw new NotImplementedException();
            return null!;
        }


        #endregion
    }
}
