using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Clock.Models.Data;

namespace ContentTypeTextNet.Pe.Plugins.Clock.ViewModels
{
    public class ClockWidgetViewModel: ViewModelSkeleton
    {
        #region variable

        DateTime _currentTime;

        #endregion
        public ClockWidgetViewModel(ClockWidgetSetting setting, ISkeletonImplements skeletonImplements)
            : base(skeletonImplements)
        {
            Setting = setting;
        }

        #region property

        public DateTime CurrentTime
        {
            get => this._currentTime;
            set => SetProperty(ref this._currentTime, value);
        }

        public ClockWidgetSetting Setting { get; }

        public void StartClock()
        {

        }

        public void StopClock()
        {

        }

        #endregion
    }
}
