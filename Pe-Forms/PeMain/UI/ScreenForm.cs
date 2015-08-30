namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Data;
	using System.Diagnostics;
	using System.Drawing;
	using System.Text;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Logic;

	public partial class ScreenForm: Form, ISetLanguage, ISetSkin
	{
		#region define

		struct ScreenInfo
		{
			public static string CloseMessage { get; set; }

			public string ScreenName { get; set; }
			public string DeviceName { get; set; }
			public string Info { get; set; }

			public override string ToString()
			{
				return string.Join(
					Environment.NewLine,
					new [] { 
						ScreenName,
						string.Empty,
						DeviceName,
						Info,
						string.Empty,
						CloseMessage,
					}
				);
			}
		}

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

		ScreenInfo Info { get; set; }

		#endregion ////////////////////////////////////

		#region override

		protected override void OnPaintBackground(PaintEventArgs e)
		{ }

		protected override void OnPaint(PaintEventArgs e)
		{
			if(Screen == null || Language == null) {
				base.OnPaint(e);
			} else {
				using(var image = new Bitmap(Width, Height, e.Graphics)) {
					using(var g = Graphics.FromImage(image)) {
						g.Clear(BackColor);
						using(var sf = new StringFormat()) {
							sf.Alignment = StringAlignment.Center;
							sf.LineAlignment = StringAlignment.Center;
							using(var fontBrush = new SolidBrush(ForeColor)) {
								g.DrawString(Info.ToString(), Font, fontBrush, ClientRectangle, sf);
							}
						}
					}
					e.Graphics.DrawImage(image, 0, 0);
				}
			}
		}

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
			ScreenInfo.CloseMessage = Language["screen/close"];
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
				map[ProgramLanguageName.screen] = ScreenUtility.GetScreenName(Screen);
			}

			UIUtility.SetDefaultText(this, Language, map.Count > 0 ? map : null);
			Info = GetScreenInfo();

			if(Screen.Primary) {
				BackColor = Color.White;
			} else {
				Random rand = new Random(Screen.GetHashCode());
				BackColor = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
			}
			ForeColor = DrawUtility.CalcAutoColor(BackColor);
		}

		ScreenInfo GetScreenInfo()
		{
			Debug.Assert(Screen != null);

			var result = new ScreenInfo();

			result.ScreenName = ScreenUtility.GetScreenName(Screen);
			result.DeviceName = Screen.DeviceName;
			if(Screen.Primary) {
				result.Info = Language["screen/primary/true"];
			} else {
				result.Info = Language["screen/primary/false"];
			}

			return result;
		}

		#endregion ////////////////////////////////////
	}
}
