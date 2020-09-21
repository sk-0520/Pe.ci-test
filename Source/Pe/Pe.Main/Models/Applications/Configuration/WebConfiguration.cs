using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    public class WebConfiguration: ConfigurationBase
    {
        public WebConfiguration(IConfigurationSection section)
            : base(section)
        {
            //var versionConverter = new VersionConverter();

            //var map = new Dictionary<string, string>() {
            //    ["APP-NAME"] = BuildStatus.Name,
            //    ["APP-BUILD"] = BuildStatus.BuildType.ToString(),
            //    ["APP-VER"] = versionConverter.ConvertNormalVersion(BuildStatus.Version),
            //    ["APP-REVISION"] = BuildStatus.Revision,
            //};
            //ViewUserAgentFormat = TextUtility.ReplaceFromDictionary(section.GetValue<string>("view_useragent_format"), map);
            //ClientUserAgentFormat = TextUtility.ReplaceFromDictionary(section.GetValue<string>("client_useragent_format"), map);
            //DeveloperTools = section.GetValue<bool>("developer_tools");
        }

        #region property

        [Configuration("view_useragent_format")]
        public string ViewUserAgentFormat { get; } = default!;
        [Configuration("client_useragent_format")]
        public string ClientUserAgentFormat { get; } = default!;

        /// <summary>
        /// ウィンドウ生成(インスタンス化)時点でWEBブラウザっぽいのがあればそれに対して開発者ツールを呼び出せる拡張処理を行うか。
        /// <para>複数あったり動的に生成する場合は個別対応が必要。</para>
        /// </summary>
        [Configuration]
        public bool DeveloperTools { get; }

        #endregion
    }
}
