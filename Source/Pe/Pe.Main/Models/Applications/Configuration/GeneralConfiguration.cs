using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models.Applications.Configuration
{
    /// <summary>
    /// アプリケーション構成: 基本。
    /// </summary>
    public class GeneralConfiguration: ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        { }

        #region property

        /// <summary>
        /// プロジェクト名。
        /// </summary>
        [Configuration]
        public string ProjectName { get; } = default!;

        /// <summary>
        /// 二重起動判定 Mutex 名。
        /// </summary>
        [Configuration]
        public string MutexName { get; } = default!;

        /// <summary>
        /// ログ設定ファイル名。
        /// </summary>
        [Configuration("log_conf_file_name")]
        public string LogConfigFileName { get; } = default!;

        /// <summary>
        /// 対応言語一覧。
        /// </summary>
        [Configuration]
        public IReadOnlyList<string> SupportCultures { get; } = default!;

        /// <summary>
        /// ライセンス名。
        /// </summary>
        [Configuration]
        public string LicenseName { get; } = default!;
        /// <summary>
        /// ライセンスURI。
        /// </summary>
        [Configuration]
        public Uri LicenseUri { get; } = default!;

        /// <summary>
        /// プロジェクト リポジトリ URI。
        /// </summary>
        [Configuration]
        public Uri ProjectRepositoryUri { get; } = default!;
        /// <summary>
        /// プロジェクト フォーラム URI。
        /// </summary>
        [Configuration]
        public Uri ProjectForumUri { get; } = default!;
        /// <summary>
        /// プロジェクト Web サイト URI。
        /// </summary>
        [Configuration("project_website_uri")]
        public Uri ProjectWebSiteUri { get; } = default!;
        /// <summary>
        /// プロジェクト プラグイン URI。
        /// </summary>
        [Configuration]
        public Uri ProjectPluginsUri { get; } = default!;
        /// <summary>
        /// 作者 Web サイト URI。
        /// </summary>
        [Configuration("author_website_uri")]
        public Uri AuthorWebSiteUri { get; } = default!;
        /// <summary>
        /// バージョン確認 URI。
        /// </summary>
        [Configuration("version_check_uri")]
        public Uri UpdateCheckUri { get; } = default!;
        /// <summary>
        /// アップデート確認前待機時間。
        /// </summary>
        [Configuration]
        public TimeSpan UpdateWait { get; }
        /// <summary>
        /// UI 用 <see cref="System.Windows.Threading.Dispatcher"/> の待機時間。
        /// </summary>
        [Configuration]
        public TimeSpan DispatcherWait { get; }

        /// <summary>
        /// クラッシュレポートを送信するか。
        /// </summary>
        [Configuration]
        public bool CanSendCrashReport { get; }
        /// <summary>
        /// ハンドルされていない例外を受け取るか。
        /// </summary>
        [Configuration]
        public bool UnhandledExceptionHandled { get; }

        #endregion
    }
}
