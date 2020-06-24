using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockSettingViewModel: ViewModelSkeleton
    {
        public ClockSettingViewModel(ClockWidgetSetting widgetSetting, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            WidgetSetting = widgetSetting;
        }

        #region property

        internal ClockWidgetSetting WidgetSetting { get; }

        public TimeZoneInfo WidgetTimeZone
        {
            get => WidgetSetting.TimeZone;
            set => SetPropertyValue(WidgetSetting, value, nameof(WidgetSetting.TimeZone));
        }

        public IReadOnlyList<TimeZoneInfo> WidgetTimeZoneItems { get; } = new List<TimeZoneInfo>(TimeZoneInfo.GetSystemTimeZones());

        public ClockWidgetKind WidgetKind
        {
            get => WidgetSetting.ClockWidgetKind;
            set => SetPropertyValue(WidgetSetting, value, nameof(WidgetSetting.ClockWidgetKind));
        }

        public IReadOnlyList<ClockWidgetKind> WidgetKindItems { get; } = Enum.GetValues(typeof(ClockWidgetKind)).Cast<ClockWidgetKind>().ToList();

        #endregion
    }
}
