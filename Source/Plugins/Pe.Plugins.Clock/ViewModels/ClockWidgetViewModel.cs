using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Clock.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Clock.ViewModels
{
    public class ClockWidgetViewModel: ViewModelSkeleton
    {
        #region variable


        #endregion

        public ClockWidgetViewModel(ClockWidgetSetting setting, ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        {
            Setting = setting;
            ClockTimer = new DispatcherTimer(DispatcherPriority.Normal);
            ClockTimer.Tick += ClockTimer_Tick;

            Content = Setting.ClockWidgetKind switch
            {
                ClockWidgetKind.SimpleAnalog => new ClockWidgetAnalogContentViewModel(skeletonImplements.Clone(), loggerFactory),
                _ => throw new NotImplementedException(),
            };
        }

        #region property

        ClockWidgetSetting Setting { get; }

        DispatcherTimer ClockTimer { get; }

        public ClockWidgetContentBaseViewModel Content { get; }

        #endregion

        #region function

        public void StartClock()
        {
            ClockTimer.Interval = TimeSpan.FromMilliseconds(500);
            ClockTimer.Start();
        }

        public void StopClock()
        {

        }

        private void ClockTimer_Tick(object? sender, EventArgs e)
        {
            ClockTimer.Stop();

            var currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Setting.TimeZone);

            Content.CurrentTime = currentTime;
            Content.SecondsAngle = currentTime.Second * 6;
            Content.MinutesAngle = currentTime.Minute * 6;
            Content.HourAngle = (currentTime.Hour * 30) + (currentTime.Minute * 0.5);

            ClockTimer.Interval = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds - currentTime.Millisecond);
            ClockTimer.Start();
        }

        #endregion
    }
}
