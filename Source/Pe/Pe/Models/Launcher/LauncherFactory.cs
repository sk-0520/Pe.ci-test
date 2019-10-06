using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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

        IIdFactory IdFactory { get; }
        ILogger Logger { get; }

        #endregion

        #region function

        /// <summary>
        ///
        /// </summary>
        /// <param name="file"></param>
        /// <param name="loadShortcut">ショートカットの内容を使用するか。</param>
        /// <returns></returns>
        public LauncherFileItemData FromFile(FileInfo file, bool loadShortcut)
        {
            var expandedPath = Environment.ExpandEnvironmentVariables(file.FullName);

            var isProgram = PathUtility.IsProgram(expandedPath);
            var isShortCut = PathUtility.IsShortcut(expandedPath);

            var result = new LauncherFileItemData() {
                LauncherItemId = IdFactory.CreateLauncherItemId(),
                //TODO: 名称取得
                Name = FileUtility.GetName(expandedPath),
            };
            if(loadShortcut && PathUtility.IsShortcut(expandedPath)) {
                using(var shortcut = new ShortcutFile(expandedPath)) {
                    //TODO: コード取得
                    result.Code = Path.GetFileNameWithoutExtension(shortcut.TargetPath);

                    result.PathExecute.Path = shortcut.TargetPath;
                    result.PathExecute.Option = shortcut.Arguments;
                    result.PathExecute.WorkDirectoryPath =  shortcut.WorkingDirectory;

                    result.Icon.Path = shortcut.IconPath;
                    result.Icon.Index = shortcut.IconIndex;
                    try {
                        result.Comment = shortcut.Description;
                    } catch(COMException ex) {
                        Logger.LogWarning(ex, "{0} {1}", ex.Message, expandedPath);
                    }

                }
            } else {
                result.Code = Path.GetFileNameWithoutExtension(file.Name);
                result.PathExecute.Path = file.FullName;
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
                LauncherGroupId = IdFactory.CreateLauncherGroupId(),
                Name = name,
                Kind = LauncherGroupKind.Normal,
                ImageName = LauncherGroupImageName.DirectoryNormal,
                ImageColor = Colors.Yellow,
            };
        }

        public string ToCode(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            var textConverter = new TextConverter();
            var funcs = new Func<string, string> [] {
                textConverter.ConvertZenkakuDigitToAsciiDigit,
                textConverter.ConvertZenkakuAlphabetToAsciiAlphabet,
                textConverter.ConvertHankakuKatakanaToZenkakuKatakana,
                textConverter.ConvertKatakaToHiragana,
                // 漢字・平仮名をなんとかする
                s => {
                    return s;
                },
                // 使用不可文字をなんとかする
                s => {
                    return s;
                }
            };

            var result = source.Trim();
            foreach(var func in funcs) {
                result = func(result);
            }

            return result;
        }

        #endregion
    }
}
