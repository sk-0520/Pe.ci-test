using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppUpdateSettingData: DataBase
    {
        #region property

        public bool IsCheckReleaseVersion { get; set; }
        public bool IsCheckRcVersion { get; set; }

        #endregion
    }
}
