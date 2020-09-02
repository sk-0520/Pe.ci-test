using System;
using System.Collections.Generic;
using System.Media;
using System.Text;
using System.Windows.Input;
using System.Windows.Threading;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemWindowViewModel: ViewModelSkeleton
    {
        #region variable

        double _progressValue;
        TimeSpan _currentTime;

        TimeSpan _selectedTime;
        bool _canStart = true;
        bool _canStop;

        ICommand? _startCommand;
        ICommand? _stopCommand;

        #endregion

        internal ClockLauncherItemWindowViewModel(ClockLauncherItem item, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Item = item;

            Timer = new DispatcherTimer();
            Timer.Interval = TimeSpan.FromMilliseconds(25);
            Timer.Tick += Timer_Tick;

            this._selectedTime = TimeSpan.FromMinutes(3);
        }

        #region property

        ClockLauncherItem Item { get; }
        DispatcherTimer Timer { get; }

        [DateTimeKind(DateTimeKind.Utc)]
        DateTime StartTimestamp { get; set; }

        public TimeSpan CurrentTime
        {
            get => this._currentTime;
            private set => SetProperty(ref this._currentTime, value);
        }

        public double ProgressValue
        {
            get => this._progressValue;
            private set => SetProperty(ref this._progressValue, value);
        }

        public bool CanStart
        {
            get => this._canStart;
            private set => SetProperty(ref this._canStart, value);
        }
        public bool CanStop
        {
            get => this._canStop;
            private set => SetProperty(ref this._canStop, value);
        }

        public TimeSpan SelectedTime
        {
            get => this._selectedTime;
            set => SetProperty(ref this._selectedTime, value);
        }

        public IReadOnlyList<TimeSpan> Times { get; } = new TimeSpan[] {
            TimeSpan.FromSeconds(10),
            TimeSpan.FromSeconds(30),
            TimeSpan.FromMinutes(1),
            TimeSpan.FromMinutes(2),
            TimeSpan.FromMinutes(3),
            TimeSpan.FromMinutes(4),
            TimeSpan.FromMinutes(5),
        };

        #endregion

        #region command

        public ICommand StartCommand => this._startCommand ??= CreateCommand(() => {
            CanStart = false;
            CanStop = true;
            StartTimestamp = DateTime.UtcNow;
            ProgressValue = 0;
            Timer.Start();
        });
        public ICommand StopCommand => this._stopCommand ??= CreateCommand(() => {
            Timer.Stop();
            CanStart = true;
            CanStop = false;
        });

        #endregion

        #region function

        void StopTimer()
        {
            ProgressValue = 1;
            Timer.Stop();
            CanStart = true;
            CanStop = false;
            SystemSounds.Hand.Play();
        }

        #endregion

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CurrentTime = DateTime.UtcNow - StartTimestamp;
            ProgressValue = CurrentTime.TotalMilliseconds / SelectedTime.TotalMilliseconds;
            if(1 <= ProgressValue) {
                StopTimer();
            }
        }
    }
}
