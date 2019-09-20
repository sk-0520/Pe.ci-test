using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Data.Dto
{
    public interface IReadOnlyDto : IReadOnlyTransfer
    { }

    public abstract class DtoBase: TransferBase, IReadOnlyDto
    { }
}
