using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppStandardInputOutputSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string OutputForegroundColor { get; set; } = string.Empty;
        public string OutputBackgroundColor { get; set; } = string.Empty;
        public string ErrorForegroundColor { get; set; } = string.Empty;
        public string ErrorBackgroundColor { get; set; } = string.Empty;
        public bool IsTopmost { get; set; }

        #endregion
    }
}
