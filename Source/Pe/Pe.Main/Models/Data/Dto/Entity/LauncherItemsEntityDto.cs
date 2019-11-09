using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Entity
{
    public interface IReadOnlyLauncherItemsRowDto : IReadOnlyRowDtoBase
    {
        #region property

        Guid LauncherItemId { get; }

        string Code { get; }
        string Name { get; }
        string Kind { get; }
        string IconPath { get; }
        long IconIndex { get; }
        bool IsEnabledCommandLauncher { get; }
        long ExecuteCount { get; }
        string Comment { get; }

        #endregion
    }

    public class LauncherItemsRowDto : RowDtoBase, IReadOnlyLauncherItemsRowDto
    {
        #region IReadOnlyLauncherItemRowDto

        public Guid LauncherItemId { get; set; }

        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Kind { get; set; } = string.Empty;
        public string IconPath { get; set; } = string.Empty;
        public long IconIndex { get; set; }
        public bool IsEnabledCommandLauncher { get; set; }
        public long ExecuteCount { get; set; }
        public string Comment { get; set; } = string.Empty;

        #endregion
    }


}
