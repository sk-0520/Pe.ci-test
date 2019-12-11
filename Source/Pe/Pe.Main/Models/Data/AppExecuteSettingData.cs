using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppExecuteSettingData: DataBase
    {
        public SettingAppExecuteSettingData()
        { }

        #region property

        public string UserId { get; set; } = string.Empty;
        public bool SendUsageStatistics { get; set; }

        #endregion
    }
}
