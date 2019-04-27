using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    public class FontsRowDto : CommonDtoBase
    {
        #region property

        public Guid FontId { get; set; }
        public string FamilyName { get; set; }
        public double Height { get; set; }
        public bool Bold { get; set; }
        public bool Italic { get; set; }
        public bool Underline { get; set; }
        public bool Strike { get; set; }

        #endregion
    }
}
