/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/17
 * 時刻: 21:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using System.Linq;

using PeMain.Logic;
using PeMain.Logic.DB;

namespace PeMain.UI
{
	partial class SettingForm
	{
		void SaveFileMainStartup()
		{
			var linkPath = Literal.StartupShortcutPath;
			if(this.selectMainStartup.Checked) {
				if(!File.Exists(linkPath)) {
					// 生成
					AppUtility.MakeAppShortcut(linkPath);
				}
			} else {
				if(File.Exists(linkPath)) {
					// 削除
					File.Delete(linkPath);
				}
			}
		}
		
		void SaveDBNoteItems(AppDBManager db)
		{
			if(this._noteItemList.Count > 0) {
				var removeList = this._noteItemList.Where(note => note.Remove).Select(note => note.NoteItem);
				var saveList = this._noteItemList.Where(note => !note.Remove).Select(note => note.NoteItem);
				
				var noteDB = new NoteDB(db);
				noteDB.ToDisabled(removeList);
				noteDB.Resist(saveList);
			}
		}
		
	}
}
