using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemControlViewModel: ViewModelSkeleton
    {
        #region variable

        DateTime _currentTime;
        double _hourAngle;
        double _minutesAngle;
        double _secondsAngle;

        Brush? _hourForeground;
        Brush? _minutesForeground;
        Brush? _secondsForeground;
        Brush? _baseForeground;

        #endregion

        internal ClockLauncherItemControlViewModel(ClockLauncherItem item, ISkeletonImplements skeletonImplements, IPlatformTheme platformTheme, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Item = item;
            PlatformTheme = platformTheme;
            MediaConverter = mediaConverter;

            Item.PropertyChanged += Item_PropertyChanged;

            PlatformTheme.Changed += PlatformTheme_Changed;
            ApplyTheme();
        }

        #region property

        ClockLauncherItem Item { get; }
        IPlatformTheme PlatformTheme { get; }
        IMediaConverter MediaConverter { get; }
        public DateTime CurrentTime
        {
            get => this._currentTime;
            set => SetProperty(ref this._currentTime, value);
        }

        public double HourAngle
        {
            get => this._hourAngle;
            set => SetProperty(ref this._hourAngle, value);
        }
        public double MinutesAngle
        {
            get => this._minutesAngle;
            set => SetProperty(ref this._minutesAngle, value);
        }
        public double SecondsAngle
        {
            get => this._secondsAngle;
            set => SetProperty(ref this._secondsAngle, value);
        }

        public Brush? HourForeground
        {
            get => this._hourForeground;
            set => SetProperty(ref this._hourForeground, value);
        }

        public Brush? MinutesForeground
        {
            get => this._minutesForeground;
            set => SetProperty(ref this._minutesForeground, value);
        }

        public Brush? SecondsForeground
        {
            get => this._secondsForeground;
            set => SetProperty(ref this._secondsForeground, value);
        }

        public Brush? BaseForeground
        {
            get => this._baseForeground;
            set => SetProperty(ref this._baseForeground, value);
        }

        #endregion

        #region function

        void ApplyTheme()
        {
            DispatcherWrapper.Begin(() => {
                var color = PlatformTheme.GetTaskbarColor();
                HourForeground = new SolidColorBrush(MediaConverter.GetAutoColor(color));
                MinutesForeground = new SolidColorBrush(MediaConverter.GetAutoColor(color));
                SecondsForeground = new SolidColorBrush(MediaConverter.GetAutoColor(color));
                BaseForeground = new SolidColorBrush(MediaConverter.GetAutoColor(color));
            }, System.Windows.Threading.DispatcherPriority.Render);
        }

        #endregion

        #region ViewModelSkeleton

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                Item.PropertyChanged -= Item_PropertyChanged;
                PlatformTheme.Changed -= PlatformTheme_Changed;
            }
            base.Dispose(disposing);
        }

        #endregion

        private void Item_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Item.CurrentTime)) {
                CurrentTime = Item.CurrentTime;
                SecondsAngle = ClockUtility.GetSecondsAngle(CurrentTime);
                MinutesAngle = ClockUtility.GetMinuteAngle(CurrentTime);
                HourAngle = ClockUtility.GetHourAngle(CurrentTime);
            }
        }

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            ApplyTheme();
        }

    }
}
