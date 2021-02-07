using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppPlatformSettingData: DataBase
    {
        #region property

        public bool SuppressSystemIdle { get; set; }
        public bool SupportExplorer { get; set; }

        #endregion
    }
}
