using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
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
            RawProperties = Model.RawProperties
                .Select(i => new CrashReportItemViewModel(i, LoggerFactory))
                .ToList()
            ;
            SendStatus = new RunningStatusViewModel(Model.SendStatus, LoggerFactory);
            SendStatus.PropertyChanged += SendStatus_PropertyChanged;
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();
        public RunningStatusViewModel SendStatus { get; }
        public string ErrorMessage => Model.ErrorMessage;

        public bool AutoSend => Model.AutoSend;
        public string UserId => Model.Data.UserId;

        public string CrashReportRawFilePath => Model.CrashReportRawFilePath;

        public string MailAddress
        {
            get => Model.Data.MailAddress;
            set => SetPropertyValue(Model.Data, value, nameof(Model.Data.MailAddress));
        }

        public string Comment
        {
            get => Model.Data.Comment;
            set => SetPropertyValue(Model.Data, value, nameof(Model.Data.Comment));
        }

        /// <summary>
        /// <para>クラッシュレポートの寿命は短いのでViewModelで囲うことはしない。</para>
        /// </summary>
        public IReadOnlyList<CrashReportItemViewModel> RawProperties { get; }


        #endregion

        #region command

        public ICommand LoadedCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {

            }
        ));

        public ICommand SendCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.SendAsync().ConfigureAwait(false);
            }
        ));

        public ICommand RebootCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.Reboot();
                CloseRequest.Send();
            }
        ));

        public ICommand CancelCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 Model.Cancel();
             }
         ));

        #endregion

        #region function

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                SendStatus.PropertyChanged -= SendStatus_PropertyChanged;
                if(disposing) {
                    SendStatus.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        private void SendStatus_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SendStatus.State)) {
                RaisePropertyChanged(nameof(ErrorMessage));
            }
        }
    }
}
