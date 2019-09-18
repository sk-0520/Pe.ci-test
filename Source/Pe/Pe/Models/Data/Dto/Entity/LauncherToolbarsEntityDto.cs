using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherToolbarsDisplayRowDto : CommonDtoBase
    {
        #region property

        public Guid LauncherToolbarId { get; set; }
        public Guid LauncherGroupId { get; set; }
        public string? PositionKind { get; set; }
        public string? Direction { get; set; }
        public string? IconScale { get; set; }
        public Guid FontId { get; set; }
        public string? AutoHideTimeout { get; set; }
        public long TextWidth { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTopmost { get; set; }
        public bool IsAutoHide { get; set; }
        public bool IsIconOnly { get; set; }

        #endregion
    }
}
