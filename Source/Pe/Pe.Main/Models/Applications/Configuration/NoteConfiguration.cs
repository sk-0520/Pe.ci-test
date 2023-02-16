using System;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class NoteConfiguration: ConfigurationBase
    {
        public NoteConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public double MovingOpacity { get; }

        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutAbsoluteSize { get; }
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutRelativeSize { get; }
        [Configuration(rootConvertMethodName: nameof(ConvertMinMaxDefault))]
        public MinMaxDefault<double> FontSize { get; }

        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenCompactWaitTime { get; }
        [Configuration(rootConvertMethodName: nameof(ConvertMinMax))]
        public MinMax<TimeSpan> HiddenBlindWaitTime { get; }

        #endregion
    }
}
