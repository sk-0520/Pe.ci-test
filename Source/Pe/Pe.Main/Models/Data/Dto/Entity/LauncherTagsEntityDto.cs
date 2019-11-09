using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherTagsRowDto : RowDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string TagName { get; set; } = string.Empty;

        #endregion
    }
}
