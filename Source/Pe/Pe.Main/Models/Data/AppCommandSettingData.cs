using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppCommandSettingData : DataBase
    {
        public SettingAppCommandSettingData()
        { }

        #region property

        public Guid FontId { get; set; }
        public IconBox IconBox { get; set; }
        public double Width { get; set; }
        public TimeSpan HideWaitTime { get; set; }
        public bool FindTag { get; set; }

        #endregion
    }
}
