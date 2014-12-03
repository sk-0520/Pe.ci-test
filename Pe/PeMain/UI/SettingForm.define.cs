/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/31
 * 時刻: 14:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using ContentTypeTextNet.Pe.Application.Data;

namespace ContentTypeTextNet.Pe.Application.UI
{
	/// <summary>
	/// Description of SettingForm_define.
	/// </summary>
	partial class SettingForm
	{
		const int TREE_LEVEL_GROUP = 0;
		const int TREE_LEVEL_ITEM = 1;
		
		const int TREE_TYPE_NONE = 0;
		const int TREE_TYPE_GROUP = 1;
		
		class NoteWrapItem
		{
			public NoteWrapItem(NoteItem item)
			{
				Remove = false;
				NewItem = false;
				
				NoteItem = item;
			}
			
			public NoteItem NoteItem { get; set; }
			
			public bool NewItem { get; set; }
			
#region property name
			public bool Remove { get; set; }
			public long Id 
			{ 
				get { return NoteItem.NoteId; }
				set { NoteItem.NoteId = value; }
			}
			public bool Visible
			{
				get { return NoteItem.Visible; }
				set { NoteItem.Visible = value; }
			}
			public bool Locked
			{
				get { return NoteItem.Locked; }
				set { NoteItem.Locked = value; }
			}
			public string Title
			{ 
				get { return NoteItem.Title; }
				set { NoteItem.Title = value; }
			}
			public string Body
			{
				get { return NoteItem.Body; }
				set { NoteItem.Body = value; }
			}
			public FontSetting Font
			{
				get { return NoteItem.Style.FontSetting; }
				set { NoteItem.Style.FontSetting = value; }
			}
			public Color Fore
			{
				get { return NoteItem.Style.ForeColor; }
				set { NoteItem.Style.ForeColor = value; }
			}
			public Color Back
			{
				get { return NoteItem.Style.BackColor; }
				set { NoteItem.Style.BackColor = value; }
			}
#endregion
		}
	}
}
