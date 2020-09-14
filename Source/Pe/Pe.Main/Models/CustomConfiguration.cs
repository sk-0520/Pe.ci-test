using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Command;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public abstract class ConfigurationBase
    {
        protected ConfigurationBase(IConfigurationSection section)
        { }

        #region function

        protected static IReadOnlyList<T> GetList<T>(IConfigurationSection section, string key)
        {
            return section.GetSection(key).Get<T[]>();
        }

        protected static Size GetSize(IConfigurationSection section, string key)
        {
            var size = section.GetSection(key);
            return new Size(size.GetValue<double>("width"), size.GetValue<double>("height"));
        }

        protected static MinMax<T> GetMinMax<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMax<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"));
        }
        protected static MinMaxDefault<T> GetMinMaxDefault<T>(IConfigurationSection section, string key)
            where T : IComparable<T>
        {
            var size = section.GetSection(key);
            return new MinMaxDefault<T>(size.GetValue<T>("minimum"), size.GetValue<T>("maximum"), size.GetValue<T>("default"));
        }

        #endregion
    }

    public class GeneralConfiguration: ConfigurationBase
    {
        public GeneralConfiguration(IConfigurationSection section) : base(section)
        {
            ProjectName = section.GetValue<string>("project_name");
            MutexName = section.GetValue<string>("mutex_name");
            LoggingConfigFileName = section.GetValue<string>("log_conf_file_name");
            SupportCultures = section.GetSection("support_cultures").Get<string[]>();

            LicenseName = section.GetValue<string>("license_name");
            LicenseUri = section.GetValue<Uri>("license_uri");

            ProjectRepositoryUri = section.GetValue<Uri>("project_repository_uri");
            ProjectForumUri = section.GetValue<Uri>("project_forum_uri");
            ProjectWebSiteUri = section.GetValue<Uri>("project_website_uri");
            UpdateCheckUri = section.GetValue<Uri>("version_check_uri");
            UpdateWait = section.GetValue<TimeSpan>("update_wait");
            DispatcherWait = section.GetValue<TimeSpan>("dispatcher_wait");

            CanSendCrashReport = section.GetValue<bool>("can_send_crash_report");
            UnhandledExceptionHandled = section.GetValue<bool>("unhandled_exception_handled");
        }

        #region property

        public string ProjectName { get; }
        public string MutexName { get; }
        public string LoggingConfigFileName { get; }

        public IReadOnlyList<string> SupportCultures { get; }

        public string LicenseName { get; }
        public Uri LicenseUri { get; }

        public Uri ProjectRepositoryUri { get; }
        public Uri ProjectForumUri { get; }
        public Uri ProjectWebSiteUri { get; }
        public Uri UpdateCheckUri { get; }
        public TimeSpan UpdateWait { get; }
        public TimeSpan DispatcherWait { get; }

        public bool CanSendCrashReport { get; }
        public bool UnhandledExceptionHandled { get; }

        #endregion
    }

    public class ApiConfiguration: ConfigurationBase
    {
        public ApiConfiguration(IConfigurationSection section)
            : base(section)
        {
            CrashReportUri = section.GetValue<Uri>("crash_report_uri");
            CrashReportSourceUri = section.GetValue<Uri>("crash_report_src_uri");

            FeedbackUri = section.GetValue<Uri>("feedback_uri");
            FeedbackSourceUri = section.GetValue<Uri>("feedback_src_uri");
        }

        #region property

        public Uri CrashReportUri { get; }
        public Uri CrashReportSourceUri { get; }

        public Uri FeedbackUri { get; }
        public Uri FeedbackSourceUri { get; }

        #endregion
    }

    public class WebConfiguration: ConfigurationBase
    {
        public WebConfiguration(IConfigurationSection section)
            : base(section)
        {
            var versionConverter = new VersionConverter();

            var map = new Dictionary<string, string>() {
                ["APP-NAME"] = BuildStatus.Name,
                ["APP-BUILD"] = BuildStatus.BuildType.ToString(),
                ["APP-VER"] = versionConverter.ConvertNormalVersion(BuildStatus.Version),
                ["APP-REVISION"] = BuildStatus.Revision,
            };
            ViewUserAgent = TextUtility.ReplaceFromDictionary(section.GetValue<string>("view_useragent"), map);
            ClientUserAgent = TextUtility.ReplaceFromDictionary(section.GetValue<string>("client_useragent"), map);
            DeveloperTools = section.GetValue<bool>("developer_tools");
        }

        #region property

        public string ViewUserAgent { get; }
        public string ClientUserAgent { get; }

        /// <summary>
        /// ウィンドウ生成(インスタンス化)時点でWEBブラウザっぽいのがあればそれに対して開発者ツールを呼び出せる拡張処理を行うか。
        /// <para>複数あったり動的に生成する場合は個別対応が必要。</para>
        /// </summary>
        public bool DeveloperTools { get; }

        #endregion
    }

    public class BackupConfiguration: ConfigurationBase
    {
        public BackupConfiguration(IConfigurationSection section)
            : base(section)
        {
            SettingCount = section.GetValue<int>("setting_count");
            ArchiveCount = section.GetValue<int>("archive_count");
        }

        #region property

        public int SettingCount { get; }
        public int ArchiveCount { get; }

        #endregion
    }

    public class FileConfiguration: ConfigurationBase
    {
        public FileConfiguration(IConfigurationSection section)
            : base(section)
        {
            DirectoryRemoveWaitCount = section.GetValue<int>("dir_remove_wait_count");
            DirectoryRemoveWaitTime = section.GetValue<TimeSpan>("dir_remove_wait_time");
            GivePriorityToFile = section.GetValue<bool>("give_priority_to_file");
        }

        #region property

        public int DirectoryRemoveWaitCount { get; }
        public TimeSpan DirectoryRemoveWaitTime { get; }

        public bool GivePriorityToFile { get; }

        #endregion
    }

    public class DisplayConfiguration: ConfigurationBase
    {
        public DisplayConfiguration(IConfigurationSection section)
            : base(section)
        {
            ChangedRetryCount = section.GetValue<int>("changed_retry_count");
            ChangedRetryWaitTime = section.GetValue<TimeSpan>("changed_retry_wait");
        }

        #region property

        public int ChangedRetryCount { get; }
        public TimeSpan ChangedRetryWaitTime { get; }

        #endregion
    }

    public class HookConfiguration: ConfigurationBase
    {
        public HookConfiguration(IConfigurationSection section)
            : base(section)
        {
            Keyboard = section.GetValue<bool>("keyboard");
            Mouse = section.GetValue<bool>("mouse");
        }

        #region property

        /// <summary>
        /// キーボードを有効にするか。
        /// </summary>
        public bool Keyboard { get; }
        /// <summary>
        /// マウスフックを有効にするか。
        /// </summary>
        public bool Mouse { get; }

        #endregion
    }

    public class NotifyLogConfiguration: ConfigurationBase
    {
        public NotifyLogConfiguration(IConfigurationSection section)
            : base(section)
        {
            NormalLogDisplayTime = section.GetValue<TimeSpan>("normal_log_display_time");
            UndoLogDisplayTime = section.GetValue<TimeSpan>("undo_log_display_time");
            CommandLogDisplayTime = section.GetValue<TimeSpan>("command_log_display_time");
            FadeoutTime = section.GetValue<TimeSpan>("fadeout_time");
        }

        #region property

        public TimeSpan NormalLogDisplayTime { get; }
        public TimeSpan UndoLogDisplayTime { get; }
        public TimeSpan CommandLogDisplayTime { get; }

        public TimeSpan FadeoutTime { get; }

        #endregion
    }

    public class LauncherItemConfiguration: ConfigurationBase
    {
        public LauncherItemConfiguration(IConfigurationSection section)
            : base(section)
        {
            IconRefreshTime = section.GetValue<TimeSpan>("icon_refresh_time");
            AutoImportUntargetPatterns = GetList<string>(section, "auto_import_untarget_patterns");
        }

        #region property

        public TimeSpan IconRefreshTime { get; }
        /// <summary>
        /// 自動登録対象外ファイルパターン。
        /// <para>正規表現・大文字小文字を区別しない。</para>
        /// </summary>
        public IReadOnlyList<string> AutoImportUntargetPatterns { get; }

        #endregion
    }

    public class LauncherToolbarConfiguration: ConfigurationBase
    {
        public LauncherToolbarConfiguration(IConfigurationSection section)
            : base(section)
        {
            AutoHideShowWaitTime = section.GetValue<TimeSpan>("auto_hide_show_wait_time");
        }

        #region property

        public TimeSpan AutoHideShowWaitTime { get; }

        #endregion
    }

    public class LauncherGroupConfiguration: ConfigurationBase
    {
        public LauncherGroupConfiguration(IConfigurationSection section)
            : base(section)
        {
        }

        #region property

        #endregion
    }

    public class NoteConfiguration: ConfigurationBase
    {
        public NoteConfiguration(IConfigurationSection section)
            : base(section)
        {
            LayoutAbsoluteSize = GetSize(section, "layout_absolute_size");
            LayoutRelativeSize = GetSize(section, "layout_relative_size");
            FontSize = GetMinMaxDefault<double>(section, "font_size");


            HiddenCompactWaitTime = section.GetValue<TimeSpan>("hidden_compact_wait_time");
            HiddenBlindWaitTime = section.GetValue<TimeSpan>("hidden_blind_wait_time");
        }

        #region property

        public Size LayoutAbsoluteSize { get; }
        public Size LayoutRelativeSize { get; }
        public MinMaxDefault<double> FontSize { get; }

        public TimeSpan HiddenCompactWaitTime { get; }
        public TimeSpan HiddenBlindWaitTime { get; }

        #endregion
    }

    public class CommandConfiguration: ConfigurationBase
    {
        public CommandConfiguration(IConfigurationSection section)
            : base(section)
        {
            IconClearWaitTime = section.GetValue<TimeSpan>("icon_clear_wait_time");
            ViewCloseWaitTime = section.GetValue<TimeSpan>("view_close_wait_time");

            var applicationCommand = section.GetSection("application_command");
            ApplicationPrefix = applicationCommand.GetValue<string>("prefix");
            ApplicationSeparator = applicationCommand.GetValue<string>("separator");
            ApplicationMapping = applicationCommand.GetSection("mapping").Get<Dictionary<string, string>>()
                .ToDictionary(k => EnumUtility.Parse<ApplicationCommand>(k.Key), v => v.Value)
            ;
        }

        #region property

        public TimeSpan IconClearWaitTime { get; }
        public TimeSpan ViewCloseWaitTime { get; }

        public string ApplicationPrefix { get; }
        public string ApplicationSeparator { get; }
        internal IReadOnlyDictionary<ApplicationCommand, string> ApplicationMapping { get; }

        #endregion
    }

    public class PlatformConfiguration: ConfigurationBase
    {
        #region define

        public class PlatformFullscreenConfiguration: ConfigurationBase
        {
            #region define

            public class PlatformFullscreenClassAndTextConfiguration: ConfigurationBase
            {
                public PlatformFullscreenClassAndTextConfiguration(IConfigurationSection section)
                    : base(section)
                {
                    WindowClassName = section.GetValue<string>("class");
                    WindowText = section.GetValue<string>("text");
                }

                #region property

                public string WindowClassName { get; }
                public string WindowText { get; }

                #endregion
            }

            #endregion

            public PlatformFullscreenConfiguration(IConfigurationSection section)
                : base(section)
            {
                IgnoreWindowClasses = section.GetSection("ignore_window_class").Get<string[]>();

                var classTextSection = section.GetSection("ignore_window_class_text");
                IgnoreClassAndTexts = classTextSection.GetChildren().Select(i => new PlatformFullscreenClassAndTextConfiguration(i)).ToArray();

                TopmostOnly = section.GetValue<bool>("topmost_only");
                ExcludeNoActive = section.GetValue<bool>("exclude_noactive");
                ExcludeToolWindow = section.GetValue<bool>("exclude_toolwindow");
            }

            #region proeprty

            public IReadOnlyList<string> IgnoreWindowClasses { get; }
            public IReadOnlyList<PlatformFullscreenClassAndTextConfiguration> IgnoreClassAndTexts { get; }

            public bool TopmostOnly { get; }
            public bool ExcludeNoActive { get; }
            public bool ExcludeToolWindow { get; }

            #endregion
        }

        #endregion

        public PlatformConfiguration(IConfigurationSection section)
            : base(section)
        {
            ThemeAccentColorMinimumAlpha = section.GetValue<byte>("theme_accent_color_minimum_alpha");
            ThemeAccentColorDefaultAlpha = section.GetValue<byte>("theme_accent_color_default_alpha");

            ExplorerSupporterRefreshTime = section.GetValue<TimeSpan>("explorer_supporter_refresh_time");
            ExplorerSupporterCacheSize = section.GetValue<int>("explorer_supporter_cache_size");

            ScreenElementsResetWaitTime = section.GetValue<TimeSpan>("screen_elements_reset_wait_time");

            Fullscreen = new PlatformFullscreenConfiguration(section.GetSection("fullscreen"));
        }

        #region property

        /// <summary>
        /// アクセントカラーの透明度を無効と判断する最低A値。
        /// <para>この値未満であれば無効。</para>
        /// </summary>
        public byte ThemeAccentColorMinimumAlpha { get; }
        /// <summary>
        /// アクセントカラーの透明度が<see cref="ThemeAccentColorMinimumAlpha"/>で無効判定なら使用するA値。
        /// </summary>
        public byte ThemeAccentColorDefaultAlpha { get; }
        public TimeSpan ExplorerSupporterRefreshTime { get; }
        public int ExplorerSupporterCacheSize { get; }

        public TimeSpan ScreenElementsResetWaitTime { get; }

        public PlatformFullscreenConfiguration Fullscreen { get; }

        #endregion
    }

    public class ScheduleConfiguration: ConfigurationBase
    {
        public ScheduleConfiguration(IConfigurationSection section)
            : base(section)
        {

            LauncherItemIconRefresh = section.GetValue<string>("launcher_item_icon_refresh");
        }

        #region function

        public string LauncherItemIconRefresh { get; }

        #endregion
    }

    public class PluginConfiguration: ConfigurationBase
    {
        public PluginConfiguration(IConfigurationSection section)
            : base(section)
        {
            Extentions = GetList<string>(section, "extentions");
        }

        #region property

        /// <summary>
        /// プラグインとなり得る拡張子。
        /// <para>先に一致したものを優先する。</para>
        /// </summary>
        public IReadOnlyList<string> Extentions { get; }


        #endregion
    }


    public class CustomConfiguration
    {
        public CustomConfiguration(IConfigurationRoot configurationRoot)
        {
            General = new GeneralConfiguration(configurationRoot.GetSection("general"));
            Web = new WebConfiguration(configurationRoot.GetSection("web"));
            Api = new ApiConfiguration(configurationRoot.GetSection("api"));
            Backup = new BackupConfiguration(configurationRoot.GetSection("backup"));
            File = new FileConfiguration(configurationRoot.GetSection("file"));
            Display = new DisplayConfiguration(configurationRoot.GetSection("display"));
            Hook = new HookConfiguration(configurationRoot.GetSection("hook"));
            NotifyLog = new NotifyLogConfiguration(configurationRoot.GetSection("notify_log"));
            LauncherItem = new LauncherItemConfiguration(configurationRoot.GetSection("launcher_item"));
            LauncherToobar = new LauncherToolbarConfiguration(configurationRoot.GetSection("launcher_toolbar"));
            LauncherGroup = new LauncherGroupConfiguration(configurationRoot.GetSection("launcher_group"));
            Note = new NoteConfiguration(configurationRoot.GetSection("note"));
            Command = new CommandConfiguration(configurationRoot.GetSection("command"));
            Platform = new PlatformConfiguration(configurationRoot.GetSection("platform"));
            Schedule = new ScheduleConfiguration(configurationRoot.GetSection("schedule"));
            Plugin = new PluginConfiguration(configurationRoot.GetSection("plugin"));
        }

        #region property

        public GeneralConfiguration General { get; }
        public WebConfiguration Web { get; }
        public ApiConfiguration Api { get; }
        public BackupConfiguration Backup { get; }
        public FileConfiguration File { get; }
        public DisplayConfiguration Display { get; }
        public HookConfiguration Hook { get; }
        public NotifyLogConfiguration NotifyLog { get; }
        public LauncherItemConfiguration LauncherItem { get; }
        public LauncherToolbarConfiguration LauncherToobar { get; }
        public LauncherGroupConfiguration LauncherGroup { get; }
        public NoteConfiguration Note { get; }
        public CommandConfiguration Command { get; }
        public PlatformConfiguration Platform { get; }
        public ScheduleConfiguration Schedule { get; }
        public PluginConfiguration Plugin { get; }
        #endregion
    }
}
