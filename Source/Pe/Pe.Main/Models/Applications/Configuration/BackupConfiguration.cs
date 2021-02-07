using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class BackupConfiguration: ConfigurationBase
    {
        public BackupConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        [Configuration]
        public int SettingCount { get; }
        [Configuration]
        public int ArchiveCount { get; }

        #endregion
    }
}
