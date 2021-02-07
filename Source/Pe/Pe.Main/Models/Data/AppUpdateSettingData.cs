using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public enum UpdateKind
    {
        [EnumResource]
        None,
        [EnumResource]
        Notify,
        [EnumResource]
        Auto,
    }

    public class SettingAppUpdateSettingData: DataBase
    {
        #region property

        public UpdateKind UpdateKind { get; set; }

        #endregion
    }
}
