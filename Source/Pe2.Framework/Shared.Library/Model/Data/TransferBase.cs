using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Data
{
    /// <summary>
    /// なんかしらの読み込み専用転送可能データ。
    /// </summary>
    public interface IReadOnlyTransfer: IReadOnlyData
    { }

    /// <summary>
    /// なんかしらの転送可能データ。
    /// </summary>
    [Serializable, DataContract]
    public class TransferBase: DataBase, IReadOnlyTransfer
    { }
}
