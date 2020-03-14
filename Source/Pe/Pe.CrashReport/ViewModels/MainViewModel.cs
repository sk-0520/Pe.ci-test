using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.CrashReport.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.CrashReport.ViewModels
{
    public class MainViewModel : SingleModelViewModelBase<MainWorker>
    {
        public MainViewModel(MainWorker model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
