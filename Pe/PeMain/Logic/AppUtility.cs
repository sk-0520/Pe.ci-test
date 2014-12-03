/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/16
 * 時刻: 23:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Utility;
using PeMain.Data;
using PeSkin;

namespace PeMain.Logic
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
		
		public static Image GetAppIcon(IconScale iconScale)
		{
			var iconSize = iconScale.ToSize();
			using(var icon = new Icon(global::PeMain.Properties.Images.App, iconSize)) {
				return icon.ToBitmap();
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
		
		public static Image CreateBoxColorImage(Color borderColor, Color backColor, Size size)
		{
			var image = new Bitmap(size.Width, size.Height);
			
			using(var g = Graphics.FromImage(image)) {
				using(var brush = new SolidBrush(backColor)) {
					using(var pen = new Pen(borderColor)) {
						g.FillRectangle(brush, new Rectangle(new Point(1, 1), new Size(size.Width - 2, size.Height - 2)));
						g.DrawRectangle(pen, new Rectangle(Point.Empty, new Size(size.Width - 1, size.Height - 1)));
					}
				}
			}
			
			return image;
		}
		
		public static Image CreateNoteBoxImage(Color color, Size size)
		{
			return CreateBoxColorImage(Color.FromArgb(160, DrawUtility.CalcAutoColor(color)), color, size);
		}
		
		public static void BackupSetting(CommonData commonData, IEnumerable<string> targetFiles, string saveDirPath, int count)
		{
			var enabledFiles = targetFiles.Where(File.Exists);
			if (!enabledFiles.Any()) {
				return;
			}
			
			// バックアップ世代交代
			if(Directory.Exists(saveDirPath)) {
				foreach(var path in Directory.GetFileSystemEntries(saveDirPath).OrderByDescending(s => Path.GetFileName(s)).Skip(count - 1)) {
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
				foreach(var filePath in enabledFiles) {
					var entry = zip.CreateEntry(Path.GetFileName(filePath));
					using(var entryStream = new BinaryWriter(entry.Open())) {
						var buffer = FileUtility.ToBinary(filePath);
						//var buffer = File.ReadAllBytes(filePath);
						/*
						using(var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
							var buffer = new byte[Literal.fileTempBufferLength];
							int readLength;
							while((readLength = fileStream.Read(buffer, 0, buffer.Length)) > 0) {
								entryStream.Write(buffer, 0, readLength);
							}
						}
						 */
						entryStream.Write(buffer);
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
		}
	}
}
