namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public partial class ScreenForm: ContentTypeTextNet.Pe.PeMain.UI.CommonForm
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

		Screen Screen 
		{
			get { return this._screen; }
			set 
			{
				ChangeScreen(value);
				this._screen = value;
			}
		}

		#endregion ////////////////////////////////////

		#region ISetCommonData
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

		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();

			UIUtility.SetDefaultText(this, CommonData.Language);
		}

		void ChangeScreen(Screen screen)
		{
		}

		#endregion ////////////////////////////////////
	}
}
