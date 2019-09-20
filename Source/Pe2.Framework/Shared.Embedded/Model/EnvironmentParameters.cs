using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Library.Shared.Embedded.Model
{
    public class EnvironmentParameters
    {
        #region property

        public static string CommandLineKeyUserDirectory { get; } = "user-dir";
        public static string CommandLineKeyMachineDirectory { get; } = "machine-dir";
        public static string CommandLineKeyTemporaryDirectory { get; } = "temp-dir";

        public static EnvironmentParameters Instance { get; private set; }

        /// <summary>
        /// アプリケーションの最上位ディレクトリ。
        /// </summary>
        public DirectoryInfo RootDirectory { get; private set; }

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

        /// <summary>
        /// ユーザーデータ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo UserRoamingDirectory { get; private set; }
        /// <summary>
        /// バックアップディレクトリ。
        /// </summary>
        public DirectoryInfo UserBackupDirectory => CombineDirectory(UserRoamingDirectory, "backups");
        /// <summary>
        /// 設定ディレクトリ。
        /// </summary>
        public DirectoryInfo UserSettingDirectory => CombineDirectory(UserRoamingDirectory, "settings");

        /// <summary>
        /// ユーザー端末配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineDirectory { get; private set; }
        /// <summary>
        /// アーカイブディレクトリ。
        /// </summary>
        public DirectoryInfo MachineArchiveDirectory => CombineDirectory(MachineDirectory, "archive");
        /// <summary>
        /// アプリケーションアップデート用アーカイブ配置ディレクトリ。
        /// </summary>
        public DirectoryInfo MachineUpdateDirectory => CombineDirectory(MachineArchiveDirectory, "application");

        /// <summary>
        /// 一時ディレクトリ。
        /// </summary>
        public DirectoryInfo TemporaryDirectory { get; private set; }

        /// <summary>
        /// 設定格納DBファイル。
        /// </summary>
        public FileInfo SettingFile => CombineFile(UserSettingDirectory, "setting.sqlite3");
        /// <summary>
        /// ファイル格納DBファイル。
        /// </summary>
        public FileInfo FileFile => CombineFile(UserSettingDirectory, "file.sqlite3");

        #endregion

        #region function

        public static EnvironmentParameters Initialize(DirectoryInfo rootDirectory, CommandLine commandLine)
        {
            if(Instance != null) {
                throw new InvalidOperationException();
            }
            if(!commandLine.IsParsed) {
                throw new ArgumentException(nameof(commandLine));
            }

            var current = new EnvironmentParameters() {
                RootDirectory = rootDirectory,
            };

            var projectName = "Pe2";
            current.UserRoamingDirectory = current.GetDirectory(commandLine, CommandLineKeyUserDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), projectName));
            current.MachineDirectory = current.GetDirectory(commandLine, CommandLineKeyMachineDirectory, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), projectName));
            current.TemporaryDirectory= current.GetDirectory(commandLine, CommandLineKeyTemporaryDirectory, Path.Combine(Path.GetTempPath(), projectName));

            Instance = current;

            return current;
        }

        DirectoryInfo GetDirectory(CommandLine commandLine, string key, string defaultValue)
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

        string CombinePath(string fullPath, string[] addPaths)
        {
            var paths = new List<string>(addPaths.Length + 1);
            paths.Add(fullPath);
            paths.AddRange(addPaths);

            var path = Path.Combine(paths.ToArray());
            return path;
        }

        DirectoryInfo CombineDirectory(DirectoryInfo directory, params string[] directoryNames)
        {
            var path = CombinePath(directory.FullName, directoryNames);
            return new DirectoryInfo(path);
        }

        FileInfo CombineFile(DirectoryInfo directory, params string[] directoryAndFileNames)
        {
            var path = CombinePath(directory.FullName, directoryAndFileNames);
            return new FileInfo(path);
        }

        #endregion
    }
}
