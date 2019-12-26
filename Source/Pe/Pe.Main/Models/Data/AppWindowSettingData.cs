using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppWindowSettingData : DataBase
    {
        #region property

        public bool IsEnabled { get; set; }
        public int Count { get; set; }
        public TimeSpan Interval { get; set; }

        #endregion
    }
}
