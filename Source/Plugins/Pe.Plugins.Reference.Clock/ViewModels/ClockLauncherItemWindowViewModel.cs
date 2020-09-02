using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemWindowViewModel: ViewModelSkeleton
    {
        internal ClockLauncherItemWindowViewModel(ClockLauncherItem item, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Item = item;
        }

        #region property

        ClockLauncherItem Item { get; }

        #endregion
    }
}
