using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    /// <summary>
    /// [アセンブリ] プラグインカテゴリ設定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginCategoryAttribute: Attribute
    {
        /// <summary>
        /// [アセンブリ] プラグインカテゴリ設定。
        /// </summary>
        /// <param name="primary">メインカテゴリ。<seealso cref="PluginCategories"/></param>
        public PluginCategoryAttribute(string primary)
        {
            Primary = primary;
            Secondaries = Array.Empty<string>();
        }

        /// <summary>
        /// [アセンブリ] プラグインカテゴリ設定。
        /// </summary>
        /// <param name="primary">メインカテゴリ。<seealso cref="PluginCategories"/></param>
        /// <param name="secondary">サブカテゴリ。<seealso cref="PluginCategories"/></param>
        /// <param name="secondaries">サブカテゴリ(3以降)。<seealso cref="PluginCategories"/></param>
        public PluginCategoryAttribute(string primary, string secondary, params string[] secondaries)
        {
            Primary = primary;
            var list = new List<string>(1 + secondaries.Length);
            list.Add(secondary);
            list.AddRange(secondaries);
            Secondaries = list;
        }

        #region property

        /// <summary>
        /// メインカテゴリ。
        /// </summary>
        public string Primary { get; }
        /// <summary>
        /// サブカテゴリ。
        /// </summary>
        public IReadOnlyList<string> Secondaries { get; }

        #endregion
    }
}
