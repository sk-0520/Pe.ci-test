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

        DateTime _currentTime;

        double _hourAngle;
        double _minutesAngle;
        double _secondsAngle;

        #endregion

        public ClockWidgetViewModel(ClockWidgetSetting setting, ISkeletonImplements skeletonImplements, ILoggerFactory loggerFactory)
            : base(skeletonImplements, loggerFactory)
        {
            Setting = setting;
            ClockTimer = new DispatcherTimer(DispatcherPriority.Normal);
            ClockTimer.Tick += ClockTimer_Tick;
        }

        #region property

        ClockWidgetSetting Setting { get; }

        DispatcherTimer ClockTimer { get; }

        public DateTime CurrentTime
        {
            get => this._currentTime;
            set => SetProperty(ref this._currentTime, value);
        }

        public ClockWidgetKind ClockWidgetKind => Setting.ClockWidgetKind;

        public double HourAngle
        {
            get => this._hourAngle;
            private set => SetProperty(ref this._hourAngle, value);
        }
        public double MinutesAngle
        {
            get => this._minutesAngle;
            private set => SetProperty(ref this._minutesAngle, value);
        }
        public double SecondsAngle
        {
            get => this._secondsAngle;
            private set => SetProperty(ref this._secondsAngle, value);
        }


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

            CurrentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Setting.TimeZone);

            SecondsAngle = CurrentTime.Second * 6;
            MinutesAngle = CurrentTime.Minute * 6;
            HourAngle = (CurrentTime.Hour * 30) + (CurrentTime.Minute * 0.5);

            ClockTimer.Interval = TimeSpan.FromMilliseconds(TimeSpan.FromSeconds(1).TotalMilliseconds - CurrentTime.Millisecond);
            ClockTimer.Start();
        }

        #endregion
    }
}
