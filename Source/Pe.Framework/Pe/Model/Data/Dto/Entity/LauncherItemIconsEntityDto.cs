using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Entity
{
    class LauncherItemIconsDto : CreateDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string IconScale { get; set; }
        public long Sequence { get; set; }
        public byte[] Image { get; set; }

        #endregion

    }
}
