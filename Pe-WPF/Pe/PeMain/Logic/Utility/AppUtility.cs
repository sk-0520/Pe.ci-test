namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
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

	public static class AppUtility
	{
		public static T LoadSetting<T>(string path, FileType fileType, ILogger logger)
			where T: ModelBase, new()
		{
			logger.Information("load setting", path);
			T result = null;
			if(File.Exists(path)) {
				switch(fileType) {
					case FileType.Json:
						result = SerializeUtility.LoadJsonDataFromFile<T>(path);
						break;

					case FileType.Binary:
						result = SerializeUtility.LoadBinaryDataFromFile<T>(path);
						break;

					default:
						throw new NotImplementedException();
				}
				logger.Debug("load data", result != null ? typeof(T).Name: "null");
			} else {
				logger.Debug("file not found", path);
			}

			return result ?? new T();
		}

		public static void SaveSetting<T>(string path, T model, FileType fileType, ILogger logger)
			where T: ModelBase
		{
			logger.Information("save setting", path);
			switch(fileType) {
				case FileType.Json:
					SerializeUtility.SaveJsonDataToFile(path, model);
					break;

				case FileType.Binary:
					SerializeUtility.SaveBinaryDataToFile(path, model);
					break;

				default:
					throw new NotImplementedException();
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
			logger.Information("load language file", baseDir);
			var langPairList = new List<KeyValuePair<string, LanguageCollectionModel>?>();
			foreach(var path in Directory.EnumerateFiles(baseDir, Constants.languageSearchPattern)) {
				try {
					var model = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(path);
					var pair = new KeyValuePair<string, LanguageCollectionModel>(path, model);
					langPairList.Add(pair);
				} catch(Exception ex) {
					logger.Error(ex);
				}
			}

			var defaultPath = Path.Combine(baseDir, Constants.languageDefaultFileName);
			var lang = langPairList.FirstOrDefault(p => p.Value.Value.Name == name)
				?? langPairList.FirstOrDefault(l => l.Value.Value.CultureCode == cultureCode)
				?? new KeyValuePair<string, LanguageCollectionModel>(defaultPath, SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(defaultPath))
			;
			return new AppLanguageManager(lang.Value, lang.Key);
		}

		/// <summary>
		/// ログ取りくん作成。
		/// <para>UI・設定に影響されない</para>
		/// </summary>
		/// <param name="outputFile"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static AppLogger CreateSystemLogger(bool outputFile, string baseDir)
		{
			var logger = new AppLogger();

			if(outputFile) {
				logger.LoggerConfig.PutsFile = true;
				var filePath = PathUtility.AppendExtension(Path.Combine(baseDir, Constants.GetNowTimestampFileName()), "log");
				FileUtility.MakeFileParentDirectory(filePath);
				logger.FilePath = filePath;
			}

			return logger;
		}

		/// <summary>
		/// 自身のショートカットを作成。
		/// </summary>
		/// <param name="savePath">保存先パス。</param>
		public static void MakeAppShortcut(string savePath, VariableConstants variableConstants)
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
		/// 
		/// なんやかんや色々あるけどアイコン再読み込みとか泥臭い処理を頑張る最上位の子。
		/// </summary>
		/// <param name="iconPath">アイコンパス(とインデックス)。</param>
		/// <param name="iconScale">アイコンサイズ。</param>
		/// <param name="waitTime">待ち時間</param>
		/// <param name="waitMaxCount">待ちを何回繰り返すか</param>
		/// <param name="logger"></param>
		/// <param name="callerMember"></param>
		/// <returns></returns>
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

		public static BitmapSource LoadIconDefault(IconPathModel iconPath, IconScale iconScale, ILogger logger = null, [CallerMemberName] string callerMember = "")
		{
			return LoadIcon(iconPath, iconScale, Constants.iconLoadWaitTime, Constants.iconLoadRetryMax, logger, callerMember);
		}

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
				var windowItem = new WindowItemModel() {
					Name = titleBuffer.ToString(),
					Process = process,
					WindowHandle = hWnd,
					WindowArea = PodStructUtility.Convert(rawRect),
				};
				windowItemList.Add(windowItem);
				return true;
			}, IntPtr.Zero
			);

			return windowItemList;
		}

		public static void ChangeWindowFromWindowList(IList<WindowItemModel> windowList)
		{
			foreach (var windowItem in windowList) {
				var rect = PodStructUtility.Convert(windowItem.WindowArea);
				var reslut = NativeMethods.MoveWindow(
					windowItem.WindowHandle,
					rect.X,
					rect.Y,
					rect.Width,
					rect.Height,
					true
				);
			}
		}
	}
}
