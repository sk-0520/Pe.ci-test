using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.CrashReport.ViewModels;

namespace ContentTypeTextNet.Pe.CrashReport.Models
{
    public class CrashReportInitializer
    {
        public CrashReportInitializer(Options options)
        {
            Options = options;
        }

        #region property

        Options Options { get; }

        #endregion

        #region function

        public MainWorker CreateWorker()
        {
            //throw new NotImplementedException();
            return null!;
        }

        public MainViewModel CreateViewModel(MainWorker model)
        {
            //throw new NotImplementedException();
            return null!;
        }


        #endregion
    }
}
