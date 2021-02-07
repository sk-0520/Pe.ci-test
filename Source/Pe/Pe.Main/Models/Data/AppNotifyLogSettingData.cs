using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppNotifyLogSettingData: DataBase
    {
        #region property

        public bool IsVisible { get; set; }
        public NotifyLogPosition Position { get; set; }

        #endregion
    }
}
