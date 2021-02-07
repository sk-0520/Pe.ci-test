using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels
{
    public class RunningStatusViewModel: SingleModelViewModelBase<IRunningStatus>, IRunningStatus
    {
        public RunningStatusViewModel(IRunningStatus model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        #endregion

        #region IRunningStatus

        public RunningState State => Model.State;

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

        private void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(State)) {
                RaisePropertyChanged(nameof(State));
            }
        }
    }
}
