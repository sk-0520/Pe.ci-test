/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/08
 * 時刻: 21:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using PeMain.Data;
using PeMain.Data.DB;
using PeUtility;

namespace PeMain.Logic.DB
{
	/// <summary>
	/// Description of Note.
	/// </summary>
	public class NoteDB: DBWrapper
	{
		public NoteDB(PeDBManager db): base(db)
		{ }
		
		/// <summary>
		/// TODO: Dataから別のどこかへ委譲。
		/// </summary>
		/// <param name="enabledOnly"></param>
		/// <returns></returns>
		public IEnumerable<NoteItem> GetNoteItemList(bool enabledOnly)
		{
			var dtoList = this.db.GetResultList<NoteItemDto>(global::PeMain.Properties.SQL.GetNoteItemList);
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
		
		
		public void ToDisabled(IEnumerable<NoteItem> noteItemList)
		{
			if(noteItemList == null || noteItemList.Count() == 0) {
				return;
			}
			
			this.db.Parameter["enabled"] = false;
			this.db.Parameter["update"] = DateTime.Now;
			var idList = new List<string>(noteItemList.Count());
			var index = 1;
			foreach(var note in noteItemList) {
				var key = string.Format("id_{0}", index++);
				var item = string.Format("NOTE_ID = :{0}", key);
				this.db.Parameter[key] = note.NoteId;
				idList.Add(item);
			}
			this.db.Expression["ID_LIST"] = new CommandExpression(string.Join(" or ", idList));
			this.db.ExecuteCommand(global::PeMain.Properties.SQL.EnabledSwitch);
		}
		
		public void ResistMasterNote(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || noteItemList.Count() == 0) {
				return;
			}
			
			var updateList = new List<MNoteEntity>();
			var insertList = new List<MNoteEntity>();
			
			foreach(var item in noteItemList) {
				var entity = new MNoteEntity();
				entity.Id = item.NoteId;
				var tempEntity = this.db.GetEntity(entity);
				var isUpdate = tempEntity != null;
				if(isUpdate) {
					entity = tempEntity;
				} else {
					entity.CommonCreate = timestamp;
					entity.CommonEnabled = true;
				}
				entity.CommonUpdate = timestamp;
				
				entity.Title = item.Title;
				entity.RawType = NoteTypeUtility.ToNumber(NoteType.Text);
				entity.Title = item.Title;
				
				if(isUpdate) {
					updateList.Add(entity);
				} else {
					insertList.Add(entity);
				}
			}
			
			if(updateList.Count > 0) {
				this.db.ExecuteUpdate(updateList);
			}
			if(insertList.Count > 0) {
				this.db.ExecuteInsert(insertList);
			}
		}

		public void ResistTransactionNote(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || noteItemList.Count() == 0) {
				return;
			}
			
			var updateList = new List<TNoteEntity>();
			var insertList = new List<TNoteEntity>();
			
			foreach(var item in noteItemList) {
				var entity = new TNoteEntity();
				entity.Id = item.NoteId;
				var tempEntity = this.db.GetEntity(entity);
				var isUpdate = tempEntity != null;
				if(isUpdate) {
					entity = tempEntity;
				} else {
					entity.CommonCreate = timestamp;
				}
				entity.CommonUpdate = timestamp;
				entity.Body = item.Body;
				
				if(isUpdate) {
					updateList.Add(entity);
				} else {
					insertList.Add(entity);
				}
			}
			
			if(updateList.Count > 0) {
				this.db.ExecuteUpdate(updateList);
			}
			if(insertList.Count > 0) {
				this.db.ExecuteInsert(insertList);
			}
		}
		
		public void ResistTransactionNoteStyle(IEnumerable<NoteItem> noteItemList, DateTime timestamp)
		{
			if(noteItemList == null || noteItemList.Count() == 0) {
				return;
			}
			
			var updateList = new List<TNoteStyleEntity>();
			var insertList = new List<TNoteStyleEntity>();
			
			foreach(var item in noteItemList) {
				var entity = new TNoteStyleEntity();
				entity.Id = item.NoteId;
				var tempEntity = this.db.GetEntity(entity);
				var isUpdate = tempEntity != null;
				if(isUpdate) {
					entity = tempEntity;
				} else {
					entity.CommonCreate = timestamp;
				}
				entity.CommonUpdate = timestamp;
				
				entity.ForeColor = item.Style.ForeColor;
				entity.BackColor = item.Style.BackColor;
				if(item.Style.FontSetting.IsDefault) {
					entity.FontFamily = string.Empty;
				} else {
					entity.FontFamily = item.Style.FontSetting.Family;
				}
				entity.FontHeight = item.Style.FontSetting.Height;
				entity.FontBold = item.Style.FontSetting.Bold;
				entity.FontItalic = item.Style.FontSetting.Italic;
				entity.Visibled = item.Visible;
				entity.Locked = item.Locked;
				entity.Topmost = item.Topmost;
				entity.Compact = item.Compact;
				entity.Location = item.Location;
				entity.Size = item.Size;
				
				if(isUpdate) {
					updateList.Add(entity);
				} else {
					insertList.Add(entity);
				}
			}
			
			if(updateList.Count > 0) {
				this.db.ExecuteUpdate(updateList);
			}
			if(insertList.Count > 0) {
				this.db.ExecuteInsert(insertList);
			}
		}
		
		public void Resist(IEnumerable<NoteItem> noteItemList)
		{
			var timestamp = DateTime.Now;
			ResistMasterNote(noteItemList, timestamp);
			ResistTransactionNote(noteItemList, timestamp);
			ResistTransactionNoteStyle(noteItemList, timestamp);
		}
		
		public NoteItem InsertMaster(NoteItem noteItem)
		{
			lock(this.db) {
				using(var tran = this.db.BeginTransaction()) {
					var noteDto = this.db.GetTableId("M_NOTE", "NOTE_ID");
					noteItem.NoteId = noteDto.MaxId + 1;
					ResistMasterNote(new [] { noteItem }, DateTime.Now);
					tran.Commit();
					return noteItem;
				}
			}
		}
		
	}
}
