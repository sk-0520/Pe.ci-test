using System;
using System.Runtime.Serialization;

namespace ContentTypeTextNet.Pe.Core.Models.Data
{
    /// <summary>
    /// なんかしらの読み込み専用転送可能データ。
    /// </summary>
    public interface IReadOnlyTransfer
    { }

    /// <summary>
    /// なんかしらの転送可能データ。
    /// </summary>
    [Serializable, DataContract]
    public class TransferBase: IReadOnlyTransfer
    { }
}
