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
        {
            LayoutAbsoluteSize = GetSize(section, "layout_absolute_size");
            LayoutRelativeSize = GetSize(section, "layout_relative_size");
            FontSize = GetMinMaxDefault<double>(section, "font_size");


            HiddenCompactWaitTime = section.GetValue<TimeSpan>("hidden_compact_wait_time");
            HiddenBlindWaitTime = section.GetValue<TimeSpan>("hidden_blind_wait_time");
        }

        #region property

        public Size LayoutAbsoluteSize { get; }
        public Size LayoutRelativeSize { get; }
        public MinMaxDefault<double> FontSize { get; }

        public TimeSpan HiddenCompactWaitTime { get; }
        public TimeSpan HiddenBlindWaitTime { get; }

        #endregion
    }
}
