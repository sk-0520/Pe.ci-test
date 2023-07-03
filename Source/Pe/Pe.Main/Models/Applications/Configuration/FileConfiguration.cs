using System;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: ファイル。
    /// </summary>
    public class FileConfiguration: ConfigurationBase
    {
        public FileConfiguration(IConfigurationSection section)
            : base(section)
        { }

        #region property

        /// <summary>
        /// ディレクトリ作成失敗時の試行回数。
        /// </summary>
        [Configuration]
        public int DirectoryRemoveWaitCount { get; }
        /// <summary>
        /// ディレクトリ作成失敗時の試行前待機時間。
        /// </summary>
        [Configuration]
        public TimeSpan DirectoryRemoveWaitTime { get; }

        /// <summary>
        /// SQL実行時にファイルからの読み込みを優先するか。
        /// </summary>
        [Configuration]
        public bool GivePriorityToFile { get; }

        #endregion
    }
}
