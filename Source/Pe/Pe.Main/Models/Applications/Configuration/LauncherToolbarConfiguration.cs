using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class LauncherToolbarConfiguration: ConfigurationBase
    {
        public LauncherToolbarConfiguration(IConfigurationSection section)
            : base(section)
        {
            AutoHideShowWaitTime = section.GetValue<TimeSpan>("auto_hide_show_wait_time");
        }

        #region property

        public TimeSpan AutoHideShowWaitTime { get; }

        #endregion
    }
}
