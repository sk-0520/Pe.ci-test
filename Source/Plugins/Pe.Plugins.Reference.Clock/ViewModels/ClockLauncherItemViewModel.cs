using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemViewModel: ViewModelSkeleton
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

        internal ClockLauncherItemViewModel(ClockLauncherItem item, ISkeletonImplements skeletonImplements, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Item = item;
            PlatformTheme = platformTheme;

            Item.PropertyChanged += Item_PropertyChanged;

            PlatformTheme.Changed += PlatformTheme_Changed;
            ApplyTheme();
        }

        #region property

        ClockLauncherItem Item { get; }
        IPlatformTheme PlatformTheme { get; }

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
                static double GetBrightness(Color color)
                {
                    return (((color.R * 299) + (color.G * 587) + (color.B * 114)) / 1000.0);
                }

                static Color GetAutoColor(Color color)
                {
                    var brightness = GetBrightness(color);
                    if(brightness > 160) {
                        return Colors.Black;
                    } else {
                        return Colors.White;
                    }
                }

                var color = PlatformTheme.GetTaskbarColor();
                HourForeground = new SolidColorBrush(GetAutoColor(color));
                MinutesForeground = new SolidColorBrush(GetAutoColor(color));
                SecondsForeground = new SolidColorBrush(GetAutoColor(color));
                BaseForeground = new SolidColorBrush(GetAutoColor(color));
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

        private void Item_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(Item.CurrentTime)) {
                CurrentTime = Item.CurrentTime;
                SecondsAngle = Item.CurrentTime.Second * 6.0;
                MinutesAngle = Item.CurrentTime.Minute * 6.0;
                HourAngle = (Item.CurrentTime.Hour * 30.0) + (Item.CurrentTime.Minute * 0.5);
            }
        }

        private void PlatformTheme_Changed(object? sender, EventArgs e)
        {
            ApplyTheme();
        }

    }
}
