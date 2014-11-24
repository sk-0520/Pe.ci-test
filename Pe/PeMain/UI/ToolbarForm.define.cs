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
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;
using PeSkin;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
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

			public CustomToolTipForm()
			{
				FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				ShowInTaskbar = false;
				TopMost = true;
				Padding = new Padding(3);

				BackColor = SystemColors.Info;
				ForeColor = SystemColors.InfoText;

				TipPadding = new Size(4, 4);
				TitleFontSetting = new FontSetting(SystemFonts.MessageBoxFont);
				MessageFontSetting = new FontSetting(SystemFonts.SmallCaptionFont);
				IconScale = IconScale.Normal;
			}

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
				if(CommonData.Skin.IsDefaultDrawToolbarToolTipBackground) {
					base.OnPaintBackground(e);
					e.Graphics.FillEllipse(SystemBrushes.InfoText, e.ClipRectangle);
				} else {
					CommonData.Skin.DrawToolbarToolTipBackground(e.Graphics, e.ClipRectangle);
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
				Visible = true;
			}

			void ToHide()
			{
				Visible = false;
			}

			public void HideItem()
			{
				Debug.WriteLine(string.Format("{0}, {1}", DateTime.Now, "HIDE"));
				ToHide();
			}

			public void ShowItem(Screen screen, ToolStripItem toolStripItem, ToolbarGroupItem groupItem, ToolbarItem toolbarItem)
			{
				Debug.Assert(toolStripItem != null);
				Debug.Assert(CommonData != null);

				var launcherItem = toolStripItem.Tag as LauncherItem;
				Debug.WriteLine(string.Format("{0}, {1} - {2} - {3}", DateTime.Now, toolbarItem, groupItem, launcherItem));
				//Show(toolStripItem.Text, toolStripItem.Owner);
				if(launcherItem != null) {
					this._imageIcon = launcherItem.GetIcon(IconScale.Normal, launcherItem.IconItem.Index).ToBitmap();
					this._title = launcherItem.Name;
					this._message = launcherItem.Note;
				} else {
					this._imageIcon = AppUtility.GetAppIcon(IconScale.Normal);
					this._title = groupItem.Name;
					this._message = string.Empty;
				}

				// 描画サイズ生成
				using(var g = CreateGraphics()) 
					using(var titleFormat = CreateTitleFormat()) {
					var titleSize = g.MeasureString(this._title, TitleFontSetting.Font, screen.WorkingArea.Size, titleFormat);
					var messageSize = HasMessage() ? g.MeasureString(this._message, MessageFontSetting.Font) : SizeF.Empty;

					this._titleHeight = Math.Max((float)IconScale.ToHeight(), titleSize.Height) + (float)(HasMessage() ? Padding.Top: 0);

					Size = new Size(
						(int)(Math.Max(titleSize.Width, messageSize.Width) + IconScale.Normal.ToWidth() + Padding.Left) + Padding.Horizontal,
						(int)(Math.Max(IconScale.ToHeight(), titleSize.Height) + messageSize.Height) + Padding.Vertical
					);
				}
				//Region = System.Drawing.Region.FromHrgn(NativeMethods.CreateRoundRectRgn(0, 0, Width, Height, 2, 2));
				CommonData.Skin.ApplyToolbarToolTipRegion(this);

				// 表示位置設定
				var itemArea = toolStripItem.Bounds;
				var screenPoint = toolStripItem.Owner.PointToScreen(itemArea.Location);
				switch(toolbarItem.ToolbarPosition) {
					case ToolbarPosition.DesktopFloat: 
					case ToolbarPosition.DesktopTop:
						// 下に表示
						Location = new Point(screenPoint.X + TipPadding.Width, screenPoint.Y + itemArea.Height + TipPadding.Height);
						if(toolbarItem.ToolbarPosition == ToolbarPosition.DesktopFloat) {
							if(Location.Y + Size.Height > screen.WorkingArea.Height) {
								goto _TOP_;
							}
						}
						break;

					case ToolbarPosition.DesktopBottom: _TOP_:
						// 上に表示
						Location = new Point(screenPoint.X + TipPadding.Width, screenPoint.Y - Height - TipPadding.Height);
						break;

					case ToolbarPosition.DesktopLeft:
						// 右に表示
						Location = new Point(screenPoint.X + itemArea.Width - TipPadding.Width, screenPoint.Y + TipPadding.Height);
						break;

					case ToolbarPosition.DesktopRight:
						// 左に表示
						Location = new Point(screenPoint.X - itemArea.Width - TipPadding.Width, screenPoint.Y + TipPadding.Height);
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

