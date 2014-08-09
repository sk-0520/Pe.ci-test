/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/07/29
 * 時刻: 1:46
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PeMain.Data;
using PeMain.Logic;
using PeMain.Logic.DB;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_note.
	/// </summary>
	public partial class SettingForm
	{
		bool NoteValid()
		{
			
			
			return true;
		}

		void NoteExportSetting(NoteSetting noteSetting)
		{
			// ホットキー
			noteSetting.CreateHotKey = this.inputNoteCreate.HotKeySetting;
			noteSetting.HiddenHotKey = this.inputNoteHidden.HotKeySetting;
			noteSetting.CompactHotKey= this.inputNoteCompact.HotKeySetting;
			
			// フォント
			noteSetting.CaptionFontSetting = this.commandNoteCaptionFont.FontSetting;
			
		}
		
		void SaveNoteItems(PeDBManager db)
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
