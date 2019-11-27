using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class KeyOptionsEntityDto : CreateDtoBase
    {
        #region property

        public Guid KeyActionId { get; set; }
        public string KeyOptionName { get; set; } = string.Empty;
        public string KeyOptionValue { get; set; } = string.Empty;

        #endregion
    }
}
