using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: 定期実行系。
    /// </summary>
    public class ScheduleConfiguration: ConfigurationBase
    {
        public ScheduleConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region function

        /// <summary>
        /// 低レベルスケジューラ時間。
        /// </summary>
        [Configuration]
        public TimeSpan LowSchedulerTime { get; }
        /// <summary>
        /// アイコン更新タイミング。
        /// </summary>
        [Configuration]
        public string LauncherItemIconRefresh { get; } = default!;

        #endregion
    }
}
