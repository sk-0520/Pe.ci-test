using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Shared.Library.Model.Data
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
    public abstract class DataBase: IReadOnlyDataBase
    { }
}
