using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppUpdateSettingEntityDto : CommonDtoBase
    {
        #region property

        public bool CheckReleaseVersion { get; set; }
        public bool CheckRcVersion { get; set; }
        public Version IgnoreVersion { get; set; } = new Version(0, 0, 0, 0);

        #endregion
    }
}
