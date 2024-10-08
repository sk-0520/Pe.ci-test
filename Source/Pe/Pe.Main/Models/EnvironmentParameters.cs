using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    [System.AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    sealed internal class InitialDirectoryAttribute: Attribute
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
                throw new ArgumentException(null, nameof(commandLine));
            }
            //CommandLine = commandLine;

            RootDirectory = rootDirectory;

#if !PRODUCT
            ApplicationBaseDirectory = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
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
            ApplicationConfiguration = new ApplicationConfiguration(configurationRoot);

            var projectName = ApplicationConfiguration.General.ProjectName;
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
        internal DirectoryInfo ApplicationBaseDirectory { get; }
#endif

        public static string RootApplicationName { get; } = "Pe.exe";
        public FileInfo RootApplication => CombineFile(RootDirectory, RootApplicationName);

        /// <summary>
        /// アプリケーションのディレクトリ。
        /// </summary>
        public DirectoryInfo AssemblyDirectory { get; } = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!);
        /// <summary>
        /// 通常のプログラムディレクトリ。
        /// </summary>
        public DirectoryInfo ApplicationDirectory =>
#if PRODUCT
            CombineDirectory(false, RootDirectory, "bin");
#else
            CombineDirectory(false, ApplicationBaseDirectory, "bin");
#endif
        /// <summary>
        /// etc ディレクトリ。
        /// </summary>
        public DirectoryInfo EtcDirectory =>
#if PRODUCT
            CombineDirectory(false, RootDirectory, "etc");
#else
            CombineDirectory(false, ApplicationBaseDirectory, "etc");
#endif
        public DirectoryInfo EtcScriptDirectory => CombineDirectory(false, EtcDirectory, "script");
        public DirectoryInfo EtcUpdateDirectory => CombineDirectory(false, EtcScriptDirectory, "update");

        /// <summary>
        /// アップデート時のローカル処理。
        /// </summary>
        public FileInfo EtcUpdateScriptFile => CombineFile(EtcUpdateDirectory, "update-application.ps1");
        /// <summary>
        /// 再起動スクリプトファイル。
        /// </summary>
        public FileInfo EtcRebootScriptFile => CombineFile(EtcScriptDirectory, "reboot.ps1");
        /// <summary>
        /// SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo SqlDirectory => CombineDirectory(false, EtcDirectory, "sql");
        /// <summary>
        /// 結合済みSQL。
        /// </summary>
        public FileInfo SqlStatementAccessorFile => CombineFile(SqlDirectory, "sql.sqlite3");
        /// <summary>
        /// Pe の SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo MainSqlDirectory => CombineDirectory(false, SqlDirectory, "ContentTypeTextNet.Pe.Main");

        /// <summary>
        /// 文書ディレクトリ。
        /// </summary>
        public DirectoryInfo DocumentDirectory =>
#if PRODUCT
            CombineDirectory(false, RootDirectory, "doc");
#else
            CombineDirectory(false, ApplicationBaseDirectory, "doc");
