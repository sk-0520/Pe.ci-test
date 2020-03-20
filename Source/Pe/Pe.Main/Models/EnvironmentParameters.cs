using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    sealed internal class InitialDirectoryAttribute : Attribute
    {
        // This is a positional argument
        public InitialDirectoryAttribute()
        { }
    }

    public class EnvironmentParameters
    {
        public EnvironmentParameters(DirectoryInfo rootDirectory, CommandLine commandLine)
        {
            if(!commandLine.IsParsed) {
                throw new ArgumentException(nameof(commandLine));
            }
            //CommandLine = commandLine;

            RootDirectory = rootDirectory;

#if !PRODUCT
            ApplicationBaseDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
#endif
            var versionConverter = new VersionConverter();
            var versionAppSettingFileName = "appsettings." + versionConverter.ConvertDisplayVersion(BuildStatus.Version, "-") + ".json";

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .SetBasePath(EtcDirectory.FullName)
                .AddJsonFile("appsettings.json", false)
#if DEBUG
                .AddJsonFile("appsettings.debug.json", true)
#endif
#if BETA
                .AddJsonFile("appsettings.beta.json", true)
#endif
                .AddJsonFile("appsettings.user.json", true)
                .AddJsonFile(versionAppSettingFileName, true)
            ;
            var configurationRoot = configurationBuilder.Build();
            Configuration = new CustomConfiguration(configurationRoot);

            var projectName = Configuration.General.ProjectName;
            UserRoamingDirectory = GetDirectory(commandLine, CommandLineKeyUserDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), projectName));
            MachineDirectory = GetDirectory(commandLine, CommandLineKeyMachineDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), projectName));
            TemporaryDirectory = GetDirectory(commandLine, CommandLineKeyTemporaryDirectory, Path.Combine(Path.GetTempPath(), projectName));
        }

        #region property

        /// <summary>
        /// ディレクトリ取得時に作成するか。
        /// </summary>
        internal protected bool CreateDirectoryWhenGet { get; protected set; }

        public static string CommandLineKeyUserDirectory { get; } = "user-dir";
        public static string CommandLineKeyMachineDirectory { get; } = "machine-dir";
        public static string CommandLineKeyTemporaryDirectory { get; } = "temp-dir";
        //CommandLine CommandLine { get; }

        /// <summary>
        /// アプリケーションの最上位ディレクトリ。
        /// </summary>
        public DirectoryInfo RootDirectory { get; }

#if !PRODUCT
        DirectoryInfo ApplicationBaseDirectory { get; }
#endif

        public FileInfo RootApplication => CombineFile(RootDirectory, "Pe.exe");

        /// <summary>
        /// アプリケーションのディレクトリ。
        /// </summary>
        public DirectoryInfo AssemblyDirectory { get; } = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        /// <summary>
        /// 通常のプログラムディレクトリ。
        /// </summary>
        public DirectoryInfo ApplicationDirectory => CombineDirectory(RootDirectory, "bin");
        /// <summary>
        /// 特殊なプログラムディレクトリ。
        /// </summary>
        public DirectoryInfo SystemApplicationDirectory => CombineDirectory(RootDirectory, "sbin");
        /// <summary>
        /// etc ディレクトリ。
        /// </summary>
        public DirectoryInfo EtcDirectory =>
#if PRODUCT
            CombineDirectory(RootDirectory, "etc");
#else
            CombineDirectory(ApplicationBaseDirectory, "etc");
