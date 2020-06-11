using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.ViewModels
{
    public class ClockAnalogClockViewModel: ViewModelSkeleton
    {
        public ClockAnalogClockViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        {
        }
    }
}
