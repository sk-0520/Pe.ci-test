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
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of NoteForm_define.
	/// </summary>
	partial class NoteForm
	{
		const int RECURSIVE = 2;
		static readonly Size menuIconSize = IconScale.Small.ToSize();
		
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

		class ColorMenuItem
		{
			public ColorMenuItem(ToolStripItem item, Color color)
			{
				Item = item;
				Color = color;
			}
			
			public ToolStripItem Item { get; private set; }
			public Color Color { get; private set; }
		}

		struct HitState
		{
			private const uint leftBit = 0x0001;
			private const uint rightBit = 0x0002;
			private const uint topBit = 0x0004;
			private const uint bottomBit = 0x0008;

			private uint _flag;

			public bool HasTrue { get { return this._flag != 0; } }

			private bool Get(uint bit)
			{
				return (this._flag & bit) == bit;
			}
			private void Set(uint bit, bool value)
			{
				if(value) {
					this._flag |= bit;
				} else {
					this._flag &= ~(this._flag & bit);
				}
			}

			public bool Left
			{
				get { return Get(leftBit); }
				set { Set(leftBit, value); }
			}
			public bool Right
			{
				get { return Get(rightBit); }
				set { Set(rightBit, value); }
			}
			public bool Top
			{
				get { return Get(topBit); }
				set { Set(topBit, value); }
			}
			public bool Bottom
			{
				get { return Get(bottomBit); }
				set { Set(bottomBit, value); }
			}
		}
	}
}
