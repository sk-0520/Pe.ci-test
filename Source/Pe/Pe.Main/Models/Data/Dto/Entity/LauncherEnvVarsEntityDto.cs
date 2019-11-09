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
        public string EnvName { get; set; } = string.Empty;
        public string EnvValue { get; set; } = string.Empty;

        #endregion
    }
}
