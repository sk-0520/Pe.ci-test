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
using System.Drawing;
using PeMain.Data;
using PeMain.Logic;

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
		
		class ColorData: ItemData<Color>, ISetLanguage
		{
			private string _displayTitle;
			private string _displayValue;
			Language Language { get; set; }
				
			public ColorData(Color value, string displayTitle, string displayValue): base(value)
			{
				this._displayTitle = displayTitle;
				this._displayValue = displayValue;
			}
			
			public override string Display {
				get 
				{
					var title = this._displayTitle;
					var value = this._displayValue;
					if(Language != null) {
						title = Language[title];
						value = Language[value];
					}
					return string.Format("{0}: {1}", title, value);
				}
			}
			
			public void SetLanguage(Language language)
			{
				Language = language;
			}
		}
	}
}
