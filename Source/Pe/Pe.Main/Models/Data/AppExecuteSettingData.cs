using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppExecuteSettingData: DataBase
    {
        public SettingAppExecuteSettingData()
        { }

        #region property

        public string UserId { get; set; } = string.Empty;
        public bool IsEnabledTelemetry { get; set; }

        #endregion
    }
}
