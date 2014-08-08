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
		public NoteDB(DBManager db): base(db)
		{ }
		
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
			Debug.Assert(noteItemList != null && noteItemList.Count() > 0);
			
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
			Debug.Assert(noteItemList != null && noteItemList.Count() > 0);
			
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
			Debug.Assert(noteItemList != null && noteItemList.Count() > 0);
			
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
	}
}
