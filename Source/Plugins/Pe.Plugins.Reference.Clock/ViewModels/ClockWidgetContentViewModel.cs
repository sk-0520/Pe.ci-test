using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public abstract class ClockWidgetContentBaseViewModel: ViewModelSkeleton
    {
        #region property

        DateTime _currentTime;
        double _hourAngle;
        double _minutesAngle;
        double _secondsAngle;

        double _hourOppositeAngle;
        double _minutesOppositeAngle;
        double _secondsOppositeAngle;

        bool _isPm;

        #endregion
        protected ClockWidgetContentBaseViewModel(ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper,ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        { }

        #region property

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

        public double HourOppositeAngle
        {
            get => this._hourOppositeAngle;
            set => SetProperty(ref this._hourOppositeAngle, value);
        }
        public double MinutesOppositeAngle
        {
            get => this._minutesOppositeAngle;
            set => SetProperty(ref this._minutesOppositeAngle, value);
        }
        public double SecondsOppositeAngle
        {
            get => this._secondsOppositeAngle;
            set => SetProperty(ref this._secondsOppositeAngle, value);
        }


        public bool IsPm
        {
            get => this._isPm;
            set => SetProperty(ref this._isPm, value);
        }

        #endregion

        #region function

        public void SetTime(DateTime dateTime)
        {

            CurrentTime = dateTime;
            SecondsAngle = dateTime.Second * 6.0;
            MinutesAngle = dateTime.Minute * 6.0;
            HourAngle = (dateTime.Hour * 30.0) + (dateTime.Minute * 0.5);

            SecondsOppositeAngle = 180.0 + SecondsAngle;
            MinutesOppositeAngle = 180.0 + MinutesAngle;
            HourOppositeAngle = 180.0 + HourAngle;
            IsPm = 12 <= dateTime.Hour;
        }

        #endregion
    }

    public class ClockWidgetSimpleAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetSimpleAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            HourAngels = Enumerable.Range(0, 12).Select(i => i * 30.0).ToList();
            SecondsAngels = Enumerable.Range(0, 60).Select(i => i * 6.0).Except(HourAngels).ToList();
        }

        #region property

        public IReadOnlyList<double> HourAngels { get; }
        public IReadOnlyList<double> SecondsAngels { get; }

        #endregion
    }

    public class ClockWidgetJaggyAnalogClockContentViewModel: ClockWidgetContentBaseViewModel
    {
        public ClockWidgetJaggyAnalogClockContentViewModel(ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        { }
    }
}
