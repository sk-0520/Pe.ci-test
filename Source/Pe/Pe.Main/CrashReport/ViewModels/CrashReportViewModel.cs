using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.CrashReport.ViewModels
{
    internal class CrashReportViewModel : ElementViewModelBase<CrashReportElement>
    {
        public CrashReportViewModel(CrashReportElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
        }

        #region property

        public bool AutoSend => Model.AutoSend;
        public string UserId => Model.Data.UserId;

        public string ContactMailAddress
        {
            get => Model.Data.ContactMailAddress;
            set => SetPropertyValue(Model.Data, value, nameof(Model.Data.ContactMailAddress));
        }

        public string Comment
        {
            get => Model.Data.Comment;
            set => SetPropertyValue(Model.Data, value, nameof(Model.Data.Comment));
        }

        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {

            }
        ));

        #endregion

        #region function

        #endregion
    }
}
