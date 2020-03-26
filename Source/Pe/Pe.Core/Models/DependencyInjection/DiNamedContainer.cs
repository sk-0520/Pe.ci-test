using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
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

        ConcurrentDictionary<string, TData> Container { get; } = new ConcurrentDictionary<string, TData>();

        #endregion

        #region function

        public TData this[string name] => Container.GetOrAdd(name, s => new TData());

        public IEnumerable<TData> GetAllData() => Container.Values;

        #endregion
    }
}
