using System;
using System.Collections.Generic;
using ContentTypeTextNet.Pe.Main.Models.Command;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: コマンド。
    /// </summary>
    public class CommandConfiguration: ConfigurationBase
    {
        #region define

        public class ApplicationCommandConfiguration: ConfigurationBase
        {
            public ApplicationCommandConfiguration(IConfiguration conf) : base(conf)
            {
            }

            #region property

            [Configuration]
            public string Prefix { get; } = default!;
            [Configuration]
            public string Separator { get; } = default!;
            [Configuration]
            public IReadOnlyDictionary<ApplicationCommand, string> Mapping { get; } = default!;

            #endregion
        }

        #endregion

        public CommandConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// アイコンクリアまでの時間。
        /// </summary>
        [Configuration]
        public TimeSpan IconClearWaitTime { get; }
        /// <summary>
        /// View クリアまでの時間。
        /// </summary>
        [Configuration]
        public TimeSpan ViewCloseWaitTime { get; }


        [Configuration("application_command")]
        public ApplicationCommandConfiguration Application { get; } = default!;

        #endregion
    }
}
