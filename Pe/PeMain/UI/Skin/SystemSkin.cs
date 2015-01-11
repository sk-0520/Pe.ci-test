using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
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
		
		void TuneRect(ref Rectangle r)
		{
			if (r.Width % 2 == 1)
			{
				r.Width += 1;
			}
			if (r.Height % 2 == 1)
			{
				r.Height += 1;
			}
		}
		
		Color VisualColor { get; set;}
		
		private void SetVisualStyle(Form target)
		{
			Debug.Assert(EnabledAeroStyle);

			var blurHehind = new DWM_BLURBEHIND();
			blurHehind.fEnable = true;
			blurHehind.hRgnBlur = IntPtr.Zero;
			blurHehind.dwFlags = DWM_BB.DWM_BB_ENABLE | DWM_BB.DWM_BB_BLURREGION;
			NativeMethods.DwmEnableBlurBehindWindow(target.Handle, ref blurHehind);
			
			// 設定色を取得
			uint rawColor;
			bool blend;
			NativeMethods.DwmGetColorizationColor(out rawColor, out blend);
			//VisualColor = Color.FromArgb((int)(rawColor & 0x00ffffff));
			var a = (int)((rawColor & 0xff000000) >> 24);
			var r = (int)((rawColor & 0x00ff0000) >> 16);
			var g = (int)((rawColor & 0x0000ff00) >> 8);
			var b = (int)((rawColor & 0x000000ff) >> 0);
			VisualColor = Color.FromArgb(a, r, g, b);
		}
		
		public override void Start(Form target)
		{
			base.Start(target);
			//EnabledVisualStyle = false;
			if(EnabledAeroStyle) {
				SetVisualStyle(target);
			}
		}
		
		public override void Close(Form target)
		{
			if(EnabledAeroStyle) {
				var margin = new MARGINS();
				margin.leftWidth = 0;
				margin.rightWidth = 0;
				margin.topHeight = 0;
				margin.bottomHeight = 0;
				NativeMethods.DwmExtendFrameIntoClientArea(target.Handle, ref margin);
			}
		}
		
		#region Layout Toolbar
		public override Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition)
		{
			var frame = SystemInformation.Border3DSize;
			var edge = new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
			
			if(EnabledAeroStyle) {
				switch(toolbarPosition) {
					case ToolbarPosition.DesktopFloat:
						edge.Top = edge.Bottom = 0;
						break;
						
					case ToolbarPosition.DesktopTop:
						edge.Top = 0;
						break;
						
					case ToolbarPosition.DesktopBottom:
						edge.Bottom = 0;
						break;
						
					case ToolbarPosition.DesktopLeft:
						edge.Left = 0;
						break;
						
					case ToolbarPosition.DesktopRight:
						edge.Right = 0;
						break;
						
					default:
						Debug.Assert(false, toolbarPosition.ToString());
						break;
				}
			}
			return edge;
		}
		
		public override Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition)
		{
			if(EnabledAeroStyle) {
				var frame = SystemInformation.Border3DSize;
				var border = new Padding(0);
				switch(toolbarPosition) {
					case ToolbarPosition.DesktopFloat:
						break;
						
					case ToolbarPosition.DesktopTop:
					case ToolbarPosition.DesktopBottom:
						border.Left = frame.Width;
						border.Right = frame.Width;
						break;
						
					case ToolbarPosition.DesktopLeft:
					case ToolbarPosition.DesktopRight:
						border.Top = frame.Height;
						border.Bottom = frame.Height;
						break;
						
					default:
						Debug.Assert(false, toolbarPosition.ToString());
						break;
				}
				
				return border;
			} else {
				return new Padding(0, 0, 0, 1);
			}
		}
		
		public override Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			if(toolbarPosition != ToolbarPosition.DesktopFloat) {
				return Rectangle.Empty;
			}
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
		
		public override SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconScale, bool showText, int textWidth)
		{
			var iconBox = iconScale.ToSize();
			var systemBorderSize = SystemInformation.Border3DSize;
			var systemPaddingSize = SystemInformation.FixedFrameBorderSize;
			var padding = new Padding(
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2,
				systemBorderSize.Width + systemPaddingSize.Width / 2,
				systemBorderSize.Height + systemPaddingSize.Height / 2
			);
			var buttonSize = new Size();
			
			buttonSize.Width = PaddingWidth + iconBox.Width + padding.Right + padding.Horizontal + MenuWidth;
			if(showText) {
				buttonSize.Width += Literal.toolbarTextWidth.ToRounding(textWidth);
			}
			buttonSize.Height = iconBox.Height + padding.Vertical;
			
			var buttonLayout = new SkinToolbarButtonLayout();
			buttonLayout.Size = buttonSize;
			buttonLayout.Padding = padding;
			buttonLayout.MenuWidth = MenuWidth;
			return buttonLayout;
		}
		#endregion
		
		#region Layout Note
		public override Padding GetNoteWindowEdgePadding()
		{
			var size = SystemInformation.Border3DSize;
			return new Padding() {
				Left = size.Width,
				Right = size.Width,
				Top = size.Height,
				Bottom = size.Height
			};
		}
		public override Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize)
		{
			var height = SystemInformation.CaptionHeight;
			var padding = GetNoteWindowEdgePadding();
			return new Rectangle(
				padding.Left,
				padding.Top,
				parentSize.Width - padding.Vertical,
				height
			);
		}
		
		public override Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, SkinNoteCommand noteCommand)
		{
			int pos = 0;
			switch(noteCommand) {
					case SkinNoteCommand.Close:   pos = 1; break;
					case SkinNoteCommand.Topmost: pos = 2; break;
					case SkinNoteCommand.Compact: pos = 3; break;
				default:
					Debug.Assert(false);
					break;
			}
			var size = new Size(parentArea.Height, parentArea.Height);
			return new Rectangle(
				new Point(parentArea.Right - size.Width * pos, parentArea.Top),
				size
			);
		}

		#endregion
		
		#region Draw Toolbar
		public override void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
			g.Clear(VisualColor);
		}
		
		public override void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
			var edgePadding = GetToolbarWindowEdgePadding(toolbarPosition);
			Color startColor = Color.FromArgb(150, Color.White);
			Color endColor = Color.FromArgb(70, Color.White);
			
			Rectangle headArea, tailArea;
			if(ToolbarPositionUtility.IsHorizonMode(toolbarPosition)) {
				headArea = new Rectangle(drawArea.Location, new Size(edgePadding.Left, drawArea.Height));
				tailArea = new Rectangle(new Point(drawArea.Width - edgePadding.Right, 0), new Size(edgePadding.Right, drawArea.Height));
			} else {
				headArea = new Rectangle(drawArea.Location, new Size(drawArea.Width, edgePadding.Top));
				tailArea = new Rectangle(new Point(0, drawArea.Height - edgePadding.Bottom), new Size(drawArea.Width, edgePadding.Bottom));
			}
			var prevSmoothingMode = g.SmoothingMode;
			try {
				g.SmoothingMode = SmoothingMode.AntiAlias;
				
				using(var brush = new LinearGradientBrush(headArea, startColor, endColor, LinearGradientMode.ForwardDiagonal)) {
					g.FillRectangle(brush, headArea);
				}
				using(var brush = new LinearGradientBrush(tailArea, endColor, startColor, LinearGradientMode.BackwardDiagonal)) {
					g.FillRectangle(brush, tailArea);
				}
				
				if(ToolbarPositionUtility.IsDockingMode(toolbarPosition)) {
					Point startPoint, endPoint;
					const int lineWidth = 1;
					switch(toolbarPosition) {
						case ToolbarPosition.DesktopTop:
							startPoint = new Point(drawArea.Left, drawArea.Bottom - lineWidth);
							endPoint = new Point(drawArea.Right, drawArea.Bottom - lineWidth);
							break;
							
						case ToolbarPosition.DesktopBottom:
							startPoint = new Point(drawArea.Left, drawArea.Top);
							endPoint = new Point(drawArea.Right, drawArea.Top);
							break;
							
						case ToolbarPosition.DesktopLeft:
							startPoint = new Point(drawArea.Right - lineWidth, drawArea.Top);
							endPoint = new Point(drawArea.Right - lineWidth, drawArea.Bottom);
							break;
							
						case ToolbarPosition.DesktopRight:
							startPoint = new Point(drawArea.Left, drawArea.Top);
							endPoint = new Point(drawArea.Left, drawArea.Bottom);
							break;
							
						default:
							startPoint = endPoint = Point.Empty;
							Debug.Assert(false, toolbarPosition.ToString());
							break;
					}
					using(var pen = new Pen(Color.FromArgb(180, Color.White))) {
						pen.Width = lineWidth;
						pen.Alignment = PenAlignment.Center;
						g.DrawLine(pen, startPoint, endPoint);
					}
				}
			} finally {
				g.SmoothingMode = prevSmoothingMode;
			}
		}

		
		public override void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
			var prevSmoothingMode = SmoothingMode.AntiAlias;
			try {
				g.SmoothingMode = SmoothingMode.AntiAlias;
				var dotSize = new Size(2, 2);
				var paddingSize = new Size(1, 1);
				using(var image = new Bitmap(dotSize.Width + paddingSize.Width, dotSize.Height + paddingSize.Height, PixelFormat.Format32bppArgb)) {
					using(var graphics = Graphics.FromImage(image)) {
						var dotArea = new Rectangle(Point.Empty, new Size(dotSize.Width, dotSize.Height));
						var startColor = Color.FromArgb(80, Color.White);
						var endColor = Color.FromArgb(190, Color.Black);
						using(var brush = new LinearGradientBrush(dotArea, startColor, endColor, LinearGradientMode.ForwardDiagonal)) {
							//graphics.Clear(Color.Transparent);
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
		
		
		public override void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition)
		{
			e.Graphics.Clear(VisualColor);
		}
		
		public override void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition)
		{
		}
		
		
		public override void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale)
		{
			var offset = GetPressOffset(e.Item);

			var buttonLayout = GetToolbarButtonLayout(iconScale, false, 0);
			var iconSize = iconScale.ToSize();
			e.Graphics.DrawImage(e.Image, PaddingWidth + buttonLayout.Padding.Left + offset.X, buttonLayout.Padding.Top + offset.Y, iconSize.Width, iconSize.Height);
		}
		
		public override void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, int textWidth)
		{
			var offset = GetPressOffset(e.Item);
			
			using(var textBrush = new SolidBrush(Color.FromArgb(255, Color.White))) {
				using(var shadowBrush = new SolidBrush(Color.FromArgb(200, Color.Gray))) {
					using(var format = ToStringFormat(e.TextFormat)) {
						format.LineAlignment = StringAlignment.Center;
						format.Trimming = StringTrimming.EllipsisCharacter;
						//format.FormatFlags = StringFormatFlags.;
						var buttonLayout = GetToolbarButtonLayout(iconScale, showText, textWidth);
						var iconSize = iconScale.ToSize();
						var textArea = new Rectangle(
							PaddingWidth + buttonLayout.Padding.Vertical + iconSize.Width + offset.X,
							buttonLayout.Padding.Top + offset.Y,
							buttonLayout.Size.Width - iconSize.Width - buttonLayout.Padding.Right - buttonLayout.Padding.Horizontal - buttonLayout.MenuWidth - PaddingWidth,
							buttonLayout.Size.Height - buttonLayout.Padding.Vertical
						);
						var prevTextRenderingHint = e.Graphics.TextRenderingHint;
						try {
							// HACK: なんとかならんのかコレ
							var textOffsetColors = new [] {
								new {X = +1, Y = -1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +0, Y = -1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = -1, Y = -1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +1, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +0, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = -1, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +1, Y = +1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +0, Y = +1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = -1, Y = +1, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								//new {X = +0, Y = +0, TextBrush = shadowBrush, Hint = TextRenderingHint.AntiAlias }, // 
								new {X = +0, Y = +0, TextBrush = textBrush,   Hint = TextRenderingHint.ClearTypeGridFit }, // 
							};
							foreach(var offsetColor in textOffsetColors) {
								var tempArea = textArea;
								tempArea.X += offsetColor.X;
								tempArea.Y += offsetColor.Y;
								
								e.Graphics.TextRenderingHint = offsetColor.Hint;
								e.Graphics.DrawString(e.Text, e.TextFont, offsetColor.TextBrush, tempArea, format);
							}
						} finally {
							e.Graphics.TextRenderingHint = prevTextRenderingHint;
						}
					}
				}
			}
		}

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
			const byte alpha = 170;
			Color startColor, endColor;
			if(toolbarButtonData.MenuState == SkinButtonState.Pressed) {
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
			
			if(toolbarButtonData.ButtonState != SkinButtonState.None) {
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
							case SkinButtonState.Normal:   alpha = 70;  break;
						case SkinButtonState.Selected:
							case SkinButtonState.Pressed:  alpha = 210; break;
						default:
							Debug.Assert(false, toolbarButtonData.ButtonState.ToString());
							break;
					}
					
					RectangleF fillArea = drawArea;
					Color startColor, endColor;
					if(toolbarButtonData.ButtonState == SkinButtonState.Pressed) {
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
						if(toolbarButtonData.ButtonState == SkinButtonState.Selected && toolbarButtonData.MenuState == SkinButtonState.Pressed) {
							g.SetClip(new Rectangle(menuArea.Left + correction.Left, menuArea.Top + correction.Top, menuArea.Left - correction.Horizontal, menuArea.Bottom - correction.Vertical));
							using(var brush = new LinearGradientBrush(menuArea, endColor, startColor, LinearGradientMode.Vertical)) {
								g.FillPath(brush, path);
							}
							g.ResetClip();
						}
					}
				}
			}
			if(toolbarButtonData.HasArrow && toolbarButtonData.MenuState != SkinButtonState.None) {
				// 矢印描画
				DrawToolbarArrowImage(toolbarButtonData);
			}
		}
		#endregion
		
		#region Draw Note

		public override void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor)
		{
			g.Clear(backColor);
		}
		
		public override void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor)
		{
			var edge = GetNoteWindowEdgePadding();
			Rectangle edgeArea;
			
			const byte alpha = 128;
			
			// 左
			edgeArea = new Rectangle(drawArea.Left, drawArea.Top, edge.Left, drawArea.Bottom);
			TuneRect(ref edgeArea);
			using(var brush = new LinearGradientBrush(edgeArea, Color.FromArgb(alpha, Color.White), Color.Transparent, LinearGradientMode.Horizontal)) {
				g.FillRectangle(brush, edgeArea);
			}
			// 上
			edgeArea = new Rectangle(drawArea.Left, drawArea.Top, drawArea.Width, edge.Top);
			TuneRect(ref edgeArea);
			using(var brush = new LinearGradientBrush(edgeArea, Color.FromArgb(alpha, Color.White), Color.Transparent, LinearGradientMode.Vertical)) {
				g.FillRectangle(brush, edgeArea);
			}
			// 右
			edgeArea = new Rectangle(drawArea.Right - edge.Right, drawArea.Top, edge.Right, drawArea.Bottom);
			TuneRect(ref edgeArea);
			using(var brush = new LinearGradientBrush(edgeArea, Color.Transparent, Color.FromArgb(alpha, Color.Black), LinearGradientMode.Horizontal)) {
				g.FillRectangle(brush, edgeArea);
			}
			// 下
			edgeArea = new Rectangle(drawArea.Left, drawArea.Bottom - edge.Bottom, drawArea.Width, edge.Bottom);
			TuneRect(ref edgeArea);
			using(var brush = new LinearGradientBrush(edgeArea, Color.Transparent, Color.FromArgb(alpha, Color.Black), LinearGradientMode.Vertical)) {
				g.FillRectangle(brush, edgeArea);
			}
		}
		
		public override void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption)
		{
			using(var sf = new StringFormat()) {
				using(var brush = new SolidBrush(foreColor)) {
					sf.FormatFlags = StringFormatFlags.NoWrap;
					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Center;
					g.DrawString(caption, font, brush, drawArea, sf);
				}
			}
		}
		
		public override void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, SkinNoteCommand noteCommand, SkinButtonState buttonState)
		{
			if(noteStatus.Locked) {
				return;
			}
			
			Color color = Color.Transparent;
			var buttonMap = new Dictionary<SkinNoteCommand, Dictionary<SkinButtonState, string>>() {
				{ SkinNoteCommand.Compact, new Dictionary<SkinButtonState, string>() {
						{ SkinButtonState.Normal,   noteStatus.Compact ? "1": "0" },
						{ SkinButtonState.Selected, noteStatus.Compact ? "1": "0" },
						{ SkinButtonState.Pressed,  noteStatus.Compact ? "1": "0" },
					}},
				{ SkinNoteCommand.Topmost, new Dictionary<SkinButtonState, string>() {
						{ SkinButtonState.Normal,   "ë" },
						{ SkinButtonState.Selected, "ë" },
						{ SkinButtonState.Pressed,  "ë" },
					}},
				{ SkinNoteCommand.Close, new Dictionary<SkinButtonState, string>() {
						{ SkinButtonState.Normal,   "r" },
						{ SkinButtonState.Selected, "r" },
						{ SkinButtonState.Pressed,  "r" },
					}},
			};
			var button = buttonMap[noteCommand][buttonState];

			using(var brush = new SolidBrush(backColor)) {
				g.FillRectangle(brush, drawArea);
				if(noteCommand == SkinNoteCommand.Topmost && noteStatus.Topmost) {
					using(var pen = new Pen(Color.FromArgb(128, foreColor))) {
						var area = new Rectangle(drawArea.X, drawArea.Y, drawArea.Width - 1, drawArea.Height - 1);
						g.DrawRectangle(pen, area);
					}
				}
			}
			using(var sf = new StringFormat()) {
				using(var font = new Font("Webdings", SystemFonts.CaptionFont.SizeInPoints)) {
					using(var brush = new SolidBrush(foreColor)) {
						sf.Alignment = StringAlignment.Center;
						sf.LineAlignment = StringAlignment.Center;
						
						g.DrawString(button, font, brush, drawArea, sf);
					}
				}
			}
			/*
			var color = new Dictionary<PeMain.IF.ButtonState, float[]>() {
				{ PeMain.IF.ButtonState.Normal,   new float[] { 1, 0, 0 } },
				{ PeMain.IF.ButtonState.Selected, new float[] { 0, 1, 0 } },
				{ PeMain.IF.ButtonState.Pressed,  new float[] { 0, 0, 1 } },
			}[buttonState];
			using(var img = new Bitmap(@"Z:\download\rgbedit\alpha.png")) {
				using(var alpha = DrawUtility.Coloring(img, color[0], color[1], color[2])) {
					g.DrawImage(alpha, drawArea.Location);
				}
			}
			*/
		}
		
		public override void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string body)
		{
			var edge = GetNoteWindowEdgePadding();
			var backArea = new Rectangle(
				drawArea.Left - edge.Left,
				drawArea.Top,
				drawArea.Width + edge.Horizontal,
				drawArea.Height + edge.Bottom
			);
			
			using(var brush = new LinearGradientBrush(backArea, Color.FromArgb(128, Color.White), Color.Transparent, LinearGradientMode.Vertical)) {
				g.FillRectangle(brush, backArea);
			}
			
			using(var sf = new StringFormat()) {
				using(var brush = new SolidBrush(foreColor)) {
					sf.Alignment = StringAlignment.Near;
					sf.LineAlignment = StringAlignment.Near;
					g.DrawString(body, font, brush, drawArea, sf);
				}
			}
		}

		#endregion
		
		public override int MenuWidth { get { return 12 ;} }
		public override int PaddingWidth { get { return MenuWidth / 2; } }
		//public override int PaddingWidth { get { return 0; } }
		
		#region IsDefaultDrawToolbar
		public override bool IsDefaultDrawToolbarWindowBackground { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarWindowEdge { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarWindowCaption { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarBackground { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarBorder { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarArrow { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarButtonImage { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarButtonText { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarDropDownButtonBackground { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarSplitButtonBackground { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarButtonBackground { get { return !EnabledAeroStyle; } }
		public override bool IsDefaultDrawToolbarToolTipBackground { get { return !EnabledAeroStyle; } }
		#endregion

	}

}

