/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/15
 * 時刻: 22:28
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	///環境変数削除用コントロール。
	/// </summary>
	public partial class EnvRemoveControl : UserControl, ISetLanguage
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		Language _language;
		bool _event = false;
		#endregion ////////////////////////////////////

		#region event
		public event EventHandler<EventArgs> ValueChanged;
		#endregion ////////////////////////////////////

		public EnvRemoveControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}

		#region property
		public IEnumerable<string> Items
		{
			get
			{
				return this.inputEnv.Lines.Where(s => !string.IsNullOrWhiteSpace(s));
			}
		}
		#endregion ////////////////////////////////////

		#region ISetCommonData
		#endregion ////////////////////////////////////

		#region ISetLanguage
		public void SetLanguage(Language language)
		{
			ApplyLanguage(language);
			this._language = language;
		}
		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize
		void Initialize()
		{
			this._event = true;
		}
		#endregion ////////////////////////////////////

		#region language
		void ApplyLanguage(Language language)
		{

		}
		#endregion ////////////////////////////////////

		#region function
		public void SetItem(IList<string> items)
		{
			this._event = false;
			try {
				var lines = string.Join(Environment.NewLine, items.Where(s => !string.IsNullOrEmpty(s)));
				this.inputEnv.Text = lines;
			} finally {
				this._event = true;
			}
		}

		public void Clear()
		{
			this.inputEnv.Clear();
		}
		#endregion ////////////////////////////////////

		void InputEnv_TextChanged(object sender, EventArgs e)
		{
			if(this._event && ValueChanged != null) {
				ValueChanged(this, new EventArgs());
			}
		}
	}
}
