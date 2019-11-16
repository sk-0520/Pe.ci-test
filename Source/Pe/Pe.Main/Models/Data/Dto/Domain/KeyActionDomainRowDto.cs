using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain
{
    public class KeyActionRowDto : RowDtoBase
    {
        #region function

        public Guid KeyActionId { get; set; }
        public string KeyActionKind { get; set; } = string.Empty;
        public string KeyActionContent { get; set; } = string.Empty;
        public string KeyActionOption { get; set; } = string.Empty;

        public string Key { get; set; } = string.Empty;
        public string Shift { get; set; } = string.Empty;
        public string Contrl { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public string Super { get; set; } = string.Empty;

        #endregion
    }
}
