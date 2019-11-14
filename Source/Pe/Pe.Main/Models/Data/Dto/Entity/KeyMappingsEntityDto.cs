using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class KeyMappingsEntityDto : CreateDtoBase
    {
        #region property

        public Guid KeyActionId { get; set; }
        public long Sequence { get; set; }

        public string Key { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Control { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public string Super { get; set; } = string.Empty;
        #endregion
    }
}
