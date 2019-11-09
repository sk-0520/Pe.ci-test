using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherGroupsRowDto : RowDtoBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public string Kind { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ImageName { get; set; } = string.Empty;
        public string ImageColor { get; set; } = string.Empty;
        public long Sequence { get; set; }

        #endregion
    }
}
