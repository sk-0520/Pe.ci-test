using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Library.Database
{
    /// <summary>
    /// データベースとの会話用インターフェイス。
    /// </summary>
    /// <remarks>
    /// <para><see cref="IDatabaseReader"/>, <see cref="IDatabaseWriter"/>による明確な分離状態で処理するのは現実的でないため本IFで統合して扱う。</para>
    /// </remarks>
    public interface IDatabaseContext: IDatabaseReader, IDatabaseWriter
    { }
}
