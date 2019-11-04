using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public class LauncherItemHistoriesEntityDto: CreateDtoBase
    {
        #region property

        public Guid LauncherItemId { get; }
        public string Kind { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;

        [Timestamp(DateTimeKind.Utc)]
        public DateTime LastExecuteTimestamp { get; set; }

        #endregion
    }
}
