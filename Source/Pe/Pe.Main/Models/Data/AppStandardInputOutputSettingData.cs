using System;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Data
{
    public class SettingAppStandardInputOutputSettingData: DataBase
    {
        #region property

        public Guid FontId { get; set; }
        public Color OutputForegroundColor { get; set; }
        public Color OutputBackgroundColor { get; set; }
        public Color ErrorForegroundColor { get; set; }
        public Color ErrorBackgroundColor { get; set; }
        public bool IsTopmost { get; set; }

        #endregion
    }
}
