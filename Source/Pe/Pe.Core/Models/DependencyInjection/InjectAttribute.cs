using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Core.Models.DependencyInjection
{
    /// <summary>
    /// 注入マーク。
    /// <para><see cref="IDiContainer.New{T}(IEnumerable{object})"/> する際の対象コンストラクタを限定。</para>
    /// <para><see cref="IDiContainer.Inject{T}(T)"/> を使用する際の対象を指定。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        public InjectAttribute()
        {
            Name = string.Empty;
        }

        public InjectAttribute(string name)
        {
            Name = name;
        }

        #region property

        public string Name { get; }

        #endregion
    }
}
