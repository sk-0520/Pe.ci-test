using System;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppGeneralSettingData: DataBase
    {
        public SettingAppGeneralSettingData()
        { }

        #region property

        public string Language { get; set; } = string.Empty;

        public string UserBackupDirectoryPath { get; set; } = string.Empty;

        public Guid ThemePluginId { get; set; }

        #endregion
    }

    public class AppGeneralFirstData: DataBase
    {
        #region property

        public Version FirstExecuteVersion { get; set; } = new Version();

        [DateTimeKind(DateTimeKind.Utc)]
        public DateTime FirstExecuteTimestamp { get; set; }


        #endregion
    }
}
