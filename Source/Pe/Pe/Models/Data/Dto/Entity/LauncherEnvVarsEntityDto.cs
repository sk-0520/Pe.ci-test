using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherEnvVarsEntityDto: CommonDtoBase
    {
        #region property

        public Guid LauncherItemId { get; set; }
        public string? EnvName { get; set; }
        public string? Kind { get; set; }
        public string? EnvValue { get; set; }

        #endregion
    }
}
