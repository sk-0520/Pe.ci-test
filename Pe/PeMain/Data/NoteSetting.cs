/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/18
 * 時刻: 12:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PeMain.Data.DB;
using PeMain.Logic;
using PeUtility;

namespace PeMain.Data
{
	
	/// <summary>
	/// 
	/// </summary>
	[Serializable]
	public class NoteSetting: Item, IDisposable
	{
		[XmlIgnore()]
		private PeDBManager _db;
		
		public NoteSetting()
		{
			CreateHotKey = new HotKeySetting();
			HiddenHotKey = new HotKeySetting();
			CompactHotKey = new HotKeySetting();
			
			CaptionFontSetting = new FontSetting();
		}
		
		/// <summary>
		/// 新規作成時のホットキー
		/// </summary>
		public HotKeySetting CreateHotKey { get; set; }
		public HotKeySetting HiddenHotKey { get; set; }
		public HotKeySetting CompactHotKey { get; set; }
		
		public FontSetting CaptionFontSetting { get; set; }
		
		public void setDatabase(PeDBManager db)
		{
			this._db = db;
		}
		
		public NoteItem InsertItem(NoteItem noteItem)
		{
			using(var tran = this._db.BeginTransaction()) {
				var noteDto = this._db.GetTableId("M_NOTE", "NOTE_ID");
				noteItem.NoteId = noteDto.MaxId + 1;
				var mNote = new MNoteEntity();
				mNote.Id = noteItem.NoteId;
				mNote.RawType = (int)NoteType.Text;
				mNote.Title = noteItem.Title;
				mNote.CommonCreate = mNote.CommonUpdate = DateTime.Now;
				mNote.CommonEnabled = true;
				this._db.ExecuteInsert(new [] { mNote });
				tran.Commit();
				return noteItem;
			}
		}
		
		public void ResistItem(NoteItem noteItem)
		{
			
		}
		
		public void Dispose()
		{
			CaptionFontSetting.Dispose();
		}
		
	}
}
