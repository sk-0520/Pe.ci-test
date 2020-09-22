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
        { }

        #region function

        [Configuration]
        public TimeSpan LowSchedulerTime { get; }
        [Configuration]
        public string LauncherItemIconRefresh { get; } = default!;

        #endregion
    }
}
