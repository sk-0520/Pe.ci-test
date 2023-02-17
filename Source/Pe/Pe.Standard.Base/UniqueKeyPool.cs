using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace ContentTypeTextNet.Pe.Standard.Base
{
    /// <summary>
    /// 呼び出し側で一意なオブジェクトを生成するヘルパー。
    /// <para><see cref="Database.IDatabaseLazyWriter.Stock(Action{Database.IDatabaseTransaction}, object)"/>の使用のみに特化。</para>
    /// </summary>
    public class UniqueKeyPool
    {
        #region property

        private ConcurrentDictionary<string, object> Pool { get; } = new ConcurrentDictionary<string, object>();

        #endregion

        #region function

        /// <summary>
        /// 呼び出し箇所で一意なオブジェクトを生成。
        /// <para>継承クラスにて一意なデータを取得できるように調整して使用することは想定していない。</para>
        /// </summary>
        /// <param name="callerMemberName"><see cref="CallerMemberNameAttribute"/></param>
        /// <param name="callerLineNumber"><see cref="CallerLineNumberAttribute"/></param>
        /// <returns></returns>
        public object Get([CallerMemberName] string callerMemberName = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            var sb = new StringBuilder(callerMemberName.Length + 1 + "2147483647".Length);
            sb.Append(callerMemberName);
            sb.Append('.');
            sb.Append(callerLineNumber);

            var result = Pool.GetOrAdd(sb.ToString(), k => new object());
            return result;
        }

        #endregion
    }

}
