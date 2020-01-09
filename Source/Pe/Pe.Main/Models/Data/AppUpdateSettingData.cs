using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum UpdateKind
    {
        None,
        Notify,
        Auto,
    }

    public class SettingAppUpdateSettingData: DataBase
    {
        #region property

        public UpdateKind UpdateKind { get; set; }

        #endregion
    }
}
