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
        public bool IsLink { get; set; }
        public string? Address { get; set; }
        public string? Encoding { get; set; }
        public TimeSpan DelayTime { get; set; }
        public long BufferSize { get; set; }
        public TimeSpan RefreshTime { get; set; }
        public bool IsEnabledRefresh { get; set; }

        #endregion
    }
}
