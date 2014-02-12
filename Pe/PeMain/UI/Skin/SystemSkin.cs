/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/07
 * 時刻: 21:36
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using PeMain.Data;
using PeUtility;
using PI.Windows;

namespace PeMain.UI
{
	/// <summary>
	/// 標準で使用されるシステム環境にあってるっぽいスキン
	/// </summary>
	public class SystemSkin: Skin
	{
		private static Point GetPressOffset(ToolStripItem toolItem)
		{
			return Point.Empty;
			/*
			var splitItem = toolItem as ToolStripSplitButton;
			if(splitItem != null && splitItem.DropDownButtonPressed) {
				return Point.Empty;
			}
			
			if(toolItem.Pressed) {
				return new Point(1, 1);
			}
			return Point.Empty;
			 */
		}
		
		
		Color VisualColor { get; set;}
		
		private void SetVisualStyle(Form target)
		{
			Debug.Assert(EnabledVisualStyle);

			var blurHehind = new DWM_BLURBEHIND();
			blurHehind.fEnable = true;
			blurHehind.hRgnBlur = IntPtr.Zero;
			blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
			API.DwmEnableBlurBehindWindow(target.Handle, ref blurHehind);
			
			// 設定色を取得
			uint rawColor;
			bool blend;
			API.DwmGetColorizationColor(out rawColor, out blend);
			VisualColor = Color.FromArgb(Convert.ToInt32(rawColor));
		}
		
		public override void Start(Form target)
		{
			base.Start(target);
			//EnabledVisualStyle = false;
			if(EnabledVisualStyle) {
				SetVisualStyle(target);
			}
		}
		
		public override void Close(Form target)
		{
			if(EnabledVisualStyle) {
				var margin = new MARGINS();
				margin.leftWidth = 0;
				margin.rightWidth = 0;
				margin.topHeight = 0;
				margin.bottomHeight = 0;
				API.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
			}
		}
		
		public override Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition)
		{
			var frame = SystemInformation.Border3DSize;
			if(EnabledVisualStyle) {
				frame.Height = 0;
			}
			return new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
		}
		
		public override Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			var padding = GetToolbarWindowEdgePadding(toolbarPosition);
			var point = new Point(padding.Left, padding.Top);
			var size = new Size();
			
			if(ToolbarPositionUtility.IsHorizonMode(toolbarPosition)) {
				size.Width = SystemInformation.SmallCaptionButtonSize.Height / 2;
				size.Height = parentSize.Height - padding.Vertical;
			} else {
				size.Width = parentSize.Width - padding.Horizontal;
				size.Height = SystemInformation.SmallCaptionButtonSize.Height / 2;
			}
			
