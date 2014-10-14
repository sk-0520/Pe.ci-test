/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/29
 * 時刻: 22:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using PeMain.IF;
using PeMain.Logic;

namespace PeMain.UI
{
	partial class App
	{
		/// <summary>
		/// メッセージ受信用。
		/// </summary>
		public partial class MessageWindow : Form, ISetCommonData
		{
			private App _parent;
			
			public MessageWindow(App parent)
			{
				//
				// The InitializeComponent() call is required for Windows Forms designer support.
				//
				InitializeComponent();
				
				this._parent = parent;
			}
		}
	}
}
