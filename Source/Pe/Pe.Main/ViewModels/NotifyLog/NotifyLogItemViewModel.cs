using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.NotifyLog;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.NotifyLog
{
    public class NotifyLogItemViewModel: ElementViewModelBase<NotifyLogItemElement>, INotifyLogId
    {
        public NotifyLogItemViewModel(NotifyLogItemElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            PropertyChangedObserver = new PropertyChangedObserver(DispatcherWrapper, LoggerFactory);
            PropertyChangedObserver.AddObserver(nameof(Model.Content), nameof(Content));
        }

        #region property

        private PropertyChangedObserver PropertyChangedObserver { get; }

        public NotifyLogKind Kind => Model.Kind;
        public string Header => Model.Header;
        public NotifyLogContentViewModel Content => new NotifyLogContentViewModel(Model.Content, LoggerFactory);

        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region ElementViewModelBase

        protected override void AttachModelEventsImpl()
        {
            base.AttachModelEventsImpl();

            Model.PropertyChanged += Model_PropertyChanged;
        }

        protected override void DetachModelEventsImpl()
        {
            base.DetachModelEventsImpl();

            Model.PropertyChanged -= Model_PropertyChanged;
        }

        #endregion

        #region INotifyLogId

        public NotifyLogId NotifyLogId => Model.NotifyLogId;

        #endregion

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            PropertyChangedObserver.Execute(e, RaisePropertyChanged);
        }
    }
}
