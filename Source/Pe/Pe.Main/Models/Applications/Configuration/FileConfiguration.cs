using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class FileConfiguration: ConfigurationBase
    {
        public FileConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public int DirectoryRemoveWaitCount { get; }
        [Configuration]
        public TimeSpan DirectoryRemoveWaitTime { get; }

        [Configuration]
        public bool GivePriorityToFile { get; }

        #endregion
    }
}
