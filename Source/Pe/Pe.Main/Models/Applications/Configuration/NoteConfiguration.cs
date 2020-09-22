using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class NoteConfiguration: ConfigurationBase
    {
        public NoteConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutAbsoluteSize { get; }
        [Configuration(rootConvertMethodName: nameof(ConvertSize))]
        public Size LayoutRelativeSize { get; }
        [Configuration(rootConvertMethodName: nameof(ConvertMinMaxDefault))]
        public MinMaxDefault<double> FontSize { get; }

        [Configuration]
        public TimeSpan HiddenCompactWaitTime { get; }
        [Configuration]
        public TimeSpan HiddenBlindWaitTime { get; }

        #endregion
    }
}
