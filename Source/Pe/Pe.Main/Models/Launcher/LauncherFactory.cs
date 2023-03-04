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
using ContentTypeTextNet.Pe.Standard.Base;
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
                    //TODO: コード取得
                    itemResult.Code = ToCode(Path.GetFileNameWithoutExtension(shortcut.TargetPath));

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
                itemResult.Code = ToCode(rawName);
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
        /// ランチャーアイテムコードを生成。
        /// <para>
        /// ランチャーアイテムコードはコマンドとかで使うユーザー入力可能な一意文字列で、
        /// IME使わずにコマンド入力したいし後々のことを考えてASCIIなものだけが許容される世界を目指す。
        /// </para>
        /// <list type="bullet">
        /// <item><description>ホワイトスペース, 許容できない記号は _ に変換する。</description></item>
        /// <item><description>コントロールコードは [c-ff-ff-...] に変換する。</description></item>
        /// <item><description>ASCII範囲外は [x-ff-ff-...] に変換する。</description></item>
        /// <item><description>ASCII範囲外でカタカナは平仮名はローマ字に変換する。</description></item>
        /// <item><description>アルファベットは機械的に小文字。</description></item>
        /// <item><term>許容する記号</term><description><see cref="CodeSymbols"/>を参照</description></item>
        /// </list>
        /// </summary>
        /// <param name="source">ランチャーアイテムコードのもとになる文字列。<para>想定用途はファイル名。</para></param>
        /// <returns></returns>
        public string ToCode(string source)
        {
            if(source == null) {
                throw new ArgumentNullException(nameof(source));
            }

            static bool IsAscii(char c) => 0x00 <= c && c <= 0x7f;
            static void AppendBinary(string s, string head, IResultBuffer rb)
            {
                var currentBinary = Encoding.UTF8.GetBytes(s);
                rb.Append('[');
                rb.Append(head);
                foreach(var b in currentBinary) {
                    rb.Append('-');
                    rb.AppendFormat("{0:x2}", b);
                }
                rb.Append(']');
            }

            var textConverter = new TextConverter();
            var funcs = new Func<string, string>[] {
                textConverter.ConvertZenkakuDigitToAsciiDigit,
                textConverter.ConvertZenkakuAlphabetToAsciiAlphabet,
                textConverter.ConvertHankakuKatakanaToZenkakuKatakana,
                textConverter.ConvertKatakanaToHiragana,
                textConverter.ConvertHiraganaToAsciiRome,
                // 全部小文字
                s => s.ToLowerInvariant(),
                // 漢字・平仮名をなんとかする
                s => {
                    return textConverter.ConvertToCustom(s, (IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer) => {
                        // ASCII範囲外を頑張る
                        if(currentText.Length != 1 || !IsAscii(currentText[0])) {
                            AppendBinary(currentText, "x", resultBuffer);
                        }

                        return 0;
                    });
                },
                // 使用不可文字をなんとかする
                s => {
                    return textConverter.ConvertToCustom(s, (IReadOnlyList<string> characterBlocks, int currentIndex, bool isLastIndex, string currentText, IResultBuffer resultBuffer) => {
                        // ここまで来てASCII範囲外はいないでしょ・・・
                        Debug.Assert(currentText.Length == 1);
                        Debug.Assert(IsAscii(currentText[0]));

                        var c = currentText[0];
                        if(!char.IsLetterOrDigit(c)) {
                            if(char.IsWhiteSpace(c)) {
                                resultBuffer.Append('_');
                            } else if(char.IsControl(c)) {
                                AppendBinary(currentText, "c", resultBuffer);
                            } else {
                                if(!CodeSymbols.Contains(c)) {
                                    resultBuffer.Append('_');
                                }
                            }
                        }
                        return 0;
                    });
                }
            };

            var result = source.Trim();
            foreach(var func in funcs) {
                result = func(result);
            }

            return result;
        }

        public string GetUniqueCode(string code, IReadOnlyList<string> codes)
        {
            return TextUtility.ToUnique(code, codes, StringComparison.OrdinalIgnoreCase, (s, n) => $"{s}-{n}");

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
