using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppGeneralSettingData : DataBase
    {
        public SettingAppGeneralSettingData()
        { }

        #region property

        public string Language { get; set; } = string.Empty;

        public string UserBackupDirectoryPath { get; set; } = string.Empty;

        #endregion
    }

    public class AppGeneralFirstData: DataBase
    {
        #region property

        public Version FirstExecuteVersion { get; set; } = new Version();

        [Timestamp(DateTimeKind.Utc)]
        public DateTime FirstExecuteTimestamp { get; set; }


        #endregion
    }
}
