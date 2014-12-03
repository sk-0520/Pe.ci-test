/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/05
 * 時刻: 2:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ContentTypeTextNet.Pe.Application.Data;
using ContentTypeTextNet.Pe.Application.IF;
using ContentTypeTextNet.Pe.Application.Logic;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.Library.PInvoke.Windows;

namespace ContentTypeTextNet.Pe.Application.UI
{
	/// <summary>
	/// Description of ToolbarForm_define.
	/// </summary>
	partial class ToolbarForm
	{
		const string menuNameMainPosDesktopFloat = "desktop_float";
		const string menuNameMainPosDesktopTop = "desktop_top";
		const string menuNameMainPosDesktopBottom = "desktop_bottom";
		const string menuNameMainPosDesktopLeft = "desktop_left";
		const string menuNameMainPosDesktopRight = "desktop_right";
		const string menuNameMainTopmost = "topmost";
		const string menuNameMainAutoHide = "autohide";
		const string menuNameMainGroupSeparator = "group_sep";
		const string menuNameMainGroupItem = "group_item_";
			
		const string menuNameExecute = "exec";
		const string menuNameExecuteEx = "ex";
		const string menuNamePath = "path";
		const string menuNameFiles = "ls";
		
		const string menuNamePath_openParentDir = "open_parent_dir";
		const string menuNamePath_openWorkDir = "open_work_dir";
		const string menuNamePath_copyCommand = "copy_command";
		const string menuNamePath_copyParentDir = "copy_parrent_dir";
		const string menuNamePath_copyWorkDir = "copy_work_dir";
		const string menuNamePath_property = "property";
		
		enum DropType
		{
			None,
			Files,
			Button
		}
		
		struct DropData
		{
			public DropType DropType { get; set; }
			public ToolStripItem ToolStripItem { get; set; }
			public LauncherItem LauncherItem  { get; set; }
			public IEnumerable<string> Files { get; set; }
			public ToolStripItem SrcToolStripItem { get; set; }
		}

		/// <summary>
		/// TODO: スキンと内部描画が入り混じっている。描画処理は整理出来たら全部スキンに回す。
		/// </summary>
		public partial class CustomToolTipForm: Form, ISetCommonData
		{
			CommonData CommonData { get; set; }

			FontSetting TitleFontSetting { get; set; }
			FontSetting MessageFontSetting { get; set; }
			IconScale IconScale { get; set; }
			Size TipPadding { get; set; }

			string _title, _message;
			Image _imageIcon;

			float _titleHeight;

			//enum FadeState
			//{
			//	None,
			//	In,
			//	Out,
			//}
			//FadeState _fadeState;

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

			void ApplyLanguage()
			{ }

			void ApplySetting()
			{
				ApplyLanguage();
			}

			public void SetCommonData(CommonData commonData)
			{
				CommonData = commonData;

				ApplySetting();
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
						e.Graphics.FillEllipse(SystemBrushes.InfoText, e.ClipRectangle);
					} else {
						CommonData.Skin.DrawToolbarToolTipBackground(e.Graphics, e.ClipRectangle);
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
				var messageArea = new Rectangle(Padding.Left, titleArea.Bottom, Width -Padding.Horizontal, Height - titleArea.Height - Padding.Vertical);
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
					this._imageIcon = launcherItem.GetIcon(IconScale.Normal, launcherItem.IconItem.Index).ToBitmap();
					this._title = launcherItem.Name;
					this._message = launcherItem.Note;
				} else {
					this._imageIcon = AppUtility.GetAppIcon(IconScale.Normal);
					this._title = CommonData.Language["toolbar/main/tips", new Dictionary<string, string>() {{AppLanguageName.groupName, groupItem.Name}}];
					this._message = string.Empty;
				}

				// 描画サイズ生成
				using(var g = CreateGraphics()) 
				using(var titleFormat = CreateTitleFormat())
				using(var messageFormat = CreateTitleFormat()) {
					var maxShowSize = new Size(screen.WorkingArea.Size.Width / 2, screen.WorkingArea.Size.Height / 2);
					var titleSize = g.MeasureString(this._title, TitleFontSetting.Font, maxShowSize, titleFormat);
					var messageSize = HasMessage() ? g.MeasureString(this._message, MessageFontSetting.Font, maxShowSize, messageFormat) : SizeF.Empty;

					this._titleHeight = Math.Max((float)IconScale.ToHeight(), titleSize.Height) + (float)(HasMessage() ? Padding.Top: 0);

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

					case ToolbarPosition.DesktopBottom: LABEL_TOP:
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
						Debug.Assert(false, toolbarItem.ToolbarPosition.ToString());
						throw new NotImplementedException();
				}

				ToShow();
			}
		}

	}
}

