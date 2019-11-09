using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherFilesEntityPathDto : DtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }
        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;


        #endregion
    }

    public class LauncherFilesEntityDto: CommonDtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }

        public string File { get; set; } = string.Empty;
        public string Option { get; set; } = string.Empty;
        public string WorkDirectory { get; set; } = string.Empty;

        public bool IsEnabledCustomEnvVar { get; set; }
        public bool IsEnabledStandardIo { get; set; }
        public bool RunAdministrator { get; set; }

        #endregion
    }

}
