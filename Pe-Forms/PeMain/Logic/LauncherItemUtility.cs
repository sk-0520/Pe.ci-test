namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
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

		/// <summary>
		/// 指定ファイルパスがショートカットファイルである場合に
		/// ショートカット先のファイルパスを使用するか、
		/// ショートカットファイルそのものを使用するか問い合わせる。
		/// </summary>
		/// <param name="path">対象ファイルパス。</param>
		/// <param name="language">言語。</param>
		/// <param name="logger">ロガー。</param>
		/// <returns>pathと異なればショートカット先を使用、同じであればショートカットファイルそのものを使用する。なお、ファイル自体がショートカットでなければpathが返る。</returns>
		public static string InquiryUseShocutTarget(string path, Language language, ILogger logger)
		{
			if(PathUtility.IsShortcutPath(path)) {
				var result = MessageBox.Show(language["common/dialog/d-d/shortcut/message"], language["common/dialog/d-d/shortcut/caption"], MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if(result == DialogResult.Yes) {
					try {
						using(var sf = new ShortcutFile(path)) {
							return sf.TargetPath;
						}
					} catch(ArgumentException ex) {
						logger.Puts(LogType.Warning, ex.Message, ex);
					}
				}
			}

			return path;
		}

	}
}
