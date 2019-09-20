using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model
{
    public enum RunningState
    {
        None,
        Running,
        End,
        Cancel,
        Error,
    }

    public interface IRunningStatus : INotifyPropertyChanged
    {
        #region property

        RunningState State { get; }

        #endregion
    }

    public class RunningStatus : BindModelBase, IRunningStatus
    {
        #region variable

        RunningState _state;

        #endregion

        public RunningStatus(ILogger logger)
            : base(logger)
        { }

        public RunningStatus(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region IRunningStatus

        public RunningState State
        {
            get => this._state;
            set => SetProperty(ref this._state, value);
        }

        #endregion
    }
}
