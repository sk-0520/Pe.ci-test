using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemPreferencesViewModel: ViewModelSkeleton
    {
        public ClockLauncherItemPreferencesViewModel(ClockLauncherItemSetting setting, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Setting = setting;
        }

        #region property

        ClockLauncherItemSetting Setting { get; }

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
