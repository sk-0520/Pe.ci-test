using System;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.Embedded.Attributes
{
    /// <summary>
    /// [アセンブリ] プラグイン作者設定。
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class PluginAuthorsAttribute: Attribute
    {
        /// <summary>
        /// [アセンブリ] プラグイン作者設定。
        /// </summary>
        /// <param name="name">作者名。</param>
        /// <param name="license">ライセンス。<seealso cref="PluginLicense"/></param>
        /// <param name="website">プラグインの作者サイト。</param>
        /// <param name="projectSite">プラグインのプロジェクトサイト。</param>
        /// <param name="email">プラグインの作者E-Mail。</param>
        public PluginAuthorsAttribute(string name, string license, string website = "", string projectSite = "", string email = "")
        {
            if(string.IsNullOrWhiteSpace(name)) {
                throw new ArgumentException(nameof(name));
            }
            Name = name;

            if(string.IsNullOrWhiteSpace(license)) {
                throw new ArgumentException(nameof(license));
            }
            License = license;

            Website = website;
            ProjectSite = projectSite;
            Email = email;
        }

        #region property

        /// <summary>
        /// 作者名。
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// ライセンス。
        /// </summary>
        public string License { get; }

        /// <summary>
        /// 作者Webサイト。
        /// </summary>
        public string Website { get; }
        /// <summary>
        /// プラグインのプロジェクトサイト。
        /// </summary>
        public string ProjectSite { get; }
        /// <summary>
        /// プラグインの作者E-Mail。
        /// </summary>
        public string Email { get; }

        #endregion
    }
}
