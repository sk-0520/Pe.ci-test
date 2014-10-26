/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/16
 * 時刻: 23:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using PeUtility;

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
	}
}
