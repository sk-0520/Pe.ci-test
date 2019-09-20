using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class NoteContentsEntityDto : RowDtoBase
    {
        #region property

        public Guid NoteId { get; set; }
        public string? ContentKind { get; set; }

        public string? Content { get; set; }

        #endregion
    }
}
