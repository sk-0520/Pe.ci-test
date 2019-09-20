using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class FontsRowDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string? FamilyName { get; set; }
        public double Height { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsStrikeThrough { get; set; }

        #endregion
    }
}
