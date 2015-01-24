namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public partial class ScreenForm: Form, ISetLanguage, ISetSkin
	{
		#region define
		#endregion ////////////////////////////////////

		#region static
		#endregion ////////////////////////////////////

		#region variable

		Screen _screen;

		#endregion ////////////////////////////////////

		#region event
		#endregion ////////////////////////////////////

		public ScreenForm()
		{
			InitializeComponent();

			Initialize();
		}

		#region property

		Language Language { get; set; }
		ISkin Skin { get; set; }

		public Screen Screen 
		{
			get { return this._screen; }
			set 
			{
				this._screen = value;
				ChangedScreen(value);
			}
		}

		#endregion ////////////////////////////////////

		#region override
		#endregion ////////////////////////////////////

		#region initialize

		void Initialize()
		{ }

		#endregion ////////////////////////////////////

		#region language
		#endregion ////////////////////////////////////

		#region function

		public void SetLanguage(Language language)
		{
			Language = language;
		}

		public void SetSkin(ISkin skin)
		{
			Skin = skin;
		}

		void ChangedScreen(Screen screen)
		{
			var rect = screen.Bounds;
			SetBounds(rect.X, rect.Y, rect.Width, rect.Height);

			var map = new Dictionary<string, string>();
			if(Screen != null) {
				map[AppLanguageName.screen] = ScreenUtility.GetScreenName(Screen, new NullLogger());
			}

			UIUtility.SetDefaultText(this, Language, map.Count > 0 ? map : null);
		}

		#endregion ////////////////////////////////////
	}
}
