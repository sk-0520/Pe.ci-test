using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.ViewModels
{
    public class EyesWidgetViewModel: ViewModelSkeleton
    {
        public EyesWidgetViewModel(ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        { }
    }
}
