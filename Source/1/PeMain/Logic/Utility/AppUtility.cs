/*
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
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using System.Threading;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Pe.PeMain.Define;
using System.Windows;
using System.Windows.Media;

namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
    public static class AppUtility
    {
        public static void GarbageCollectionTemporaryFile(string parentDirectoryPath, ILogger logger)
        {
            logger.Debug("gc: temp files", parentDirectoryPath);

            if(!Directory.Exists(parentDirectoryPath)) {
                logger.Debug("gc: not found directory", parentDirectoryPath);
                return;
            }
            var pathList = Directory
                .EnumerateFiles(parentDirectoryPath, Constants.TemporaryFileSearchPattern, SearchOption.TopDirectoryOnly)
            ;
            long targetFileCount = 0;
            long removedFileCount = 0;
            foreach(var path in pathList) {
                targetFileCount += 1;
                try {
                    File.Delete(path);
                    removedFileCount += 1;
                } catch(Exception ex) {
                    logger.Warning(path, ex);
                }
            }
            logger.Debug($"gc: {removedFileCount}/{targetFileCount}", parentDirectoryPath);
        }

        public static IEnumerable<KeyValuePair<string, LanguageCollectionModel>?> GetLanguageFiles(string baseDir, ILogger logger)
        {
            foreach(var path in Directory.EnumerateFiles(baseDir, Constants.languageSearchPattern)) {
                LanguageCollectionModel model = null;
                try {
                    model = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(path);
                } catch(Exception ex) {
                    logger.Error(ex);
                    continue;
                }
                Debug.Assert(model != null);

                var pair = new KeyValuePair<string, LanguageCollectionModel>(path, model);
                yield return pair;
            }
        }

        /// <summary>
        /// 指定ディレクトリ内から指定した言語名の言語ファイルを取得する。
        /// </summary>
        /// <param name="baseDir">検索ディレクトリ</param>
        /// <param name="name">検索名</param>
        /// <param name="cultureCode">検索コード</param>
        /// <returns></returns>
        public static AppLanguageManager LoadLanguageFile(string baseDir, string name, string cultureCode, ILogger logger)
        {
            logger.Debug("load: language file", baseDir);
            var defaultPath = Path.Combine(baseDir, Constants.languageDefaultFileName);

            var langPairList = GetLanguageFiles(baseDir, logger)
                .Where(p => string.Compare(p?.Key, defaultPath, StringComparison.OrdinalIgnoreCase) != 0)
            ;

            var baseLang = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(defaultPath);

            var lang = langPairList.FirstOrDefault(p => p.Value.Value.Name == name)
                ?? langPairList.FirstOrDefault(l => l.Value.Value.CultureCode == cultureCode)
                ?? new KeyValuePair<string, LanguageCollectionModel>(defaultPath, baseLang)
            ;
            if(lang.Value !=  baseLang) {
                // マージ
                var useLang = lang.Value;

                var dk = baseLang.Define.Select(l => l.Id).Except(useLang.Define.Select(l => l.Id)).ToArray();
                useLang.Define.AddRange(baseLang.Define.Where(l => dk.Any(k => k == l.Id)).ToArray());

                var wk = baseLang.Words.Select(l => l.Id).Except(useLang.Words.Select(l => l.Id)).ToArray();
                useLang.Words.AddRange(baseLang.Words.Where(l => wk.Any(k => k == l.Id)).ToArray());
            }
            return new AppLanguageManager(lang.Value, lang.Key);
        }

        /// <summary>
        /// ファイルに対するログ出力用ストリームの作成。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>自動的にフラッシュするストリーム。</returns>
        public static StreamWriter CreateFileLoggerStream(string filePath)
        {
            FileUtility.MakeFileParentDirectory(filePath);

            var stream = new StreamWriter(filePath, true, Encoding.UTF8) {
                AutoFlush = true,
            };
            return stream;
        }

        /// <summary>
        /// ログ取りくん作成。
        /// <para>UI・設定に影響されない</para>
        /// </summary>
        /// <param name="outputFile"></param>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public static AppLogger CreateSystemLogger(bool outputFile, string baseDir)
        {
            var logger = new AppLogger();

            if(outputFile) {
                logger.LoggerConfig.PutsStream = true;
                var path = Path.Combine(baseDir, PathUtility.AppendExtension(Constants.GetNowTimestampFileName(), Constants.logFileExtension));
                var stream = CreateFileLoggerStream(path);
                logger.AttachStream(stream, true);
            }

            return logger;
        }

        /// <summary>
        /// 自身のショートカットを作成。
        /// </summary>
        /// <param name="savePath">保存先パス。</param>
        public static void MakeAppShortcut(string savePath)
        {
            using(var shortcut = new ShortcutFile()) {
                shortcut.TargetPath = Constants.applicationExecutablePath;
                shortcut.WorkingDirectory = Constants.applicationRootDirectoryPath;
                shortcut.SetIcon(new IconPathModel() {
                    Path = Constants.applicationExecutablePath,
                    Index = 0,
                });
                shortcut.Save(savePath);
            }
        }

        /// <summary>
        /// アイコン読み込み処理。
        /// <para>なんやかんや色々あるけどアイコン再読み込みとか泥臭い処理を頑張る最上位の子。</para>
        /// </summary>
        /// <param name="iconPath">アイコンパス(とインデックス)。</param>
        /// <param name="iconScale">アイコンサイズ。</param>
        /// <param name="waitTime">待ち時間</param>
        /// <param name="waitMaxCount">待ちを何回繰り返すか</param>
        /// <param name="logger"></param>
        /// <param name="callerMember"></param>
        /// <returns>読み込まれたアイコン。読み込みできなかった場合は null を返す。</returns>
        public static BitmapSource LoadIcon(IconPathModel iconPath, IconScale iconScale, TimeSpan waitTime, int waitMaxCount, ILogger logger = null, [CallerMemberName] string callerMember = "")
        {
            Debug.Assert(FileUtility.Exists(iconPath.Path));

            var waitCount = 0;
            while(waitCount <= waitMaxCount) {
                var icon = IconUtility.Load(iconPath.Path, iconScale, iconPath.Index);
                if(icon != null) {
                    return icon;
                } else {
                    logger.SafeDebug(iconPath.Path, string.Format("{0} -> wait: {1} ms, count: {2}", callerMember, waitTime.TotalMilliseconds, waitCount));
                    Thread.Sleep(waitTime);
                    waitCount++;
                }
            }

            return null;
        }

        /// <summary>
        /// LoadIconを規定値で使用。
        /// </summary>
        /// <param name="iconPath"></param>
        /// <param name="iconScale"></param>
        /// <param name="logger"></param>
        /// <param name="callerMember"></param>
        /// <returns></returns>
        public static BitmapSource LoadIconDefault(IconPathModel iconPath, IconScale iconScale, ILogger logger = null, [CallerMemberName] string callerMember = "")
        {
            return LoadIcon(iconPath, iconScale, Constants.iconLoadWaitTime, Constants.iconLoadRetryMax, logger, callerMember);
        }

        public static BitmapSource LoadLauncherItemIcon(IconScale iconScale, LauncherItemModel launcherItem, LauncherIconCaching launcherIconCaching, INonProcess nonProcess)
        {
            return launcherIconCaching[iconScale].Get(
                launcherItem, 
                () => {
                    var icon = LauncherItemUtility.GetIcon(launcherItem, iconScale, nonProcess);
                    FreezableUtility.SafeFreeze(icon);
                    return icon;
                }
            );
        }

        /// <summary>
        /// ウィンドウ一覧の取得。
        /// </summary>
        /// <param name="getAppWindow">自身のウィンドウも取得対象とするか。</param>
        /// <returns></returns>
        public static IList<WindowItemModel> GetSystemWindowList(bool getAppWindow)
        {
            // http://msdn.microsoft.com/en-us/library/windows/desktop/ms633574(v=vs.85).aspx
            var skipClassName = new[] {
                "Shell_TrayWnd", // タスクバー
                "Button",
                "Progman", // プログラムマネージャ
                "#32769", // デスクトップ
                "WorkerW",
                "SysShadow",
                "SideBar_HTMLHostWindow",
            };

            var myProcess = Process.GetCurrentProcess();
            var windowItemList = new List<WindowItemModel>();

            NativeMethods.EnumWindows((hWnd, lParam) => {
                int processId;
                NativeMethods.GetWindowThreadProcessId(hWnd, out processId);
                var process = Process.GetProcessById(processId);
                if(!getAppWindow) {
                    if(myProcess.Id == process.Id) {
                        return true;
                    }
                }

                if(!NativeMethods.IsWindowVisible(hWnd)) {
                    return true;
                }

                var classBuffer = new StringBuilder(WindowsUtility.classNameLength);
                NativeMethods.GetClassName(hWnd, classBuffer, classBuffer.Capacity);
                var className = classBuffer.ToString();
                if(skipClassName.Any(s => s == className)) {
                    return true;
                }

                var titleLength = NativeMethods.GetWindowTextLength(hWnd);
                var titleBuffer = new StringBuilder(titleLength + 1);
                NativeMethods.GetWindowText(hWnd, titleBuffer, titleBuffer.Capacity);
                var rawRect = new RECT();
                NativeMethods.GetWindowRect(hWnd, out rawRect);
                var isZoomed = NativeMethods.IsZoomed(hWnd);
                var isIconic = NativeMethods.IsIconic(hWnd);
                WindowState windowState;
                if(isZoomed || isIconic) {
                    if(isZoomed) {
                        windowState = WindowState.Maximized;
                    } else {
                        Debug.Assert(isIconic);
                        windowState = WindowState.Minimized;
                    }
                } else {
                    windowState = WindowState.Normal;
                }
                var windowItem = new WindowItemModel() {
                    Name = titleBuffer.ToString(),
                    Process = process,
                    WindowHandle = hWnd,
                    WindowArea = PodStructUtility.Convert(rawRect),
                    WindowState = windowState,
                };
                windowItemList.Add(windowItem);
                return true;
            }, IntPtr.Zero
            );

            return windowItemList;
        }

        public static void ChangeWindowFromWindowList(IList<WindowItemModel> windowList)
        {
            foreach(var windowItem in windowList) {
                switch(windowItem.WindowState) {
                    case WindowState.Maximized:
                        NativeMethods.ShowWindow(windowItem.WindowHandle, SW.SW_MAXIMIZE);
                        break;

                    case WindowState.Minimized:
                        NativeMethods.ShowWindow(windowItem.WindowHandle, SW.SW_MINIMIZE);
                        break;

                    case WindowState.Normal:
                        {
                            if(NativeMethods.IsZoomed(windowItem.WindowHandle) || NativeMethods.IsIconic(windowItem.WindowHandle)) {
                                NativeMethods.ShowWindow(windowItem.WindowHandle, SW.SW_RESTORE);
                            }

                            var deviceWindowArea = PodStructUtility.Convert(windowItem.WindowArea);
                            var reslut = NativeMethods.MoveWindow(
                                windowItem.WindowHandle,
                                deviceWindowArea.X,
                                deviceWindowArea.Y,
                                deviceWindowArea.Width,
                                deviceWindowArea.Height,
                                true
                            );
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public static Color GetHotTrackColor(BitmapSource bitmapSource)
        {
            byte baseAplha = 120;
            var skipDark = 80;
            var skipLight = 170;
            var baseLight = 128;

            var pixels = MediaUtility.GetPixels(bitmapSource);
            var colors = MediaUtility.GetColors(pixels)
                .Where(c => c.A > baseAplha)
                .Where(c => !(c.R >= skipDark && c.G >= skipDark && c.B >= skipDark))
                .Where(c => !(c.R <= skipLight && c.G <= skipLight && c.B <= skipLight))
                .Where(c => c.R <= baseLight || c.G <= baseLight || c.B <= baseLight)
            ;
            return MediaUtility.GetPredominantColor(colors, baseAplha);
        }
    }
}
