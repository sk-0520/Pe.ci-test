using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models.Data.Dto.Domain
{
    public interface IReadOnlyLauncherItemsIconRowDto : IReadOnlyRowDtoBase
    {
        #region property

        string? Kind { get; }
        string? FilePath { get; }
        long FileIndex { get; }
        string? IconPath { get; }
        long IconIndex { get; }

        #endregion
    }

    public class LauncherItemIconRowDto : RowDtoBase, IReadOnlyLauncherItemsIconRowDto
    {
        #region IReadOnlyLauncherItemsIconRowDto

        public string? Kind { get; set; }
        public string? FilePath { get; set; }
        public long FileIndex { get; set; }
        public string? IconPath { get; set; }
        public long IconIndex { get; set; }

        #endregion
    }
}
