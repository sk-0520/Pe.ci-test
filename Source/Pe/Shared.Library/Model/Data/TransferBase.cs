using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Data
{
    public interface IReadOnlyTransfer: IReadOnlyData
    { }

    [Serializable, DataContract]
    public class TransferBase: DataBase, IReadOnlyTransfer
    { }
}
