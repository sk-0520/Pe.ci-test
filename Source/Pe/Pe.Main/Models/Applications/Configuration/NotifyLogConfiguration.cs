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
            //NormalLogDisplayTime = section.GetValue<TimeSpan>("normal_log_display_time");
            //UndoLogDisplayTime = section.GetValue<TimeSpan>("undo_log_display_time");
            //CommandLogDisplayTime = section.GetValue<TimeSpan>("command_log_display_time");
            //FadeoutTime = section.GetValue<TimeSpan>("fadeout_time");
        }

        #region property

        [Configuration]
        public TimeSpan NormalLogDisplayTime { get; }
        [Configuration]
        public TimeSpan UndoLogDisplayTime { get; }
        [Configuration]
        public TimeSpan CommandLogDisplayTime { get; }

        [Configuration]
        public TimeSpan FadeoutTime { get; }

        #endregion
    }
}
