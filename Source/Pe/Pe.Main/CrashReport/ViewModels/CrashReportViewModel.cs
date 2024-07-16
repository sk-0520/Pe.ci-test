using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.CrashReport.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.CrashReport.ViewModels
{
    public class CrashReportViewModel: ElementViewModelBase<CrashReportElement>
    {
        #region variable

        private double _waitCount;

        #endregion

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

        private TimeSpan AutoSendWaitTime { get; } = TimeSpan.FromSeconds(10);
        private DateTime AutoSendStartTime { get; set; }
        private DateTime AutoSendEndTime { get; set; }
        private DispatcherTimer? AutoSendWaitTimer { get; set; }

        public double WaitCount
        {
            get => this._waitCount;
            set => SetProperty(ref this._waitCount, value);
        }

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
        /// </summary>
        /// <remarks>
        /// <para>クラッシュレポートの寿命は短いのでViewModelで囲うことはしない。</para>
        /// </remarks>
        public IReadOnlyList<CrashReportItemViewModel> RawProperties { get; }

        public string CrashReportSaveFilePath => Model.CrashReportSaveFilePath;
        #endregion

        #region command

        private ICommand? _LoadedCommand;
        public ICommand LoadedCommand => this._LoadedCommand ??= new DelegateCommand(
            () => {
                if(AutoSend) {
                    AutoSendWaitTimer = new DispatcherTimer();
                    AutoSendWaitTimer.Interval = TimeSpan.FromMilliseconds(16);
                    DispatcherWrapper.BeginAsync(() => {
                        AutoSendStartTime = DateTime.UtcNow;
                        AutoSendEndTime = AutoSendStartTime + AutoSendWaitTime;
                        if(AutoSend) {
                            AutoSendWaitTimer.Tick += AutoSendWaitTimer_Tick;
                            AutoSendWaitTimer.Start();
                        }
                    }, DispatcherPriority.SystemIdle);
                }
            }
        );

        private ICommand? _StopAutoSendCommand;
        public ICommand StopAutoSendCommand => this._StopAutoSendCommand ??= new DelegateCommand(
            () => {
                StopAutoSend();
            }
        );

        private ICommand? _ShowSourceUriCommand;
        public ICommand ShowSourceUriCommand => this._ShowSourceUriCommand ??= new DelegateCommand(
             () => {
                 Model.ShowSourceUri();
             }
         );

        private ICommand? _SendCommand;
        public ICommand SendCommand => this._SendCommand ??= new DelegateCommand(
            () => {
                StopAutoSend();

                Model.SendAsync(CancellationToken.None).ConfigureAwait(false);
            }
        );

        private ICommand? _RebootCommand;
        public ICommand RebootCommand => this._RebootCommand ??= new DelegateCommand(
            () => {
                Model.Reboot();
                CloseRequest.Send();
            }
        );

        private ICommand? _CancelCommand;
        public ICommand CancelCommand => this._CancelCommand ??= new DelegateCommand(
             () => {
                 Model.Cancel();
             }
         );

        #endregion

        #region function

        private void StopAutoSend()
        {
            if(AutoSendWaitTimer != null) {
                AutoSendWaitTimer.Stop();
                AutoSendWaitTimer.Tick -= AutoSendWaitTimer_Tick;
            }

            Model.CancelAutoSend();
            RaisePropertyChanged(nameof(AutoSend));
        }

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

        private void SendStatus_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SendStatus.State)) {
                RaisePropertyChanged(nameof(ErrorMessage));
                RaisePropertyChanged(nameof(CrashReportSaveFilePath));
            }
        }

        private void AutoSendWaitTimer_Tick(object? sender, EventArgs e)
        {
            var current = DateTime.UtcNow;
            if(AutoSendEndTime < current) {
                WaitCount = 1;

                StopAutoSend();

                SendCommand.Execute(null);
            } else {
                WaitCount = (current - AutoSendStartTime).TotalMilliseconds / AutoSendWaitTime.TotalMilliseconds;
            }
        }
    }
}
