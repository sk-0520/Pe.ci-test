using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model.Data
{
    /// <summary>
    /// なんかしらの読み込み専用転送可能データ。
    /// </summary>
    public interface IReadOnlyTransfer : IReadOnlyData
    { }

    /// <summary>
    /// なんかしらの転送可能データ。
    /// </summary>
    [Serializable, DataContract]
    public class TransferBase : DataBase, IReadOnlyTransfer
    { }
}
