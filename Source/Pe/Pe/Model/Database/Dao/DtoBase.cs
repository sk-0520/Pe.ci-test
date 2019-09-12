using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model.Data;

namespace ContentTypeTextNet.Pe.Main.Model.Database.Dao
{
    public interface IReadOnlyDto : IReadOnlyTransfer
    { }

    public abstract class DtoBase : TransferBase, IReadOnlyDto
    { }
}
