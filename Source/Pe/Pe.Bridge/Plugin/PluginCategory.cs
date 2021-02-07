using System;
using System.Collections.Generic;
using System.Linq;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// 認識可能カテゴリ。
    /// </summary>
    public static class PluginCategories
    {
        #region property

        public const string Design = "design";
        public const string FileSystem = "file-system";
        public const string Utility = "utility";
        public const string Network = "network";
        public const string Toy = "toy";

        public const string Others = "";

        #endregion
    }

    public interface IPluginCategory
    {
        #region property

        /// <summary>
        /// 主カテゴリ。
        /// </summary>
        string PluginPrimaryCategory { get; }

        /// <summary>
        /// <see cref="PluginPrimaryCategory"/>以外の該当しそうなカテゴリ一覧。
        /// </summary>
        IReadOnlyList<string> PluginSecondaryCategories { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginCategory"/>
    public class PluginCategory: IPluginCategory
    {
        public PluginCategory(string primary)
        {
            PluginPrimaryCategory = primary ?? throw new ArgumentNullException(primary);
            PluginSecondaryCategories = new List<string>();
        }

        public PluginCategory(string primary, IEnumerable<string> secondaries)
        {
            PluginPrimaryCategory = primary ?? throw new ArgumentNullException(primary);
            PluginSecondaryCategories = secondaries?.ToList() ?? throw new ArgumentNullException(primary);
        }


        #region IPluginCategory

        /// <inheritdoc cref="IPluginCategory.PluginPrimaryCategory"/>
        public string PluginPrimaryCategory { get; }

        /// <inheritdoc cref="IPluginCategory.PluginSecondaryCategories"/>
        public List<string> PluginSecondaryCategories { get; }
        IReadOnlyList<string> IPluginCategory.PluginSecondaryCategories => PluginSecondaryCategories;

        #endregion
    }
}
