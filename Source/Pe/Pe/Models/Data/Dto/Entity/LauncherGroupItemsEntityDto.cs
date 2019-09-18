using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherGroupItemsRowDto : RowDtoBase
    {
        #region property

        public Guid LauncherGroupId { get; set; }
        public Guid LauncherItemId { get; set; }
        public long Sort { get; set; }

        #endregion
    }
}
