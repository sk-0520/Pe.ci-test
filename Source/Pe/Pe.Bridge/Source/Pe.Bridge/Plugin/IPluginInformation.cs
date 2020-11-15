using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン識別。
    /// </summary>
    public interface IPluginIdentifiers
    {
        #region property

        /// <summary>
        /// プラグインの識別ID。
        /// <para>重複してるとバグる。</para>
        /// </summary>
        Guid PluginId { get; }

        /// <summary>
        /// プラグインを人が見て判断するための名前。
        /// <para>ローカライズ不要。</para>
        /// <para>手動インストールの場合に、<see cref="PluginId"/> を使用しないディレクトリ名として ASCII 範囲内でかつディレクトリ名として有効な文字のみを使用すること。ディレクトリ名として扱われる場合は重複するとバグる。</para>
        /// </summary>
        string PluginName { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginIdentifiers"/>
    public class PluginIdentifiers: IPluginIdentifiers
    {
        public PluginIdentifiers(Guid pluginId, string pluginName)
        {
            PluginId = pluginId;
            PluginName = pluginName;
        }

        #region IPluginIdentifiers

        /// <inheritdoc cref="IPluginIdentifiers.PluginId"/>
        public Guid PluginId { get; }
        /// <inheritdoc cref="IPluginIdentifiers.PluginName"/>
        public string PluginName { get; }

        #endregion

        #region object

        public override string ToString()
        {
            var sb = new StringBuilder(PluginName.Length + "()".Length + " xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx".Length);

            sb.Append(PluginName);
            sb.Append('(');
            sb.Append(PluginId.ToString("D")); // P でええやんと思う今日この頃
            sb.Append(')');

            return sb.ToString();
        }

        #endregion
    }

    /// <summary>
    /// プラグインバージョン。
    /// </summary>
    public interface IPluginVersions
    {
        #region property

        /// <summary>
        /// プラグインのバージョン情報。
        /// </summary>
        Version PluginVersion { get; }

        /// <summary>
        /// プラグインが動作可能な Pe の最低バージョン(以上)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MinimumSupportVersion { get; }
        /// <summary>
        /// プラグインが動作可能な Pe の最大バージョン(以下)。
        /// <para>0.0.0 で全バージョン稼働OK。</para>
        /// </summary>
        Version MaximumSupportVersion { get; }

        #endregion
    }

    /// <inheritdoc cref="IPluginVersions"/>
    public class PluginVersions: IPluginVersions
    {
        public PluginVersions(Version pluginVersion, Version minimumSupportVersion, Version maximumSupportVersion)
        {
            PluginVersion = pluginVersion;
            MinimumSupportVersion = minimumSupportVersion;
            MaximumSupportVersion = maximumSupportVersion;

        }

        #region IPluginVersion

        /// <inheritdoc cref="IPluginVersions.PluginVersion"/>
        public Version PluginVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MinimumSupportVersion"/>
        public Version MinimumSupportVersion { get; }
        /// <inheritdoc cref="IPluginVersions.MaximumSupportVersion"/>
        public Version MaximumSupportVersion { get; }

        #endregion
    }

    /// <summary>
    /// Pe の解釈可能なライセンス一覧。
    /// <para>const なのでバージョンアップ時に変になるかもねー。</para>
    /// </summary>
    public static class PluginLicense
    {
        #region property

        public const string Unknown = "unknown";

        public const string DoWhatTheF_ckYouWantToPublicLicense1 = "WTFPLv1";
        public const string DoWhatTheF_ckYouWantToPublicLicense2 = "WTFPLv2";
        public const string GnuGeneralPublicLicense1 = "GPLv1";
        public const string GnuGeneralPublicLicense2 = "GPLv2";
        public const string GnuGeneralPublicLicense3 = "GPLv3";

        #endregion
    }

    /// <summary>
    /// プラグイン作者情報。
    /// </summary>
    public interface IPluginAuthors
    {
        /// <summary>
        /// プラグイン作者。
        /// </summary>
        IAuthor PluginAuthor { get; }
        /// <summary>
        /// プラグイン情報。
        /// </summary>
        string PluginLicense { get; }
    }

    /// <inheritdoc cref="IPluginAuthors"/>
    public class PluginAuthors: IPluginAuthors
    {
        public PluginAuthors(IAuthor pluginAuthor, string pluginLicense)
        {
            PluginAuthor = pluginAuthor;
            PluginLicense = pluginLicense;
        }
        #region IPluginAuthors

        /// <inheritdoc cref="IPluginAuthors.PluginAuthor"/>
        public IAuthor PluginAuthor { get; }
        /// <inheritdoc cref="IPluginAuthors.PluginLicense"/>
        public string PluginLicense { get; }

        #endregion
    }


    /// <summary>
    /// プラグイン情報。
    /// </summary>
    public interface IPluginInformations
    {
        /// <summary>
        /// プラグインID。
        /// </summary>
        IPluginIdentifiers PluginIdentifiers { get; }
        /// <summary>
        /// バージョン情報。
        /// </summary>
        IPluginVersions PluginVersions { get; }
        /// <summary>
        /// 作者情報。
        /// </summary>
        IPluginAuthors PluginAuthors { get; }

        /// <summary>
        /// プラグインカテゴリ。
        /// </summary>
        IPluginCategory PluginCategory { get; }
    }

    /// <inheritdoc cref="IPluginInformations"/>
    public class PluginInformations: IPluginInformations
    {
        public PluginInformations(IPluginIdentifiers pluginIdentifiers, IPluginVersions pluginVersions, IPluginAuthors pluginAuthors, IPluginCategory pluginCategory)
        {
            PluginIdentifiers = pluginIdentifiers;
            PluginVersions = pluginVersions;
            PluginAuthors = pluginAuthors;
            PluginCategory = pluginCategory;
        }

        #region IPluginInformations

        /// <inheritdoc cref="IPluginInformations.PluginIdentifiers"/>
        public IPluginIdentifiers PluginIdentifiers { get; }

        /// <inheritdoc cref="IPluginInformations.PluginVersions"/>
        public IPluginVersions PluginVersions { get; }

        /// <inheritdoc cref="IPluginInformations.PluginAuthors"/>
        public IPluginAuthors PluginAuthors { get; }

        /// <inheritdoc cref="IPluginInformations.PluginCategory"/>
        public IPluginCategory PluginCategory { get; }
        #endregion
    }
}
