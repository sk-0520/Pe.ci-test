using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto.Domain
{
    public interface IReadOnlyLauncherItemsIconRowDto : IReadOnlyRowDtoBase
    {
        #region property

        string Kind { get; }
        string CommandPath { get; }
        long CommandIndex { get; }
        string IconPath { get; }
        long IconIndex { get; }

        #endregion
    }

    public class LauncherItemIconRowDto : RowDtoBase, IReadOnlyLauncherItemsIconRowDto
    {
        #region IReadOnlyLauncherItemsIconRowDto

        public string Kind { get; set; }
        public string CommandPath { get; set; }
        public long CommandIndex { get; set; }
        public string IconPath { get; set; }
        public long IconIndex { get; set; }

        #endregion
    }
}
