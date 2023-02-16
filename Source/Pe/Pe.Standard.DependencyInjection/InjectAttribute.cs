using System;
using System.Collections.Generic;

namespace ContentTypeTextNet.Pe.Standard.DependencyInjection
{
    /// <summary>
    /// 注入マーク。
    /// <para><see cref="IDiContainer.New{T}(IEnumerable{object})"/> する際の対象コンストラクタを限定。</para>
    /// <para><see cref="IDiContainer.Inject{T}(T)"/> を使用する際の対象を指定。</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Parameter | AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InjectAttribute: Attribute
    {
        /// <summary>
        /// コンストラクタの限定、対象プロパティ(or フィールド)をマーク。
        /// </summary>
        public InjectAttribute()
        {
            Name = string.Empty;
        }

        /// <summary>
        /// コンストラクタの限定、対象プロパティ(or フィールド)を名前付きでマーク。
        /// </summary>
        /// <param name="name">名前。</param>
        public InjectAttribute(string name)
        {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException("empty", nameof(name));
            }

            Name = name;
        }

        #region property

        public string Name { get; }

        #endregion
    }
}
