/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/31
 * 時刻: 14:48
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_define.
	/// </summary>
	public partial class SettingForm
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
			
			public bool Remove { get; set; }
			public bool NewItem { get; set; }
			public NoteItem NoteItem { get; set; }
			
		}
	}
}
