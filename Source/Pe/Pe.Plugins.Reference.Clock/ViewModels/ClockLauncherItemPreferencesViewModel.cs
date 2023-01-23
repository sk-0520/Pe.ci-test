using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.ViewModels;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Models.Data;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.ViewModels
{
    public class ClockLauncherItemPreferencesViewModel: ViewModelSkeleton
    {
        #region variable

        private string formattedValue = string.Empty;

        #endregion

        public ClockLauncherItemPreferencesViewModel(ClockLauncherItemSetting setting, ISkeletonImplements skeletonImplements, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(skeletonImplements, dispatcherWrapper, loggerFactory)
        {
            Setting = setting;

            ApplyFormat();
        }

        #region property

        private ClockLauncherItemSetting Setting { get; }

        #endregion

        #region command

        public string Format
        {
            get => Setting.Format;
            set
            {
                SetPropertyValue(Setting, value, nameof(Setting.Format));
                ApplyFormat();
            }
        }

        public string FormattedValue
        {
            get => this.formattedValue;
            private set => SetProperty(ref this.formattedValue, value);
        }

        #endregion

        #region function

        private void ApplyFormat()
        {
            try {
                FormattedValue = GetFormattedTimestamp(Format, DateTime.Now);
            } catch(Exception ex) {
                FormattedValue = ex.Message;
            }
        }

        private string GetFormattedTimestamp(string format, DateTime dateTime)
        {
            return dateTime.ToString(format, CultureInfo.InvariantCulture);
        }

        #endregion
    }
}
