using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    class LauncherItemIconsDto : CreateDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string? IconScale { get; set; }
        public long Sequence { get; set; }
        public byte[]? Image { get; set; }

        #endregion

    }
}
