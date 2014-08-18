/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/03
 * 時刻: 22:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.ComponentModel;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of NoteForm_define.
	/// </summary>
	partial class NoteForm
	{
		const int RECURSIVE = 2;
		
		private class NoteBindItem: INotifyPropertyChanged
		{
			public event PropertyChangedEventHandler PropertyChanged;
			
			public NoteBindItem(NoteItem item)
			{
				NoteItem = item;
			}
			
			public NoteItem NoteItem { get; private set; }
			
			public string Title
			{
				get
				{
					return NoteItem.Title;
				}
				set
				{
					NoteItem.Title = value;
					NotifyPropertyChanged("Title");
				}
			}
			
			public string Body
			{
				get
				{
					return NoteItem.Body;
				}
				set
				{
					NoteItem.Body = value;
					NotifyPropertyChanged("Body");
				}
			}
			
			protected void NotifyPropertyChanged(String s)
			{
				if(PropertyChanged != null)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(s));
				}
			}
		}
	}
}
