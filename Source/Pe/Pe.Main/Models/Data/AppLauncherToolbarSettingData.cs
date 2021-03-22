using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class AppLauncherToolbarSettingData: DataBase
    {
        #region property

        public LauncherToolbarContentDropMode ContentDropMode { get; set; }
        public LauncherGroupPosition GroupMenuPosition { get; set; }

        #endregion
    }
}
