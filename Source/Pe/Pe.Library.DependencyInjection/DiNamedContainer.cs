using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Library.DependencyInjection
{
    /// <summary>
    /// 名前あり・なしでの分岐を管理するコンテナ。
    /// </summary>
    public class DiNamedContainer<TData>
        where TData : class, new()
    {
        public DiNamedContainer()
        { }

        #region property

        internal ConcurrentDictionary<string, TData> Container { get; } = new ConcurrentDictionary<string, TData>();

        #endregion

        #region function

        /// <summary>
        /// 名前アクセス。
        /// </summary>
        /// <param name="name">対象の名前。無名の場合は<see cref="string.Empty"/>となるが、" " は別名と扱われる(トリムなんてしない)。</param>
        /// <returns></returns>
        public TData this[string name] => Container.GetOrAdd(name, s => new TData());

        /// <summary>
        /// 名前一覧。
        /// </summary>
        public IEnumerable<TData> Values => Container.Values;
        /// <summary>
        /// 各コンテナ一覧。
        /// </summary>
        public IEnumerable<string> Keys => Container.Keys;

        /// <summary>
        /// 名前とコンテナを対とした全件データの取得。
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, TData>[] ToArray() => Container.ToArray();

        #endregion
    }
}
