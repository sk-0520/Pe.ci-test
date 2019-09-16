using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Data;

namespace ContentTypeTextNet.Pe.Main.Models.Database.Dao
{
    public interface IReadOnlyDto : IReadOnlyTransfer
    { }

    public abstract class DtoBase : TransferBase, IReadOnlyDto
    { }
}
