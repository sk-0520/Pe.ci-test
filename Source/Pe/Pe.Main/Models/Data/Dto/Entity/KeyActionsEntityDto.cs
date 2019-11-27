using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class KeyActionsEntityDto : CommonDtoBase
    {
        #region property

        public Guid KeyActionId { get; set; }
        public string KeyActionKind { get; set; } = string.Empty;
        public string KeyActionContent { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        #endregion
    }
}