#endif
        public DirectoryInfo EtcScriptDirectory => CombineDirectory(EtcDirectory, "script");
        public DirectoryInfo EtcUpdateDirectory => CombineDirectory(EtcScriptDirectory, "update");

        /// <summary>
        /// アップデート時のローカル処理。
        /// </summary>
        public FileInfo EtcUpdateScriptFile => CombineFile(EtcUpdateDirectory, "update-application.ps1");
        /// <summary>
        /// SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo SqlDirectory => CombineDirectory(EtcDirectory, "sql");
        /// <summary>
        /// 結合済みSQL。
        /// </summary>
        public FileInfo SqlStatementAccessorFile => CombineFile(SqlDirectory, "sql.sqlite3");
        /// <summary>
        /// Pe の SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo MainSqlDirectory => CombineDirectory(SqlDirectory, "ContentTypeTextNet.Pe.Main");

        public DirectoryInfo WebViewTemplateDirectory => CombineDirectory(EtcDirectory, "web-view");

        public DirectoryInfo WebViewStyletDirectory => CombineDirectory(WebViewTemplateDirectory, "style");

        public DirectoryInfo WebViewScriptDirectory => CombineDirectory(WebViewTemplateDirectory, "script");
        public FileInfo WebViewJQueryScriptFile => CombineFile(WebViewScriptDirectory, "jquery.js");
        public DirectoryInfo WebViewStyleDirectory => CombineDirectory(WebViewTemplateDirectory, "style");
        public DirectoryInfo WebViewFeedbackTemplateDirectory => CombineDirectory(WebViewTemplateDirectory, "feedback");
        public FileInfo WebViewFeedbackTemplateFile => CombineFile(WebViewFeedbackTemplateDirectory, "feedback.html");
        public FileInfo WebViewFeedbackStyleFile => CombineFile(WebViewFeedbackTemplateDirectory, "feedback.css");
        public FileInfo WebViewFeedbackScriptFile => CombineFile(WebViewFeedbackTemplateDirectory, "feedback.js");


        /// <summary>
        /// 文書ディレクトリ。
        /// </summary>
        public DirectoryInfo DocumentDirectory =>
#if PRODUCT
            CombineDirectory(RootDirectory, "doc");
#else
            CombineDirectory(ApplicationBaseDirectory, "doc");