#endif

        public DirectoryInfo LicenseDirectory => CombineDirectory(false, DocumentDirectory, "license");

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
        public DirectoryInfo UserBackupDirectory => CombineDirectory(true, UserRoamingDirectory, "backups");
        /// <summary>
        /// 設定ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo UserSettingDirectory => CombineDirectory(true, UserRoamingDirectory, "settings");
        /// <summary>
        /// プラグインディレクトリ。
        /// </summary>
        private DirectoryInfo UserPluginDirectory => CombineDirectory(true, UserSettingDirectory, "plugin");
        /// <summary>
        /// プラグイン設定ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </remarks>
        public DirectoryInfo UserPluginDataDirectory => CombineDirectory(true, UserPluginDirectory, "data");

        /// <summary>
        /// ユーザー端末配置ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineDirectory { get; set; }
        /// <summary>
        /// アーカイブディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineArchiveDirectory => CombineDirectory(true, MachineDirectory, "archive");
        /// <summary>
        /// アプリケーションアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo MachineUpdateArchiveDirectory => CombineDirectory(true, MachineArchiveDirectory, "application");
        /// <summary>
        /// プラグインアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineUpdatePluginDirectory => CombineDirectory(true, MachineArchiveDirectory, "plugins");
        /// <summary>
        /// ユーザー端末プラグイン用ディレクトリ。
        /// </summary>
        private DirectoryInfo MachinePluginDirectory => CombineDirectory(true, MachineDirectory, "plugin");
        /// <summary>
        /// ユーザー端末プラグイン設定ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </remarks>
        public DirectoryInfo MachinePluginDataDirectory => CombineDirectory(true, MachinePluginDirectory, "data");
        /// <summary>
        /// プラグインモジュール配置ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>この下にプラグインごとのディレクトリを配置してバイナリを置く。</para>
        /// </remarks>
        public DirectoryInfo MachinePluginModuleDirectory => CombineDirectory(true, MachinePluginDirectory, "modules");
        /// <summary>
        /// インストール対象プラグインの配置ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>起動時にこの下にあるプラグインを<see cref="MachinePluginModuleDirectory"/>に転送する。</para>
        /// </remarks>
        public DirectoryInfo MachinePluginInstallDirectory => CombineDirectory(true, MachinePluginDirectory, "install");

        /// <summary>
        /// WebViewの端末親ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineWebViewDirectory => CombineDirectory(true, MachineDirectory, "web-view");
        /// <summary>
        /// 送信済みクラッシュレポート配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineCrashReportDirectory => CombineDirectory(true, MachineDirectory, "crash");

        /// <summary>
        /// 一時ディレクトリ。
        /// </summary>
        [InitialDirectory]
        public DirectoryInfo TemporaryDirectory { get; set; }

        /// <summary>
        /// 設定ファイル格納用一時ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporarySettingDirectory => CombineDirectory(true, TemporaryDirectory, "setting");
        /// <summary>
        /// 一時プラグインディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryPluginDirectory => CombineDirectory(true, TemporaryDirectory, "plugin");
        /// <summary>
        /// 一時プラグイン設定ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>この下にプラグインごとのディレクトリを配置してデータを置く。</para>
        /// </remarks>
        public DirectoryInfo TemporaryPluginDataDirectory => CombineDirectory(true, TemporaryPluginDirectory, "data");

        /// <summary>
        /// アーカイブ展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryExtractDirectory => CombineDirectory(true, TemporaryDirectory, "extract");
        /// <summary>
        /// アプリケーション展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryApplicationExtractDirectory => CombineDirectory(true, TemporaryExtractDirectory, "application");
        /// <summary>
        /// プラグイン展開親ディレクトリ。
        /// </summary>
        /// <remarks>
        /// <para>この下にプラグインごとのディレクトリを作成して展開する。</para>
        /// </remarks>
        public DirectoryInfo TemporaryPluginExtractBaseDirectory => CombineDirectory(true, TemporaryExtractDirectory, "plugins");
        /// <summary>
        /// プラグイン自動展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryPluginAutomaticExtractDirectory => CombineDirectory(true, TemporaryPluginExtractBaseDirectory, "automatic");
        /// <summary>
        /// プラグイン手動展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryPluginManualExtractDirectory => CombineDirectory(true, TemporaryPluginExtractBaseDirectory, "manual");
        /// <summary>
        /// アップデート時ログファイル。
        /// </summary>
        public FileInfo TemporaryUpdateLogFile => CombineFile(TemporaryDirectory, "update.log");
        /// <summary>
        /// 再起動ログファイル。
        /// </summary>
        public FileInfo TemporaryRebootLogFile => CombineFile(TemporaryDirectory, "reboot.log");
        /// <summary>
        /// プロセス間通信実行ログ
        /// </summary>
        /// <remarks>
        /// <para>TODO: ただひたすら大きくなるのでどっかのタイミング綺麗にはしたいなぁ、と思いつつ後回し。</para>
        /// </remarks>
        public FileInfo TemporaryIpcLogFile => CombineFile(TemporaryDirectory, "ipc.log");
        /// <summary>
        /// 生クラッシュレポート配置ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryCrashReportDirectory => CombineDirectory(true, TemporaryDirectory, "crash");

        /// <summary>
        /// 設定格納DBファイル。
        /// </summary>
        public FileInfo MainFile => CombineFile(UserSettingDirectory, "setting.sqlite3");
        /// <summary>
        /// ファイル格納DBファイル。
        /// </summary>
        public FileInfo LargeFile => CombineFile(UserSettingDirectory, "file.sqlite3");

        public ApplicationConfiguration ApplicationConfiguration { get; }

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

        private DirectoryInfo CombineDirectory(bool isUserDirectory, DirectoryInfo directory, params string[] directoryNames)
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

    internal class ApplicationEnvironmentParameters: EnvironmentParameters
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
