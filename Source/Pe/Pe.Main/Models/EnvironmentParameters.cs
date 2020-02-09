using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Configuration;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public class EnvironmentParameters
    {
        public EnvironmentParameters(DirectoryInfo rootDirectory, CommandLine commandLine)
        {
            if(!commandLine.IsParsed) {
                throw new ArgumentException(nameof(commandLine));
            }
            //CommandLine = commandLine;

            RootDirectory = rootDirectory;

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
            ;
            var configurationRoot = configurationBuilder.Build();
            Configuration = new CustomConfiguration(configurationRoot);

            var projectName = Configuration.General.ProjectName;
            UserRoamingDirectory = GetDirectory(commandLine, CommandLineKeyUserDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), projectName));
            MachineDirectory = GetDirectory(commandLine, CommandLineKeyMachineDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), projectName));
            TemporaryDirectory = GetDirectory(commandLine, CommandLineKeyTemporaryDirectory, Path.Combine(Path.GetTempPath(), projectName));

            OldSettingRootDirectoryPath = commandLine.GetValue(CommandLineKeyOldSettingRootDirectoryPath, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        #region property

        public static string CommandLineKeyUserDirectory { get; } = "user-dir";
        public static string CommandLineKeyMachineDirectory { get; } = "machine-dir";
        public static string CommandLineKeyTemporaryDirectory { get; } = "temp-dir";
        public static string CommandLineKeyOldSettingRootDirectoryPath { get; } = "old-setting-root";
        //CommandLine CommandLine { get; }
        public string OldSettingRootDirectoryPath { get; }
        /// <summary>
        /// アプリケーションの最上位ディレクトリ。
        /// </summary>
        public DirectoryInfo RootDirectory { get; }

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
        public DirectoryInfo EtcDirectory => CombineDirectory(RootDirectory, "etc");
        public FileInfo EtcUpdateScriptFile => CombineFile(EtcDirectory, "update-application.ps1");
        /// <summary>
        /// SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo SqlDirectory => CombineDirectory(EtcDirectory, "sql");
        /// <summary>
        /// Pe の SQL ディレクトリ。
        /// </summary>
        public DirectoryInfo MainSqlDirectory => CombineDirectory(SqlDirectory, "ContentTypeTextNet.Pe.Main");
        /// <summary>
        /// 文書ディレクトリ。
        /// </summary>
        public DirectoryInfo DocumentDirectory => CombineDirectory(RootDirectory, "doc");
        public DirectoryInfo LicenseDirectory => CombineDirectory(DocumentDirectory, "license");

        public FileInfo ComponentsFile => CombineFile(LicenseDirectory, "components.json");

        /// <summary>
        /// ユーザーデータ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo UserRoamingDirectory { get; }
        /// <summary>
        /// バックアップディレクトリ。
        /// </summary>
        public DirectoryInfo UserBackupDirectory => CombineDirectory(UserRoamingDirectory, "backups");
        /// <summary>
        /// 設定ディレクトリ。
        /// </summary>
        public DirectoryInfo UserSettingDirectory => CombineDirectory(UserRoamingDirectory, "settings");
        public DirectoryInfo UserPluginDirectory => CombineDirectory(UserSettingDirectory, "plugins");

        /// <summary>
        /// ユーザー端末配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineDirectory { get; set; }
        /// <summary>
        /// アーカイブディレクトリ。
        /// </summary>
        public DirectoryInfo MachineArchiveDirectory => CombineDirectory(MachineDirectory, "archive");
        /// <summary>
        /// アプリケーションアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineUpdateArchiveDirectory => CombineDirectory(MachineArchiveDirectory, "application");
        public DirectoryInfo MachinePluginDirectory => CombineDirectory(MachineDirectory, "plugins");

        /// <summary>
        /// 一時ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryDirectory { get; set; }

        /// <summary>
        /// 設定ファイル格納用一時ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporarySettingDirectory => CombineDirectory(TemporaryDirectory, "setting");
        public DirectoryInfo TemporaryPluginDirectory => CombineDirectory(TemporaryDirectory, "plugins");

        /// <summary>
        /// アーカイブ展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryExtractDirectory => CombineDirectory(TemporaryDirectory, "extract");
        /// <summary>
        /// アプリケーション展開ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryApplicationExtractDirectory => CombineDirectory(TemporaryExtractDirectory, "application");

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
            return new DirectoryInfo(path);
        }

        private FileInfo CombineFile(DirectoryInfo directory, params string[] directoryAndFileNames)
        {
            var path = CombinePath(directory.FullName, directoryAndFileNames);
            return new FileInfo(path);
        }

        #endregion


    }


}
