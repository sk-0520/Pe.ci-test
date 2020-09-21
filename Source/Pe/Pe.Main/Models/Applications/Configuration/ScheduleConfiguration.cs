using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class ScheduleConfiguration: ConfigurationBase
    {
        public ScheduleConfiguration(IConfigurationSection section)
            : base(section)
        {

            //LowSchedulerTime = section.GetValue<TimeSpan>("low_scheduler_time");
            //LauncherItemIconRefresh = section.GetValue<string>("launcher_item_icon_refresh");
        }

        #region function

        [Configuration]
        public TimeSpan LowSchedulerTime { get; }
        [Configuration]
        public string LauncherItemIconRefresh { get; } = default!;

        #endregion
    }
}
