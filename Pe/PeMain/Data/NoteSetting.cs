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
using System.Diagnostics;
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
			lock(this._db) {
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
		}
		
		void ResistTNote(NoteItem noteItem, DateTime timestamp)
		{
			var tNote = new TNoteEntity();
			tNote.Id = noteItem.NoteId;
			var tempTNote = this._db.GetEntity(tNote);
			if(tempTNote != null) {
				tNote = tempTNote;
			} else {
				tNote.CommonCreate = timestamp;
			}
			tNote.CommonUpdate = timestamp;
			tNote.Body = noteItem.Title;
			
			if(tempTNote != null) {
				this._db.ExecuteUpdate(new [] { tNote });
			} else {
				this._db.ExecuteInsert(new [] { tNote });
			}
		}
		
		void ResistTNoteStyle(NoteItem noteItem, DateTime timestamp)
		{
			var tNoteStyle = new TNoteStyleEntity();
			tNoteStyle.Id = noteItem.NoteId;
			var tempTNoteStyle = this._db.GetEntity(tNoteStyle);
			if(tempTNoteStyle != null) {
				tNoteStyle = tempTNoteStyle;
			} else {
				tNoteStyle.CommonCreate = timestamp;
			}
			tNoteStyle.CommonUpdate = timestamp;
			
			tNoteStyle.ForeColor = noteItem.Style.ForeColor;
			tNoteStyle.BackColor = noteItem.Style.BackColor;
			if(noteItem.Style.FontSetting.IsDefault) {
				tNoteStyle.FontFamily = string.Empty;
			} else {
				tNoteStyle.FontFamily = noteItem.Style.FontSetting.Family;
			}
			tNoteStyle.FontHeight = noteItem.Style.FontSetting.Height;
			tNoteStyle.FontBold = noteItem.Style.FontSetting.Bold;
			tNoteStyle.FontItalic = noteItem.Style.FontSetting.Italic;
			tNoteStyle.Visibled = noteItem.Visibled;
			tNoteStyle.Locked = noteItem.Locked;
			tNoteStyle.Topmost = noteItem.Topmost;
			tNoteStyle.Compact = noteItem.Compact;
			tNoteStyle.Location = noteItem.Location;
			tNoteStyle.Size = noteItem.Size;
			
			if(tempTNoteStyle != null) {
				this._db.ExecuteUpdate(new [] { tNoteStyle });
			} else {
				this._db.ExecuteInsert(new [] { tNoteStyle });
			}
		}
		
		public void ResistItem(NoteItem noteItem)
		{
			lock(this._db) {
				using(var tran = this._db.BeginTransaction()) {
					var timestamp = DateTime.Now;
					
					var mNote = new MNoteEntity();
					mNote.Id = noteItem.NoteId;
					mNote = this._db.GetEntity(mNote);
					Debug.Assert(mNote != null, noteItem.ToString());
					mNote.Title = noteItem.Title;
					mNote.RawType = (int)NoteType.Text;
					mNote.Title = noteItem.Title;
					mNote.CommonUpdate = timestamp;
					
					ResistTNote(noteItem, timestamp);
					ResistTNoteStyle(noteItem, timestamp);
					
					tran.Commit();
				}
			}
		}
		
		public void Dispose()
		{
			CaptionFontSetting.Dispose();
		}
		
	}
}