			return new Rectangle(point, size);
		}
		
		public override Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			var edgePadding = GetToolbarWindowEdgePadding(toolbarPosition);
			var captionArea = GetToolbarCaptionArea(toolbarPosition, parentSize);
			var captionPlus = new System.Drawing.Size();
			if(ToolbarPositionUtility.IsHorizonMode(toolbarPosition)) {
				captionPlus.Width = captionArea.Width;
			} else {
				captionPlus.Height =captionArea.Height;
			}
			var padding = new Padding(
				edgePadding.Left + captionPlus.Width,
				edgePadding.Top  + captionPlus.Height,
				edgePadding.Right,
				edgePadding.Bottom
			);
			
			return padding;
		}
		
		public override SkinToolbarButtonLayout GetToolbarButtonLayout(IconSize iconSize, bool showText, int textWidth)
		{
			var iconBox = iconSize.ToSize();
			var systemBorderSize = SystemInformation.Border3DSize;
			var systemPaddingSize = SystemInformation.FixedFrameBorderSize;
			var padding = new Padding(
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2,
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2
			);
			var buttonSize = new Size();
			var menuWidth = 12;
			
			buttonSize.Width = iconBox.Width + padding.Right + padding.Horizontal + menuWidth;
			if(showText) {
				buttonSize.Width += textWidth > 0 ? textWidth: Literal.toolbarTextWidth;
			}
			buttonSize.Height = iconBox.Height + padding.Vertical;
			
			var buttonLayout = new SkinToolbarButtonLayout();
			buttonLayout.Size = buttonSize;
			buttonLayout.Padding = padding;
			buttonLayout.MenuWidth = menuWidth;
			return buttonLayout;
		}

		public override void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position)
		{
			g.Clear(VisualColor);
		}
		
		public override void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolPosition)
		{
			var edgePadding = GetToolbarWindowEdgePadding(toolPosition);
			Color startColor = Color.FromArgb(150, Color.White);
			Color endColor = Color.FromArgb(70, Color.White);
			
			var leftArea = new Rectangle(drawArea.Location, new Size(edgePadding.Left, drawArea.Height));
			var rightArea = new Rectangle(new Point(drawArea.Width - edgePadding.Right), new Size(edgePadding.Right, drawArea.Height));
			g.SmoothingMode = SmoothingMode.AntiAlias;
			using(var brush = new LinearGradientBrush(leftArea, startColor, endColor, LinearGradientMode.ForwardDiagonal)) {
				g.FillRectangle(brush, leftArea);
			}
			using(var brush = new LinearGradientBrush(rightArea, endColor, startColor, LinearGradientMode.BackwardDiagonal)) {
				g.FillRectangle(brush, rightArea);
			}
		}

		
		public override void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
			var prevSmoothingMode = SmoothingMode.AntiAlias;
			try {
				g.SmoothingMode = SmoothingMode.AntiAlias;
				var dotSize = new Size(3, 3);
				using(var image = new Bitmap(dotSize.Width + 1, dotSize.Height + 1, PixelFormat.Format32bppArgb)) {
					using(var graphics = Graphics.FromImage(image)) {
						var dotArea = new Rectangle(Point.Empty, new Size(dotSize.Width, dotSize.Height));
						var startColor = Color.FromArgb(192, Color.White);
						var endColor = Color.FromArgb(128, Color.Black);
						using(var brush = new LinearGradientBrush(dotArea, startColor, endColor, LinearGradientMode.ForwardDiagonal)) {
							graphics.Clear(Color.Transparent);
							graphics.FillRectangle(brush, dotArea);
						}
					}
					
					using(var brush = new TextureBrush(image, WrapMode.Tile)) {
						g.FillRectangle(brush, drawArea);
					}
					
				}
			} finally {
				g.SmoothingMode = prevSmoothingMode;
			}
		}
		
		
		public override void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position)
		{
			e.Graphics.Clear(VisualColor);
		}
		
		public override void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position)
		{
		}
		
		
		public override void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem)
		{
			var offset = GetPressOffset(e.Item);
			
			var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconSize, false, 0);
			var iconSize = toolbarItem.IconSize.ToSize();
			e.Graphics.DrawImage(e.Image, buttonLayout.Padding.Left + offset.X, buttonLayout.Padding.Top + offset.Y, iconSize.Width, iconSize.Height);
		}
		
		public override void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem)
		{
			var offset = GetPressOffset(e.Item);
			
			using(var textBrush = new SolidBrush(Color.FromArgb(250, Color.White))) {
				using(var shadowBrush = new SolidBrush(Color.FromArgb(255, Color.DarkGray))) {
					using(var format = ToStringFormat(e.TextFormat)) {
						format.LineAlignment = StringAlignment.Center;
						format.Trimming = StringTrimming.EllipsisCharacter;
						//format.FormatFlags = StringFormatFlags.;
						var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconSize, toolbarItem.ShowText, toolbarItem.TextWidth);
						var iconSize = toolbarItem.IconSize.ToSize();
						var textArea = new Rectangle(
							buttonLayout.Padding.Vertical + iconSize.Width + offset.X,
							buttonLayout.Padding.Top + offset.Y,
							buttonLayout.Size.Width - iconSize.Width - buttonLayout.Padding.Right - buttonLayout.Padding.Horizontal - buttonLayout.MenuWidth,
							buttonLayout.Size.Height - buttonLayout.Padding.Vertical
						);
						var prevTextRenderingHint = e.Graphics.TextRenderingHint;
						try {
						// HACK: なんとかならんのかコレ
							var textOffsetColors = new [] {
								new {X = +1, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 左
								new {X = +1, Y = -1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 上
								new {X = +1, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 右
								new {X = +1, Y = +1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 下
								new {X = -1, Y = -1, TextBrush = textBrush,   Hint = TextRenderingHint.ClearTypeGridFit }, // 戻し
							};
							foreach(var offsetColor in textOffsetColors) {
								textArea.X += offsetColor.X;
								textArea.Y += offsetColor.Y;
								
								e.Graphics.TextRenderingHint = offsetColor.Hint;
								e.Graphics.DrawString(e.Text, e.TextFont, offsetColor.TextBrush, textArea, format);
							}
						} finally {
							
							e.Graphics.TextRenderingHint = prevTextRenderingHint;
						}
					}
				}
			}
		}
		
		public override bool IsDefaultDrawToolbarWindowBackground { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarWindowEdge { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarWindowCaption { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarBackground { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarBorder { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarArrow { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarButtonImage { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarButtonText { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarDropDownButtonBackground { get { return !EnabledVisualStyle; } }
		public override bool IsDefaultDrawToolbarSplitButtonBackground { get { return !EnabledVisualStyle; } }

		protected override void DrawToolbarArrowImage(ToolbarButtonData toolbarButtonData)
		{
			var padding = new Padding(3);
			var size = toolbarButtonData.MenuArea.Width - padding.Horizontal;
			var arrowArea = new RectangleF(
				(float)(toolbarButtonData.MenuArea.Left + toolbarButtonData.MenuArea.Width / 2.0 - size / 2.0),
				(float)(toolbarButtonData.MenuArea.Top + toolbarButtonData.MenuArea.Height / 2.0 - size / 2.0),
				size,
				size
			);
			var lines = new[] {
				new PointF(arrowArea.Left, arrowArea.Top),
				new PointF(arrowArea.Right, arrowArea.Top),
				new PointF(arrowArea.Left + arrowArea.Width / 2, arrowArea.Bottom),
			};
			byte alpha = 170;
			Color startColor, endColor;
			if(toolbarButtonData.MenuState == ToolbarButtonState.Pressed) {
				startColor = Color.FromArgb(alpha, Color.White);
				endColor = Color.FromArgb(alpha, Color.Gray);
			} else {
				startColor = Color.FromArgb(alpha, Color.Gray);
				endColor = Color.FromArgb(alpha, Color.White);
			}
			using(var brush = new LinearGradientBrush(arrowArea, startColor, endColor, LinearGradientMode.Vertical)) {
				toolbarButtonData.Graphics.FillPolygon(brush, lines);
			}
		}
		
		protected override void DrawToolbarButton(ToolbarButtonData toolbarButtonData)
		{
			var g = toolbarButtonData.Graphics;
			g.SmoothingMode = SmoothingMode.HighQuality;
			var drawArea = toolbarButtonData.ButtonArea;
			var correction = new Padding(1);// 幅・高さの補正px
			
			if(toolbarButtonData.ButtonState != ToolbarButtonState.None) {
				// ボタン全体の境界線を描画する
				using(var path = new GraphicsPath()) {
					// 領域作成
					var arcSize = new SizeF(4.5f, 4.5f);
					path.StartFigure();
					path.AddArc(drawArea.Right - arcSize.Width - correction.Left, drawArea.Top, arcSize.Width, arcSize.Height, 270, 90);
					path.AddArc(drawArea.Right - arcSize.Width - correction.Left, drawArea.Bottom - correction.Bottom - arcSize.Height, arcSize.Width, arcSize.Height - correction.Bottom, 0, 90);
					path.AddArc(drawArea.Left, drawArea.Bottom - arcSize.Height - correction.Bottom, arcSize.Width, arcSize.Height - correction.Bottom, 90, 90);
					path.AddArc(drawArea.Left, drawArea.Top, arcSize.Width, arcSize.Height, 180, 90);
					path.CloseFigure();
					byte alpha = 0;
					switch(toolbarButtonData.ButtonState) {
							case ToolbarButtonState.Normal:   alpha = 70;  break;
						case ToolbarButtonState.Selected:
							case ToolbarButtonState.Pressed:  alpha = 210; break;
						default:
							Debug.Assert(false, toolbarButtonData.ButtonState.ToString());
							break;
					}
					
					RectangleF fillArea = drawArea;
					Color startColor, endColor;
					if(toolbarButtonData.ButtonState == ToolbarButtonState.Pressed) {
						startColor = Color.FromArgb(0, Color.White);
						endColor = Color.FromArgb(alpha, Color.White);
					} else {
						startColor = Color.FromArgb(alpha, Color.White);
						endColor = Color.FromArgb(0, Color.White);
					}
					// 背景 TODO: クリップなしのべた塗
					using(var brush = new LinearGradientBrush(fillArea, startColor, endColor, LinearGradientMode.ForwardDiagonal)) {
						//g.FillRectangle(brush, fillArea);
						g.FillPath(brush, path);
						g.ResetClip();
					}
					// 境界線
					using(var pen = new Pen(Color.FromArgb(alpha, Color.White))) {
						pen.Alignment = PenAlignment.Inset;
						g.DrawPath(pen, path);
					}
					// 境界線
					if(toolbarButtonData.HasMenuSplit) {
						var menuArea = toolbarButtonData.MenuArea;
						using(var pen = new Pen(Color.FromArgb(alpha, Color.Gray))) {
							pen.Alignment = PenAlignment.Left;
							g.DrawLine(pen, menuArea.Left, menuArea.Top + correction.Top, menuArea.Left, menuArea.Bottom - correction.Vertical);
						}
						// ボタン内メニューボタンあり
						if(toolbarButtonData.ButtonState == ToolbarButtonState.Selected && toolbarButtonData.MenuState == ToolbarButtonState.Pressed) {
							g.SetClip(new Rectangle(menuArea.Left + correction.Left, menuArea.Top + correction.Top, menuArea.Left - correction.Horizontal, menuArea.Bottom - correction.Vertical));
							using(var brush = new LinearGradientBrush(menuArea, endColor, startColor, LinearGradientMode.Vertical)) {
								g.FillPath(brush, path);
							}
							g.ResetClip();
						}
					}
				}
			}
			if(toolbarButtonData.HasArrow && toolbarButtonData.MenuState != ToolbarButtonState.None) {
				// 矢印描画
				DrawToolbarArrowImage(toolbarButtonData);
			}
		}

	}

}

