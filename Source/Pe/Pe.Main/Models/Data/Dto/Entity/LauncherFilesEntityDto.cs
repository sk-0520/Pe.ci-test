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
        public string? File { get; set; }
        public string? Option { get; set; }
        public string? WorkDirectory { get; set; }


        #endregion
    }

    public class LauncherFilesEntityDto: CommonDtoBase
    {
        #region property

        Guid LauncherItemId { get; set; }

        public string? File { get; set; }
        public string? Option { get; set; }
        public string? WorkDirectory { get; set; }

        public bool IsEnabledCustomEnvVar { get; set; }
        public bool IsEnabledStandardIo { get; set; }
        public bool RunAdministrator { get; set; }

        #endregion
    }

}
