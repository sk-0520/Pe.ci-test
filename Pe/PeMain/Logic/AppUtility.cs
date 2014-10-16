/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/16
 * 時刻: 23:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;

namespace PeMain.Logic
{
	public static class AppUtility
	{
		public static void MakeAppShortcut(string savePath)
		{
			var shortcut = new ShortcutFile(savePath, true);
			shortcut.TargetPath = Literal.ApplicationExecutablePath; 
			shortcut.IconPath = Literal.ApplicationExecutablePath;
			shortcut.IconIndex = 0;
			shortcut.WorkingDirectory = Literal.ApplicationRootDirPath; 
			shortcut.Save();
		}
	}
}
