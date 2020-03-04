using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    public interface IPluginVersion
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

    public static class PluginLicense
    {
        #region property

        public static string Unknown => "unknown";

        public static string DoWhatTheF_ckYouWantToPublicLicense1 => "WTFPLv1";
        public static string DoWhatTheF_ckYouWantToPublicLicense2 => "WTFPLv2";
        public static string GnuGeneralPublicLicense1 => "GPLv1";
        public static string GnuGeneralPublicLicense2 => "GPLv2";
        public static string GnuGeneralPublicLicense3 => "GPLv3";

        #endregion
    }

    public interface IPluginAuthor
    {
        IAuthor PluginAuthor { get; }
        string PluginLicense { get; }
    }

    public interface IPluginInformation
    {
        IPluginVersion PluginVersion { get; }
        IPluginAuthor PluginAuthor { get; }
    }
}
