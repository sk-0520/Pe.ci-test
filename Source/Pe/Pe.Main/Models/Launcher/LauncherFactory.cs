using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Launcher
{
    public class LauncherFactory
    {
        public LauncherFactory(IIdFactory idFactory, ILoggerFactory loggerFactory)
        {
            IdFactory = idFactory;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private IIdFactory IdFactory { get; }
        private ILogger Logger { get; }

        public static IReadOnlyCollection<char> CodeSymbols { get; } = new[] { '-', '.', '^', '_', '[', ']', };

        /// <summary>
        /// ランチャーグループのグループシーケンス刻み。
        /// </summary>
        public int GroupItemStep { get; } = 10;
        /// <summary>
        /// ランチャーグループ内のランチャーアイテムシーケンス刻み。
        /// </summary>
        public int GroupItemsStep { get; } = 10;

        #endregion

        #region function

        /// <summary>
        ///TODO: 分離した方がよさげ
        /// </summary>
        /// <param name="file"></param>
        /// <param name="loadShortcut">ショートカットの内容を使用するか。</param>
        /// <returns></returns>
        internal LauncherFileItemData FromFile(FileSystemInfo file, bool loadShortcut)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);

            var isProgram = PathUtility.IsProgram(expandedPath);
            var isShortCut = PathUtility.IsShortcut(expandedPath);

            var itemResult = new LauncherItemData() {
                LauncherItemId = IdFactory.CreateLauncherItemId(),
                Kind = LauncherItemKind.File,
                //TODO: 名称取得
                Name = IOUtility.GetName(expandedPath),
                IsEnabledCommandLauncher = true,
            };
            var fileResult = new LauncherFileData();

            if(loadShortcut && PathUtility.IsShortcut(expandedPath)) {
                using(var shortcut = new ShortcutFile(expandedPath)) {
                    fileResult.Path = shortcut.TargetPath;
                    fileResult.Option = shortcut.Arguments;
                    fileResult.WorkDirectoryPath = shortcut.WorkingDirectory;

                    itemResult.Icon.Path = shortcut.IconPath;
                    itemResult.Icon.Index = shortcut.IconIndex;
                    try {
                        itemResult.Comment = shortcut.Description;
                    } catch(COMException ex) {
                        Logger.LogWarning(ex, "{0} {1}", ex.Message, expandedPath);
                    }

                }
            } else {
                var rawName = Path.GetFileNameWithoutExtension(file.Name);
                if(string.IsNullOrEmpty(rawName)) {
                    rawName = itemResult.Name;
                }
                fileResult.Path = file.FullName;
            }

            return new LauncherFileItemData(itemResult, fileResult);
        }

        public IEnumerable<string> GetTags(FileInfo file)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);
            if(PathUtility.IsProgram(expandedPath) && File.Exists(expandedPath)) {
                var versionInfo = FileVersionInfo.GetVersionInfo(expandedPath);
                if(!string.IsNullOrEmpty(versionInfo.CompanyName)) {
                    yield return versionInfo.CompanyName;
                }
            }
        }

        public LauncherGroupData CreateGroupData(string name, LauncherGroupKind kind)
        {
            return new LauncherGroupData() {
                LauncherGroupId = IdFactory.CreateLauncherGroupId(),
                Name = name,
                Kind = kind,
                ImageName = LauncherGroupImageName.DirectoryNormal,
                ImageColor = Colors.Yellow,
            };
        }

        /// <summary>
        /// グループ名から重複しないグループ名を生成。
        /// </summary>
        /// <param name="groupNames"></param>
        /// <returns></returns>
        public string CreateUniqueGroupName(IReadOnlyList<string> groupNames)
        {
            var uniqueGroupName = TextUtility.ToUnique(Properties.Resources.String_LauncherGroup_NewItem_Name, groupNames, StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}({n})");
            return uniqueGroupName;
        }

        #endregion
    }
}
