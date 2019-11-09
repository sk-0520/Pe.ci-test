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
        public string ContentKind { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public bool IsLink { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Encoding { get; set; } = string.Empty;
        public TimeSpan DelayTime { get; set; }
        public long BufferSize { get; set; }
        public TimeSpan RefreshTime { get; set; }
        public bool IsEnabledRefresh { get; set; }

        #endregion
    }
}
