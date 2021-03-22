using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class LauncherToolbarConfiguration: ConfigurationBase
    {
        public LauncherToolbarConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public TimeSpan AutoHideShowWaitTime { get; }

        #endregion
    }
}
