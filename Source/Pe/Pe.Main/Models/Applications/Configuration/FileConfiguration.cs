using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class FileConfiguration: ConfigurationBase
    {
        public FileConfiguration(IConfigurationSection section)
            : base(section)
        {
            //DirectoryRemoveWaitCount = section.GetValue<int>("dir_remove_wait_count");
            //DirectoryRemoveWaitTime = section.GetValue<TimeSpan>("dir_remove_wait_time");
            //GivePriorityToFile = section.GetValue<bool>("give_priority_to_file");
        }

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
