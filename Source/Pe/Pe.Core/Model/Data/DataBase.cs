using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Model.Data
{
    /// <summary>
    /// なんかしらの保存可能読み込み専用データ。
    /// </summary>
    public interface IReadOnlyDataBase
    { }

    /// <summary>
    /// なんかしらのシリアライズ可能データ。
    /// </summary>
    [Serializable, DataContract]
    public abstract class DataBase : IReadOnlyDataBase
    { }
}
