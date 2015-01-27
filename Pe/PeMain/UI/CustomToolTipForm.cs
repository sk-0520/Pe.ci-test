namespace ContentTypeTextNet.Pe.PeMain.UI
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using ContentTypeTextNet.Pe.PeMain.UI.Ex;

	/// <summary>
	/// TODO: スキンと内部描画が入り混じっている。描画処理は整理出来たら全部スキンに回す。
	/// </summary>
	public partial class CustomToolTipForm: CommonForm
	{
		//CommonData CommonData { get; set; }

		FontSetting TitleFontSetting { get; set; }
		FontSetting MessageFontSetting { get; set; }
		IconScale IconScale { get; set; }
		Size TipPadding { get; set; }

		string _title, _message;
		Image _imageIcon;

		float _titleHeight;

		public CustomToolTipForm()
		{
			//Opacity = 0;
			Visible = false;
			FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			ShowInTaskbar = false;
			TopMost = true;
			Padding = new Padding(3);

			BackColor = SystemColors.Info;
			ForeColor = SystemColors.InfoText;

			//this._fadeState = FadeState.None;

			TipPadding = new Size(4, 4);
			TitleFontSetting = new FontSetting(SystemFonts.MessageBoxFont);
			MessageFontSetting = new FontSetting(SystemFonts.SmallCaptionFont);
			IconScale = IconScale.Normal;
		}

		protected override void Dispose(bool disposing)
		{
			_imageIcon.ToDispose();

			base.Dispose(disposing);
		}

		protected override bool ShowWithoutActivation { get { return true; } }

		protected override CreateParams CreateParams
		{
			get
			{
				var result = base.CreateParams;

				result.ExStyle |= (int)(WS_EX.WS_EX_NOACTIVATE | WS_EX.WS_EX_TOOLWINDOW);
				result.ClassStyle |= (int)CS.CS_DROPSHADOW;

				return result;
			}
		}

		protected override void ApplyLanguage()
		{
			base.ApplyLanguage();
		}

		protected override void ApplySetting()
		{
			base.ApplySetting();
		}

		bool HasMessage()
		{
			return !string.IsNullOrWhiteSpace(this._message);
		}

		StringFormat CreateTitleFormat()
		{
			var sf = new StringFormat();

			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Center;
			sf.Trimming = StringTrimming.EllipsisCharacter;

			return sf;
		}
		StringFormat CreateMessageFormat()
		{
			var sf = new StringFormat();

			sf.Alignment = StringAlignment.Near;
			sf.LineAlignment = StringAlignment.Near;
			sf.Trimming = StringTrimming.EllipsisCharacter;

			return sf;
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			if(CommonData != null && CommonData.Skin != null) {
				if(CommonData.Skin.IsDefaultDrawToolbarToolTipBackground) {
					base.OnPaintBackground(e);
					//e.Graphics.FillEllipse(SystemBrushes.InfoText, e.ClipRectangle);
				} else {
					CommonData.Skin.DrawToolTipBackground(e.Graphics, e.ClipRectangle);
				}
			} else {
				base.OnPaintBackground(e);
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var iconTop = this._titleHeight / 2 - IconScale.ToHeight() / 2;
			e.Graphics.DrawImage(this._imageIcon, new Point(Padding.Left, Padding.Top + (int)iconTop));
			var iconWidth = Padding.Left + IconScale.ToWidth() + Padding.Left;
			var titleArea = new Rectangle(iconWidth, Padding.Top, Width - iconWidth, (int)this._titleHeight);
			var messageArea = new Rectangle(Padding.Left, titleArea.Bottom, Width - Padding.Horizontal, Height - titleArea.Height - Padding.Vertical);
			using(var sf = CreateTitleFormat())
			using(var brush = new SolidBrush(Color.Black)) {
				e.Graphics.DrawString(this._title, TitleFontSetting.Font, brush, titleArea, sf);
			}
			if(HasMessage()) {
				using(var sf = CreateTitleFormat())
				using(var brush = new SolidBrush(Color.Black)) {
					e.Graphics.DrawString(this._message, MessageFontSetting.Font, brush, messageArea, sf);
				}
			}
		}

		void ToShow()
		{
			WindowsUtility.ShowNoActive(this);
		}

		void ToHide()
		{
			Visible = false;
		}

		public void HideItem()
		{
			ToHide();
		}

		public void ShowItem(Screen screen, ToolStripItem toolStripItem, ToolbarGroupItem groupItem, ToolbarItem toolbarItem)
		{
			Debug.Assert(toolStripItem != null);
			Debug.Assert(CommonData != null);

			var launcherItem = toolStripItem.Tag as LauncherItem;

			if(launcherItem != null) {
				this._imageIcon = launcherItem.GetIcon(IconScale.Normal, launcherItem.IconItem.Index, CommonData.ApplicationSetting).ToBitmap();
				this._title = launcherItem.Name;
				if(launcherItem.LauncherType == LauncherType.Embedded) {
					var applicationItem = CommonData.ApplicationSetting.GetApplicationItem(launcherItem);
					this._message = LanguageUtility.ApplicationItemToComment(CommonData.Language, applicationItem);
				} else {
					this._message = launcherItem.Note;
				}
			} else {
				this._imageIcon = IconUtility.ImageFromIcon(CommonData.Skin.GetIcon(SkinIcon.App), IconScale.Normal);
				this._title = CommonData.Language["toolbar/main/tips", new Dictionary<string, string>() { { AppLanguageName.groupName, groupItem.Name } }];
				this._message = string.Empty;
			}

			// 描画サイズ生成
			using(var g = CreateGraphics())
			using(var titleFormat = CreateTitleFormat())
			using(var messageFormat = CreateTitleFormat()) {
				var maxShowSize = new Size(screen.WorkingArea.Size.Width / 2, screen.WorkingArea.Size.Height / 2);
				var titleSize = g.MeasureString(this._title, TitleFontSetting.Font, maxShowSize, titleFormat);
				var messageSize = HasMessage() ? g.MeasureString(this._message, MessageFontSetting.Font, maxShowSize, messageFormat) : SizeF.Empty;

				this._titleHeight = Math.Max((float)IconScale.ToHeight(), titleSize.Height) + (float)(HasMessage() ? Padding.Top : 0);

				Size = new Size(
					(int)(Math.Max(titleSize.Width, messageSize.Width) + IconScale.Normal.ToWidth() + Padding.Left) + Padding.Horizontal,
					(int)(Math.Max(IconScale.ToHeight(), titleSize.Height) + messageSize.Height) + Padding.Vertical
				);
			}

			CommonData.Skin.ApplyToolbarToolTipRegion(this);

			// 表示位置設定
			var itemArea = toolStripItem.Bounds;
			if(toolStripItem.OwnerItem != null) {
				var ownerItemLocation = toolStripItem.OwnerItem.Bounds.Location;
				itemArea.Offset(ownerItemLocation);
			}
			var screenPoint = toolStripItem.Owner.PointToScreen(itemArea.Location);
			switch(toolbarItem.ToolbarPosition) {
				case ToolbarPosition.DesktopFloat:
				case ToolbarPosition.DesktopTop:
					// 下に表示
					Location = new Point(screenPoint.X, screenPoint.Y + itemArea.Height + TipPadding.Height);
					if(toolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
						if(Location.Y + Size.Height > screen.WorkingArea.Height) {
							goto LABEL_TOP;
						}
					}
					break;

				case ToolbarPosition.DesktopBottom:
				LABEL_TOP:
					// 上に表示
					Location = new Point(screenPoint.X, screenPoint.Y - Height - TipPadding.Height);
					break;

				case ToolbarPosition.DesktopLeft:
					// 右に表示
					Location = new Point(screenPoint.X + itemArea.Width + TipPadding.Width, screenPoint.Y);
					break;

				case ToolbarPosition.DesktopRight:
					// 左に表示
					Location = new Point(screenPoint.X - Width - TipPadding.Width, screenPoint.Y);
					break;

				default:
					throw new NotImplementedException();
			}

			ToShow();
		}
	}
}
