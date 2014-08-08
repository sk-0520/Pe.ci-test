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
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using PeMain.Data.DB;
using PeMain.Logic;
using PeMain.Logic.DB;
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
		
		/// <summary>
		/// TODO: Dataから別のどこかへ委譲。
		/// </summary>
		/// <param name="enabledOnly"></param>
		/// <returns></returns>
		public IEnumerable<NoteItem> GetNoteItemList(bool enabledOnly)
		{
			var dtoList = this._db.GetResultList<NoteItemDto>(global::PeMain.Properties.SQL.GetNoteItemList);
			if(enabledOnly) {
				dtoList = dtoList.Where(dto => dto.CommonEnabled);
			}
			var count = dtoList.Count();
			if(count > 0) {
				var result = new List<NoteItem>(count);
				foreach(var dto in dtoList) {
					var noteItem = new NoteItem();
					
					noteItem.NoteId = dto.Id;
					
					noteItem.Title = dto.Title;
					noteItem.Body = dto.Body;
					noteItem.NoteType = NoteTypeUtility.ToNoteType(dto.RawType);
					
					noteItem.Visible = dto.Visibled;
					noteItem.Compact = dto.Compact;
					noteItem.Topmost = dto.Topmost;
					noteItem.Locked = dto.Locked;
					
					noteItem.Location = new Point(dto.X, dto.Y);
					noteItem.Size = new Size(dto.Width, dto.Height);
					
					noteItem.Style.ForeColor = dto.ForeColor;
					noteItem.Style.BackColor = dto.BackColor;
					if(!string.IsNullOrWhiteSpace(dto.FontFamily) && dto.FontHeight > 0) {
						noteItem.Style.FontSetting.Family = dto.FontFamily; 
						noteItem.Style.FontSetting.Height = dto.FontHeight; 
						noteItem.Style.FontSetting.Bold = dto.FontBold; 
						noteItem.Style.FontSetting.Italic = dto.FontItalic; 
					}
					
					result.Add(noteItem);
				}
				
				return result;
			} else {
				return new NoteItem[] {};
			}
		}
		
		/// <summary>
		/// TODO: Dataから別のどこかへ委譲。
		/// </summary>
		/// <param name="noteItem"></param>
		/// <returns></returns>
		public NoteItem InsertItem(NoteItem noteItem)
		{
			lock(this._db) {
				using(var tran = this._db.BeginTransaction()) {
					var noteDto = this._db.GetTableId("M_NOTE", "NOTE_ID");
					noteItem.NoteId = noteDto.MaxId + 1;
					var mNote = new MNoteEntity();
					mNote.Id = noteItem.NoteId;
					mNote.RawType = NoteTypeUtility.ToNumber(NoteType.Text);
					mNote.Title = noteItem.Title;
					mNote.CommonCreate = mNote.CommonUpdate = DateTime.Now;
					mNote.CommonEnabled = true;
					this._db.ExecuteInsert(new [] { mNote });
					tran.Commit();
					return noteItem;
				}
			}
		}

		
		/// <summary>
		/// TODO: Dataから別のどこかへ委譲。
		/// </summary>
		/// <param name="noteItem"></param>
		public void ResistItem(NoteItem noteItem, ILogger logger)
		{
			lock(this._db) {
				var noteDB = new NoteDB(this._db);
				using(var tran = noteDB.BeginTransaction()) {
					try {
						noteDB.Resist(new [] { noteItem });
						tran.Commit();
					} catch(Exception ex) {
						tran.Rollback();
						logger.Puts(LogType.Error, ex.Message, ex);
					}
				}
			}
		}
		
		public void Dispose()
		{
			CaptionFontSetting.Dispose();
		}
		
	}
}
