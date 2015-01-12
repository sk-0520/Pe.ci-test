using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	public static class AppUtility
	{
		/// <summary>
		/// 自身のショートカットを作成。
		/// </summary>
		/// <param name="savePath"></param>
		public static void MakeAppShortcut(string savePath)
		{
			var shortcut = new ShortcutFile(savePath, true);
			shortcut.TargetPath = Literal.ApplicationExecutablePath;
			shortcut.IconPath = Literal.ApplicationExecutablePath;
			shortcut.IconIndex = 0;
			shortcut.WorkingDirectory = Literal.ApplicationRootDirPath;
			shortcut.Save();
		}
		
		public static Image GetAppIcon(ISkin skin, IconScale iconScale)
		{
			return IconUtility.ImageFromIcon(skin.GetIcon(SkinIcon.App), iconScale);
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
		
		public static void BackupSetting(CommonData commonData, IEnumerable<string> targetFiles, string saveDirPath, int count)
		{
			var enabledFiles = targetFiles.Where(FileUtility.Exists);
			if (!enabledFiles.Any()) {
				return;
			}
			
			// バックアップ世代交代
			if(Directory.Exists(saveDirPath)) {
				var archiveList = Directory.GetFileSystemEntries(saveDirPath, "*.zip")
					.Where(File.Exists)
					.OrderByDescending(s => Path.GetFileName(s))
					.Skip(count - 1)
				;
				foreach(var path in archiveList) {
					try {
						File.Delete(path);
					} catch(Exception ex) {
						commonData.Logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
			
			var fileName = Literal.NowTimestampFileName + ".zip";
			var saveFilePath = Path.Combine(saveDirPath, fileName);
			FileUtility.MakeFileParentDirectory(saveFilePath);
			
			// zip
			using(var zip = new ZipArchive(new FileStream(saveFilePath, FileMode.Create), ZipArchiveMode.Create)) {
				var basePath = Literal.UserSettingDirPath;
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
			BackupSetting(commonData, backupFiles, Literal.UserBackupDirPath, Literal.backupCount);
			
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
	}
}
