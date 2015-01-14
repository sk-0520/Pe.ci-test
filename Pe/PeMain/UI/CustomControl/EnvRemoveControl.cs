using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	///環境変数削除用コントロール。
	/// </summary>
	public partial class EnvRemoveControl: UserControl, ISetLanguage, ISetSkin
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable
		Language _language;
		ISkin _skin;
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

		#region ISetSkin

		public void SetSkin(ISkin skin)
		{
			ApplySkin(skin);
			this._skin = skin;
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

		#region skin
		void ApplySkin(ISkin skin)
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
