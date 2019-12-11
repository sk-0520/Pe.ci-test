using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class AppExecuteSettingEntityDto : CommonDtoBase
    {
        #region property
        public bool Accepted { get; set; }
        public Version? FirstVersion { get; set; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime FirstTimestamp { get; set; }
        public Version? LastVersion { get; set; }
        [Timestamp(DateTimeKind.Utc)]
        public DateTime LastTimestamp { get; set; }
        public long ExecuteCount { get; set; }
        public string UserId { get; set; } = string.Empty;
        public bool SendUsageStatistics { get; set; }
        #endregion
    }

}
