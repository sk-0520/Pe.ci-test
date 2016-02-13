/**
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class LauncherItemUtility
    {
        public static BitmapSource GetIcon(LauncherItemModel model, IconScale iconScale, INonProcess nonProcess)
        {
            CheckUtility.DebugEnforceNotNull(model);
            CheckUtility.DebugEnforceNotNull(model.Icon);
            CheckUtility.DebugEnforceNotNull(nonProcess);

            var hasIcon = false;
            var useIcon = new IconPathModel();

            if(!string.IsNullOrWhiteSpace(model.Icon.Path)) {
                var expandIconPath = Environment.ExpandEnvironmentVariables(model.Icon.Path);
                hasIcon = FileUtility.Exists(expandIconPath);
                if(hasIcon) {
                    useIcon.Path = expandIconPath;
                    useIcon.Index = model.Icon.Index;
                }
            }
            if(!hasIcon) {
                if(!string.IsNullOrWhiteSpace(model.Command)) {
                    var expandCommandPath = Environment.ExpandEnvironmentVariables(model.Command);
                    hasIcon = FileUtility.Exists(expandCommandPath);
                    if(hasIcon) {
                        useIcon.Path = expandCommandPath;
                        useIcon.Index = 0;
                    }
                }
                if(!hasIcon && model.LauncherKind == LauncherKind.Command) {
                    return AppResource.GetLauncherCommandIcon(iconScale, nonProcess.Logger);
                }
            }

            if(hasIcon) {
                return AppUtility.LoadIconDefault(useIcon, iconScale, nonProcess.Logger);
            } else {
                return AppResource.GetNotFoundIcon(iconScale, nonProcess.Logger);
            }
        }

        public static BitmapSource MakeOverwrapIcon(LauncherItemModel model, INonProcess nonProcess)
        {
            // TODO: だりぃ
            return null;
        }

        /// <summary>
        /// コマンド選択用ファイルダイアログお表示する。
        /// </summary>
        /// <param name="defaultPath"></param>
        /// <param name="nonProcess"></param>
        /// <returns>選択されたファイル。未選択の場合は null 。</returns>
        public static string ShowOpenCommandDialog(string defaultPath, INonProcess nonProcess)
        {
            return DialogUtility.ShowOpenSingleFileDialog(defaultPath);
        }

        /// <summary>
        /// オプション選択用ファイルダイアログを表示する。
        /// </summary>
        /// <param name="defaultPath"></param>
        /// <returns>選択されたファイル群をまとめた文字列。未選択の場合は null 。</returns>
        public static string ShowOpenOptionDialog(string defaultPath)
        {
            var files = DialogUtility.ShowOpenMultiFileDialog(defaultPath);
            if(files != null) {
                return string.Join(" ", TextUtility.WhitespaceToQuotation(files));
            } else {
                return null;
            }
        }

        static public TagItemModel GetTag(string expandedPath)
        {
            var result = new TagItemModel();
            if(PathUtility.IsProgram(expandedPath) && File.Exists(expandedPath)) {
                var versionInfo = FileVersionInfo.GetVersionInfo(expandedPath);
                if(!string.IsNullOrEmpty(versionInfo.CompanyName)) {
                    result.Items.Add(versionInfo.CompanyName);
                }
            }

            return result;
        }

        public static LauncherItemModel CreateFromFile(string path, bool loadShortcut, INonProcess nonProcess)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(path);

            var isProgram = PathUtility.IsProgram(expandedPath);
            var isShortCut = PathUtility.IsShortcut(expandedPath);

            var result = new LauncherItemModel();
            SettingUtility.InitializeLauncherItem(result, null, nonProcess);
            result.LauncherKind = LauncherKind.File;

            if(loadShortcut && PathUtility.IsShortcut(expandedPath)) {
                using(var shortcut = new ShortcutFile(expandedPath)) {
                    result.Command = shortcut.TargetPath;

                    result.Option = shortcut.Arguments;
                    result.WorkDirectoryPath = shortcut.WorkingDirectory;

                    var icon = shortcut.GetIcon();
                    result.Icon.Path = icon.Path;
                    result.Icon.Index = icon.Index;

                    result.Comment = shortcut.Description;
                }
            } else {
                result.Command = path;
            }

            result.Name = FileUtility.GetName(expandedPath);

            result.Tag = GetTag(Environment.ExpandEnvironmentVariables(result.Command));

            return result;
        }


        public static bool FilterItemName(LauncherItemModel model, string pattern)
        {
            CheckUtility.DebugEnforceNotNull(model);

            if(string.IsNullOrWhiteSpace(pattern)) {
                return true;
            }

            var s = pattern.Trim();

            if(char.IsUpper(s[0])) {
                // 前方一致 + 大文字小文字区別あり
                return model.Name.StartsWith(s, StringComparison.InvariantCulture);
            } else {
                // 部分一致 + 大文字小文字区別無し
                return model.Name.IndexOf(s, StringComparison.InvariantCultureIgnoreCase) != -1;
            }
        }
    }
}
