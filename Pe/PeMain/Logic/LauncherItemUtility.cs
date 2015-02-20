namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	public static class LauncherItemUtility
	{
		/// <summary>
		/// ファイルパスからランチャーアイテムの生成。
		/// </summary>
		/// <param name="expandPath"></param>
		/// <param name="useShortcut"></param>
		/// <param name = "forceLauncherType"></param>
		/// <param name = "forceType"></param>
		/// <returns></returns>
		public static LauncherItem LoadFile(string expandPath, bool useShortcut, bool forceLauncherType, LauncherType forceType)
		{
#if DEBUG
			if(forceLauncherType) {
				Debug.Assert(forceType != LauncherType.None);
			}
#endif

			var item = new LauncherItem();

			item.Name = Path.GetFileNameWithoutExtension(expandPath);
			if(item.Name.Length == 0 && expandPath.Length >= @"C:\".Length) {
				var drive = DriveInfo.GetDrives().SingleOrDefault(d => d.Name == expandPath);
				if(drive != null) {
					item.Name = drive.VolumeLabel;
				}
			}
			if(item.Name.Length == 0) {
				item.Name = expandPath;
			}
			item.Command = expandPath;
			/*
			item.IconPath = filePath;
			item.IconIndex = 0;
			 */
			item.IconItem.Path = expandPath;
			item.IconItem.Index = 0;

			if(Directory.Exists(expandPath)) {
				// ディレクトリ
				if(forceLauncherType) {
					item.LauncherType = forceType;
				} else {
					item.LauncherType = LauncherType.Directory;
				}
			} else {
				// ファイルとかもろもろ
				if(forceLauncherType) {
					item.LauncherType = forceType;
				} else {
					item.LauncherType = LauncherType.File;
				}

				var dotExt = Path.GetExtension(expandPath);
				switch(dotExt.ToLower()) {
					case ".lnk":
						{
							if(!useShortcut) {
								using(var shortcut = new ShortcutFile(expandPath)) {
									item.Command = shortcut.TargetPath;
									item.Option = shortcut.Arguments;
									item.WorkDirPath = shortcut.WorkingDirectory;
									item.IconItem = new IconItem(shortcut.GetIcon());
									item.Note = shortcut.Description;
								}
							}
						}
						break;

					/*
				case ".url":
					item.LauncherType = LauncherType.URI;
					break;
					 */

					case ".exe":
						{
							var verInfo = FileVersionInfo.GetVersionInfo(item.Command);
							if(!string.IsNullOrEmpty(verInfo.ProductName)) {
								item.Name = verInfo.ProductName;
							}
							item.Note = verInfo.Comments;
							if(!string.IsNullOrEmpty(verInfo.CompanyName)) {
								item.Tag.Add(verInfo.CompanyName);
							}

						}
						break;

					default:
						break;
				}
			}


			Debug.Assert(item.Name.Length > 0);

			return item;
		}

		public static LauncherItem LoadFile(string expandPath, bool useShortcut)
		{
			return LoadFile(expandPath, useShortcut, false, LauncherType.None);
		}

		static public string GetUniqueName(LauncherItem item, IEnumerable<LauncherItem> seq)
		{
			return TextUtility.ToUniqueDefault(item.Name, seq.Select(i => i.Name));
		}
	}
}
