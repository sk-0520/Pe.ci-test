using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppNoteSettingEntityDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string TitleKind { get; set; } = string.Empty;
        public string LayoutKind { get; set; } = string.Empty;
        public string ForegroundColor { get; set; } = string.Empty;
        public string BackgroundColor { get; set; } = string.Empty;
        public bool IsTopmost { get; set; }


        #endregion
    }
}