#endif

        public DirectoryInfo LicenseDirectory => CombineDirectory(DocumentDirectory, "license");

        public FileInfo ComponentsFile => CombineFile(LicenseDirectory, "components.json");
        public FileInfo HelpFile => CombineFile(DocumentDirectory, "help.html");

        /// <summary>
        /// ユーザーデータ配置ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo UserRoamingDirectory { get; }
        /// <summary>
        /// バックアップディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo UserBackupDirectory => CombineDirectory(UserRoamingDirectory, "backups");
        /// <summary>
        /// 設定ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo UserSettingDirectory => CombineDirectory(UserRoamingDirectory, "settings");
        /// <summary>
        /// プラグインディレクトリ。
        /// </summary>
        public DirectoryInfo UserPluginDirectory => CombineDirectory(UserSettingDirectory, "plugins");
        /// <summary>
        /// プラグイン設定ディレクトリ。
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </summary>
        public DirectoryInfo UserPluginDataDirectory => CombineDirectory(UserPluginDirectory, "data");

        /// <summary>
        /// ユーザー端末配置ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineDirectory { get; set; }
        /// <summary>
        /// アーカイブディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineArchiveDirectory => CombineDirectory(MachineDirectory, "archive");
        /// <summary>
        /// アプリケーションアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineUpdateArchiveDirectory => CombineDirectory(MachineArchiveDirectory, "application");
        /// <summary>
        /// プラグインアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineUpdatePluginDirectory => CombineDirectory(MachineArchiveDirectory, "plugins");
        /// <summary>
        /// ユーザー端末プラグインディレクトリ。
        /// </summary>
        public DirectoryInfo MachinePluginDirectory => CombineDirectory(MachineDirectory, "plugins");
        /// <summary>
        /// ユーザー端末プラグイン設定ディレクトリ。
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </summary>
        public DirectoryInfo MachinePluginDataDirectory => CombineDirectory(MachinePluginDirectory, "data");
        /// <summary>
        /// プラグインモジュール配置ディレクトリ。
        /// <para>この下にプラグインごとのディレクトリを配置してバイナリを置く。</para>
        /// </summary>
        public DirectoryInfo MachinePluginModuleDirectory => CombineDirectory(MachinePluginDirectory, "modules");

        /// <summary>
        /// WebViewの端末親ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineWebViewDirectory => CombineDirectory(MachineDirectory, "web-view");
        public DirectoryInfo MachineWebViewUserDirectory => CombineDirectory(MachineWebViewDirectory, "user");
        /// <summary>
        /// 送信済みクラッシュレポート配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineCrashReportDirectory => CombineDirectory(MachineDirectory, "crash");


        /// <summary>
        /// 一時ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo TemporaryDirectory { get; set; }

        /// <summary>
        /// 設定ファイル格納用一時ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporarySettingDirectory => CombineDirectory(TemporaryDirectory, "setting");
        /// <summary>
        /// WebViewのユーザーディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryPluginDirectory => CombineDirectory(TemporaryDirectory, "plugins");
        /// <summary>
        /// 一時プラグイン設定ディレクトリ。
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </summary>
        public DirectoryInfo TemporaryPluginDataDirectory => CombineDirectory(TemporaryPluginDirectory, "data");

        /// <summary>
        /// アーカイブ展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryExtractDirectory => CombineDirectory(TemporaryDirectory, "extract");
        /// <summary>
        /// アプリケーション展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryApplicationExtractDirectory => CombineDirectory(TemporaryExtractDirectory, "application");
        /// <summary>
        /// プラグイン展開ディレクトリ。
        /// <para>この下にプラグインごとのディレクトリを作成して展開する。</para>
        /// </summary>
        public DirectoryInfo TemporaryPluginExtractDirectory => CombineDirectory(TemporaryExtractDirectory, "plugins");
        /// <summary>
        /// WebViewの一時親ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryWebViewDirectory => CombineDirectory(TemporaryDirectory, "web-view");
        /// <summary>
        /// WebViewのキャッシュディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryWebViewCacheDirectory => CombineDirectory(TemporaryWebViewDirectory, "cache");
        /// <summary>
        /// アップデート時ログファイル。
        /// </summary>
        public FileInfo TemporaryUpdateLogFile => CombineFile(TemporaryDirectory, "update.log");
        /// <summary>
        /// 生クラッシュレポート配置ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryCrashReportDirectory => CombineDirectory(TemporaryDirectory, "crash");

        /// <summary>
        /// 設定格納DBファイル。
        /// </summary>
        public FileInfo MainFile => CombineFile(UserSettingDirectory, "setting.sqlite3");
        /// <summary>
        /// ファイル格納DBファイル。
        /// </summary>
        public FileInfo FileFile => CombineFile(UserSettingDirectory, "file.sqlite3");

        public CustomConfiguration Configuration { get; }

        #endregion

        #region function

        private static DirectoryInfo GetDirectory(CommandLine commandLine, string key, string defaultValue)
        {
            var commandLineKey = commandLine.GetKey(key);
            if(commandLineKey != null) {
                if(commandLine.Values.TryGetValue(commandLineKey, out var commandLineValue)) {
                    var rawPath = commandLineValue.First;
                    if(!string.IsNullOrWhiteSpace(rawPath)) {
                        var path = Environment.ExpandEnvironmentVariables(rawPath.Trim());
                        return new DirectoryInfo(path);
                    }
                }
            }

            return new DirectoryInfo(defaultValue);
        }

        private string CombinePath(string fullPath, string[] addPaths)
        {
            var paths = new List<string>(addPaths.Length + 1);
            paths.Add(fullPath);
            paths.AddRange(addPaths);

            var path = Path.Combine(paths.ToArray());
            return path;
        }

        private DirectoryInfo CombineDirectory(DirectoryInfo directory, params string[] directoryNames)
        {
            var path = CombinePath(directory.FullName, directoryNames);
            var dir = new DirectoryInfo(path);
            if(CreateDirectoryWhenGet) {
                if(!dir.Exists) {
                    dir.Create();
                }
            }
            return dir;
        }

        private FileInfo CombineFile(DirectoryInfo directory, params string[] directoryAndFileNames)
        {
            var path = CombinePath(directory.FullName, directoryAndFileNames);
            return new FileInfo(path);
        }

        #endregion


    }

    internal class ApplicationEnvironmentParameters : EnvironmentParameters
    {
        public ApplicationEnvironmentParameters(DirectoryInfo rootDirectory, CommandLine commandLine)
            : base(rootDirectory, commandLine)
        { }

        #region function

        public void SetFileSystemInitialized()
        {
            CreateDirectoryWhenGet = true;
        }

        #endregion
    }
}
