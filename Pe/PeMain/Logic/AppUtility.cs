namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.IO;
	using System.IO.Compression;
	using System.Linq;
	using System.Reflection;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.UI;

	public static class AppUtility
	{
		/// <summary>
		/// 自身のショートカットを作成。
		/// </summary>
		/// <param name="savePath"></param>
		public static void MakeAppShortcut(string savePath)
		{
			/*
			var shortcut = new ShortcutFile(savePath, true);
			shortcut.TargetPath = Literal.ApplicationExecutablePath;
			shortcut.IconPath = Literal.ApplicationExecutablePath;
			shortcut.IconIndex = 0;
			shortcut.WorkingDirectory = Literal.ApplicationRootDirectoryPath;
			shortcut.Save();
			*/
			var shortcut = new ShortcutFile2();
			shortcut.TargetPath = Literal.ApplicationExecutablePath;
			shortcut.WorkingDirectory = Literal.ApplicationRootDirectoryPath;
			shortcut.SetIcon(new IconPath(Literal.ApplicationExecutablePath, 0));
			shortcut.Save(savePath);
		}
		
		/// <summary>
		/// 拡張状態か。
		/// </summary>
		/// <returns></returns>
		public static bool IsExtension()
		{
			return Control.ModifierKeys == Keys.Shift;
		}
		
		public static ZipArchiveEntry WriteArchive(ZipArchive archive, string path, string baseDirPath)
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

		public static void BackupSetting(IEnumerable<string> targetFiles, string saveDirPath, int count, ILogger logger)
		{
			var enabledFiles = targetFiles.Where(FileUtility.Exists);
			if (!enabledFiles.Any()) {
				return;
			}
			
			// バックアップ世代交代
			RotateFile(saveDirPath, "*.zip", count, logger);
			
			var fileName = Literal.NowTimestampFileName + ".zip";
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				var basePath = Literal.UserSettingDirectoryPath;
				foreach(var filePath in enabledFiles) {
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
		/// 現在の設定データを保存する。
		/// </summary>
		public static void SaveSetting(CommonData commonData)
		{
			// バックアップ
			var backupFiles = new [] {
				Literal.UserMainSettingPath,
				Literal.UserLauncherItemsPath,
				Literal.UserDBPath,
				Literal.ApplicationSettingBaseDirectoryPath,
			};
			BackupSetting(backupFiles, Literal.UserBackupDirectoryPath, Literal.backupCount, commonData.Logger);
			
			// 保存開始
			// メインデータ
			Serializer.SaveFile(commonData.MainSetting, Literal.UserMainSettingPath);
			//ランチャーデータ
			var sortedSet = new HashSet<LauncherItem>();
			foreach(var item in commonData.MainSetting.Launcher.Items.OrderBy(item => item.Name)) {
				sortedSet.Add(item);
			}
			Serializer.SaveFile(sortedSet, Literal.UserLauncherItemsPath);
			//// クリップボードデータ
			//var list = new List<ClipboardItem>(commonData.MainSetting.Clipboard.Items);
			//Serializer.SaveFile(list, Literal.UserClipboardItemsPath);
		}

		/// <summary>
		/// スキンを取得。
		/// 
		/// 取得したスキンはISkin.Loadまで処理する。
		/// </summary>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static HashSet<ISkin> GetSkins(ILogger logger)
		{
			var result = new HashSet<ISkin>() {
				new SystemSkin(),
			};

			var skinDllList = Directory.EnumerateFiles(Literal.ApplicationSkinDirectoryPath, "?*Skin.dll", SearchOption.TopDirectoryOnly);
			foreach(var skinDll in skinDllList) {
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
							logger.Puts(LogType.Error, ex.Message + ": " + skinDll, ex);
						}
					}
				}
			}

			foreach(var skin in result) {
				skin.Load();
			}

			return result;
		}
	}
}
