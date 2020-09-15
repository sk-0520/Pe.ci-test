using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class NotifyLogConfiguration: ConfigurationBase
    {
        public NotifyLogConfiguration(IConfigurationSection section)
            : base(section)
        {
            NormalLogDisplayTime = section.GetValue<TimeSpan>("normal_log_display_time");
            UndoLogDisplayTime = section.GetValue<TimeSpan>("undo_log_display_time");
            CommandLogDisplayTime = section.GetValue<TimeSpan>("command_log_display_time");
            FadeoutTime = section.GetValue<TimeSpan>("fadeout_time");
        }

        #region property

        public TimeSpan NormalLogDisplayTime { get; }
        public TimeSpan UndoLogDisplayTime { get; }
        public TimeSpan CommandLogDisplayTime { get; }

        public TimeSpan FadeoutTime { get; }

        #endregion
    }
}
