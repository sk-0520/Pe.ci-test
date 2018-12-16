using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;

namespace ContentTypeTextNet.Pe.Main.Model.Launcher
{
    public class LauncherCreator
    {
        public LauncherCreator(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateCurrentClass();
        }

        #region property

        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <param name="loadShortcut">ショートカットの内容を使用するか。</param>
        /// <returns></returns>
        public LauncherItemSimpleNewData FromFile(FileInfo file, bool loadShortcut)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);

            var isProgram = PathUtility.IsProgram(expandedPath);
            var isShortCut = PathUtility.IsShortcut(expandedPath);

            var result = new LauncherItemSimpleNewData() {
                LauncherItemId = Guid.NewGuid(),
                Kind = LauncherItemKind.File,
                Name = FileUtility.GetName(expandedPath),
            };
            if(loadShortcut && PathUtility.IsShortcut(expandedPath)) {
                using(var shortcut = new ShortcutFile(expandedPath)) {
                    result.Command.Command = shortcut.TargetPath;
                    result.Command.Option = shortcut.Arguments;
                    result.Command.WorkDirectoryPath =  shortcut.WorkingDirectory;

                    result.Icon.Path = shortcut.IconPath;
                    result.Icon.Index = shortcut.IconIndex;

                    result.Note = shortcut.Description;

                }
            } else {
                result.Command.Command = file.FullName;
            }

            return result;
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

        public LauncherGroupData CreateGroupData(string name)
        {
            return new LauncherGroupData() {
                LauncherGroupId = Guid.NewGuid(),
                Name = name,
                ImageName = LauncherGroupImageName.Directory,
                ImageColor = Colors.Yellow,
            };
        }

        #endregion
    }
}
