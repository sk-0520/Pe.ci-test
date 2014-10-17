/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/12
 * 時刻: 0:10
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_main.
	/// </summary>
	partial class SettingForm
	{

		void SaveMainStartupFile()
		{
			var linkPath = Literal.StartupShortcutPath;
			if(this.selectMainStartup.Checked) {
				if(!File.Exists(linkPath)) {
					// 生成
					//
					/*
					var shortcut = new ShortcutFile(linkPath, true);
					shortcut.TargetPath = Literal.ApplicationExecutablePath; 
					shortcut.IconPath = Literal.ApplicationExecutablePath;
					shortcut.IconIndex = 0;
					shortcut.WorkingDirectory = Literal.ApplicationRootDirPath; 
					shortcut.Save();
					*/
					AppUtility.MakeAppShortcut(linkPath);
				}
			} else {
				if(File.Exists(linkPath)) {
					// 削除
					File.Delete(linkPath);
				}
			}
		}
	}
}
