using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.ViewModels
{
    internal class ClockDigitalClockViewModel: ViewModelSkeleton
    {
        public ClockDigitalClockViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        {
        }
    }
}
