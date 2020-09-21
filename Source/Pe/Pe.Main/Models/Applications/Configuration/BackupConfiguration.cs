using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class BackupConfiguration: ConfigurationBase
    {
        public BackupConfiguration(IConfigurationSection section)
            : base(section)
        {
            //SettingCount = section.GetValue<int>("setting_count");
            //ArchiveCount = section.GetValue<int>("archive_count");
        }

        #region property

        [Configuration]
        public int SettingCount { get; }
        [Configuration]
        public int ArchiveCount { get; }

        #endregion
    }
}
