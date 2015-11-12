namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.CompilerServices;
	using System.Threading;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Skin.SystemSkin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.UI;

	/// <summary>
	/// アプリケーションの共通処理。
	/// </summary>
	public static class AppUtility
	{
		/// <summary>
		/// 自身のショートカットを作成。
		/// </summary>
		/// <param name="savePath">保存先パス。</param>
		public static void MakeAppShortcut(string savePath)
		{
			using(var shortcut = new ShortcutFile()) {
				shortcut.TargetPath = Literal.ApplicationExecutablePath;
				shortcut.WorkingDirectory = Literal.ApplicationRootDirectoryPath;
				shortcut.SetIcon(new IconPath(Literal.ApplicationExecutablePath, 0));
				shortcut.Save(savePath);
			}
		}
		
		/// <summary>
		/// 拡張状態か。
		/// </summary>
		/// <returns></returns>
		public static bool IsExtension()
		{
			return Control.ModifierKeys == Keys.Shift;
		}

		public static void RotateFile(string baseFile, string targetWildcardName, int count, ILogger logger)
		{
			// バックアップ世代交代
			if(Directory.Exists(baseFile)) {
				var archiveList = Directory.GetFileSystemEntries(baseFile, targetWildcardName)
					.Where(File.Exists)
					.OrderByDescending(Path.GetFileName)
					.Skip(count - 1)
				;
				foreach(var path in archiveList) {
					try {
						File.Delete(path);
					} catch(Exception ex) {
						logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
		}

		static ZipArchiveEntry WriteArchive(ZipArchive archive, string path, string baseDirPath)
		{
			var entryPath = path.Substring(baseDirPath.Length);
			while(entryPath.First() == Path.DirectorySeparatorChar) {
				entryPath = entryPath.Substring(1);
			}

			var entry = archive.CreateEntry(entryPath);

			using(var entryStream = new BinaryWriter(entry.Open())) {
				var buffer = FileUtility.ToBinary(path);
				entryStream.Write(buffer);
			}

			return entry;
		}

		/// <summary>
		/// 指定パスにZIP形式でアーカイブを作成。
		/// </summary>
		/// <param name="saveFilePath">保存先パス。</param>
		/// <param name="basePath">基準とするディレクトリパス。</param>
		/// <param name="targetFiles">取り込み対象パス。</param>
		public static void WriteZip(string saveFilePath, string basePath, IEnumerable<string> targetFiles)
		{
			using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				foreach(var filePath in targetFiles) {
					if(File.Exists(filePath)) {
						WriteArchive(zip, filePath, basePath);
					} else if(Directory.Exists(filePath)) {
						var list = Directory.EnumerateFiles(filePath, "*", SearchOption.AllDirectories);
						foreach(var f in list) {
							WriteArchive(zip, f, basePath);
						}
					}
				}
			}
		}

		/// <summary>
		/// 設定バックアップ。
		/// </summary>
		/// <param name="targetFiles">保存対象パス。</param>
		/// <param name="saveDirPath">保存ディレクトリ。</param>
		/// <param name="rotateCount">ローテート対象となるファイル数。</param>
		/// <param name="logger"></param>
		public static void BackupSetting(IEnumerable<string> targetFiles, string saveDirPath, int rotateCount, ILogger logger)
		{
			var enabledFiles = targetFiles.Where(FileUtility.Exists);
			if (!enabledFiles.Any()) {
				return;
			}
			
			// バックアップ世代交代
			RotateFile(saveDirPath, "*.zip", rotateCount, logger);
			
			var fileName = PathUtility.AppendExtension(Literal.NowTimestampFileName, "zip");
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			WriteZip(saveFilePath, Literal.UserSettingDirectoryPath, enabledFiles);
		}

		/// <summary>
		/// 現在の設定データを保存する。
		/// </summary>
		public static void SaveSetting(CommonData commonData)
		{
			// バックアップ
			var backupFiles = new [] {
				Literal.UserMainSettingPath,
				Literal.UserLauncherItemsPath,
				Literal.UserDBPath,
				Literal.UserClipboardItemsPath,
				Literal.UserTemplateItemsPath,
				Literal.ApplicationSettingBaseDirectoryPath,
			};
			BackupSetting(backupFiles, Literal.UserBackupDirectoryPath, Literal.backupCount, commonData.Logger);
			
			// 保存開始
			// メインデータ
			Serializer.SaveXmlFile(commonData.MainSetting, Literal.UserMainSettingPath);
			// ランチャーデータ
			var sortedSet = new HashSet<LauncherItem>();
			foreach(var item in commonData.MainSetting.Launcher.Items.OrderBy(item => item.Name)) {
				sortedSet.Add(item);
			}
			Serializer.SaveXmlFile(sortedSet, Literal.UserLauncherItemsPath);
			// クリップボードデータ
			if(commonData.MainSetting.Clipboard.SaveHistory && commonData.MainSetting.Clipboard.SaveTypes != ClipboardType.None) {
				var saveList = ClipboardUtility.FilterClipboardItemList(commonData.MainSetting.Clipboard.HistoryItems, commonData.MainSetting.Clipboard.SaveTypes);
				if(saveList.Any()) {
					Serializer.SaveCompressFile(saveList, Literal.UserClipboardItemsPath);
				} else {
					// 削除する
					if(File.Exists(Literal.UserClipboardItemsPath)) {
						try {
							File.Delete(Literal.UserClipboardItemsPath);
						} catch(Exception ex) {
							commonData.Logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage(Literal.UserClipboardItemsPath, ex));
						}
					}
				}
			}

			// テンプレートデータ
			Serializer.SaveXmlFile(commonData.MainSetting.Clipboard.TemplateItems, Literal.UserTemplateItemsPath);
		}

		/// <summary>
		/// スキンを取得。
		/// 
		/// 取得したスキンはISkin.Loadまで処理する。
		/// </summary>
		/// <remarks></remarks>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static HashSet<ISkin> GetSkins(ILogger logger)
		{
			var result = new HashSet<ISkin>() {
				new SystemSkin(),
			};

			var skinDllList = Directory.EnumerateFiles(Literal.ApplicationSkinDirectoryPath, "?*Skin.dll", SearchOption.TopDirectoryOnly);
			foreach(var skinDll in skinDllList.Where(s => string.Compare("SystemSkin", Path.GetFileNameWithoutExtension(s), true) != 0)) {
				var assembly = Assembly.LoadFrom(skinDll);
				foreach(var type in assembly.GetTypes()) {
					if(!type.IsClass || type.IsAbstract || type.IsNotPublic || !type.IsVisible) {
						continue;
					}
					if(type.GetInterface("ContentTypeTextNet.Pe.Library.Skin.ISkin") != null) {
						var ctor = type.GetConstructor(Type.EmptyTypes);
						if(ctor == null) {
							logger.Puts(LogType.Error, "default constructor: " + skinDll, type);
							continue;
						}
						var instance = ctor.Invoke(new object[] { });
						if(instance == null) {
							logger.Puts(LogType.Error, "create instance: " + skinDll, ctor);
							continue;
						}
						try {
							var skin = (ISkin)instance;
							result.Add(skin);
						} catch(Exception ex) {
							logger.Puts(LogType.Error, ex.Message , new ExceptionMessage(skinDll, ex));
						}
					}
				}
			}

			foreach(var skin in result) {
				skin.Load();
			}

			return result;
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
		/// <param name="member"></param>
		/// <returns></returns>
		public static Icon LoadIcon(IconPath iconPath, IconScale iconScale, TimeSpan waitTime, int waitMaxCount, ILogger logger, [CallerMemberName] string member = "")
		{
			Debug.Assert(FileUtility.Exists(iconPath.Path));

			var waitCount = 0;
			while(waitCount <= waitMaxCount) {
				var icon = IconUtility.Load(iconPath.Path, iconScale, iconPath.Index);
				if(icon != null) {
					return icon;
				} else {
					logger.PutsDebug(iconPath.Path, () => string.Format("{0} -> wait: {1} ms, count: {2}", member, waitTime.TotalMilliseconds, waitCount));
					Thread.Sleep(Literal.loadIconRetryTime);
					waitCount++;
				}
			}

			return null;
		}

		/// <summary>
		/// アプリケーションアイコンとランチャーアイテムのアイコンをうまいこと結合する。
		/// </summary>
		/// <param name="skin"></param>
		/// <param name="launcherItem"></param>
		/// <param name="iconScale"></param>
		/// <returns></returns>
		public static Icon GetAppLauncherItem(CommonData commonData, LauncherItem launcherItem, IconScale iconScale)
		{
			var bitmapSize = iconScale.ToSize();

			using(var bitmap = new Bitmap(bitmapSize.Width, bitmapSize.Height)) {
				using(var g = Graphics.FromImage(bitmap)) {
					g.DrawIcon(commonData.Skin.GetIcon(SkinIcon.App), 0, 0);
					// アイテムの描画
					// TODO: 引数のアイコンサイズから動的に変わってない。
					var icon = launcherItem.GetIcon(IconScale.Small, launcherItem.IconItem.Index, commonData.ApplicationSetting, commonData.Logger);
					var iconSize = IconScale.Small.ToSize();
					g.DrawIcon(icon, iconSize.Width, iconSize.Height);
				}

				using(var hIcon = UnmanagedIcon.FromBitmap(bitmap)) {
					return hIcon.ToManagedIcon();
				}
			}
		}

		public static Color GetToolbarPositionColor(bool isFore, bool isMain)
		{
			var alpha = 80;
			if(isFore) {
				return isMain ? SystemColors.ActiveCaptionText : Color.FromArgb(alpha, SystemColors.InactiveCaptionText);
			} else {
				return isMain ? SystemColors.ActiveCaption : Color.FromArgb(alpha, SystemColors.InactiveCaption);
			}
		}

		public static Size GetButtonSize(Size size)
		{
			return new Size(
				size.Width + NativeMethods.GetSystemMetrics(SM.SM_CXEDGE) * 4,
				size.Height + NativeMethods.GetSystemMetrics(SM.SM_CYEDGE) * 4
			);
		}

		public static bool ExecuteItem(CommonData commonData, LauncherItem launcherItem)
		{
			try {
				Executor.RunItem(launcherItem, commonData);
				launcherItem.Increment(null, null);
				return true;
			} catch(Exception ex) {
				commonData.Logger.Puts(LogType.Warning, ex.Message, ex);
			}

			return false;
		}

		public static ExecuteForm ShowExecuteEx(CommonData commonData, LauncherItem launcherItem, IEnumerable<string> exOptions)
		{
			var form = new ExecuteForm() {
				LauncherItem = launcherItem,
				ExOptions = exOptions,
			};
			form.SetCommonData(commonData);
			commonData.RootSender.AppendWindow(form);
			form.Show();
			form.FormClosed += ExecuteFormClosed;
			return form;
		}

		static void ExecuteFormClosed(object sender, FormClosedEventArgs e)
		{
			var form = (ExecuteForm)sender;
			if(form.DialogResult == DialogResult.OK) {
				var editedItem = form.EditedLauncherItem;
				if(ExecuteItem(form.CommonData, editedItem)) {
					form.LauncherItem.Increment(editedItem.Option, editedItem.WorkDirPath);
				}
			}
		}

		public static int GetTuneItemHeight(Padding padding, IconScale iconScale, Font font)
		{
			var iconHeight = iconScale.ToHeight();
			var fontHeight = font.Height;
			var itemHeight = Math.Max(iconHeight, fontHeight) + padding.Vertical + 1 * 2;
			return itemHeight;
		}

	}
}
