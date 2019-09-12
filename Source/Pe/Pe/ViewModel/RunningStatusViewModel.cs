using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Model;
using ContentTypeTextNet.Pe.Core.ViewModel;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModel
{
    public class RunningStatusViewModel : SingleModelViewModelBase<IRunningStatus>, IRunningStatus
    {
        public RunningStatusViewModel(IRunningStatus model, ILogger logger)
            : base(model, logger)
        { }

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

        private void Model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(State)) {
                RaisePropertyChanged(nameof(State));
            }
        }
    }
}
