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
using PInvoke.Windows;

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
		
#region Layout Toolbar
		public override Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition)
		{
			var frame = SystemInformation.Border3DSize;
			var edge = new Padding(frame.Width, frame.Height, frame.Width, frame.Height);
			
			if(EnabledVisualStyle) {
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
			if(EnabledVisualStyle) {
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
		
		public override Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, NoteCommand noteCommand)
		{
			int pos = 0;
			switch(noteCommand) {
				case NoteCommand.Close:   pos = 1; break;
				case NoteCommand.Compact: pos = 2; break;
				default: 
					Debug.Assert(false);
					break;
			}
			var size = new Size(parentArea.Height, parentArea.Height);
			return new Rectangle(
				new Point(parentArea.Right - size.Width * pos - pos*2, parentArea.Top),
				size
			);
		}

#endregion
		
		
#region Draw Toolbar
		public override void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position)
		{
			g.Clear(VisualColor);
		}
		
		public override void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolPosition)
		{
			var edgePadding = GetToolbarWindowEdgePadding(toolPosition);
			Color startColor = Color.FromArgb(150, Color.White);
			Color endColor = Color.FromArgb(70, Color.White);
			
			Rectangle headArea, tailArea;
			if(ToolbarPositionUtility.IsHorizonMode(toolPosition)) {
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
				
				if(ToolbarPositionUtility.IsDockingMode(toolPosition)) {
					Point startPoint, endPoint;
					var lineWidth = 1;
					switch(toolPosition) {
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
							Debug.Assert(false, toolPosition.ToString());
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
			
			var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconScale, false, 0);
			var iconSize = toolbarItem.IconScale.ToSize();
			e.Graphics.DrawImage(e.Image, buttonLayout.Padding.Left + offset.X, buttonLayout.Padding.Top + offset.Y, iconSize.Width, iconSize.Height);
		}
		
		public override void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem)
		{
			var offset = GetPressOffset(e.Item);
			
			using(var textBrush = new SolidBrush(Color.FromArgb(255, Color.White))) {
				using(var shadowBrush = new SolidBrush(Color.FromArgb(200, Color.Gray))) {
					using(var format = ToStringFormat(e.TextFormat)) {
						format.LineAlignment = StringAlignment.Center;
						format.Trimming = StringTrimming.EllipsisCharacter;
						//format.FormatFlags = StringFormatFlags.;
						var buttonLayout = GetToolbarButtonLayout(toolbarItem.IconScale, toolbarItem.ShowText, toolbarItem.TextWidth);
						var iconSize = toolbarItem.IconScale.ToSize();
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
#endregion 
		
#region Draw Note

		public override void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor)
		{
			g.Clear(backColor);
		}
		
		public override void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor)
		{
			var padding = GetNoteWindowEdgePadding();
			
			using(var brush = new SolidBrush(Color.FromArgb(128, Color.Black))) {
				Rectangle edgeArea;
				// 左
				edgeArea = new Rectangle(drawArea.Left, drawArea.Top, padding.Left, drawArea.Bottom);
				g.FillRectangle(brush, edgeArea);
				// 右
				edgeArea = new Rectangle(drawArea.Right - padding.Right, drawArea.Top, padding.Right, drawArea.Bottom);
				g.FillRectangle(brush, edgeArea);
				// 上
				edgeArea = new Rectangle(drawArea.Left, drawArea.Top, drawArea.Width, padding.Top);
				g.FillRectangle(brush, edgeArea);
				// 下
				edgeArea = new Rectangle(drawArea.Left, drawArea.Bottom - padding.Bottom, drawArea.Width, padding.Bottom);
				g.FillRectangle(brush, edgeArea);
			}
		}
		
		public override void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption)
		{
			using(var brush = new SolidBrush(Color.FromArgb(128, Color.Red))) {
				g.FillRectangle(brush, drawArea);
				g.DrawString(caption, font, SystemBrushes.ActiveCaption, drawArea);
			}
		}
		
		public override void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, NoteCommand noteCommand, ButtonState buttonState)
		{
			Color color = Color.Transparent;
			switch(buttonState) {
				case ButtonState.Normal:
					color = Color.Green;
					break;
				case ButtonState.Selected:
					color = Color.Blue;
					break;
				case ButtonState.Pressed:
					color = Color.Red;
					break;
				default:
					Debug.Assert(false, buttonState.ToString());
					break;
			}
				
			using(var brush = new SolidBrush(color)) {
				g.FillRectangle(brush, drawArea);
				g.DrawString(noteCommand.ToString()[0].ToString(), SystemFonts.CaptionFont, SystemBrushes.ActiveCaption, drawArea);
			}
		}
		
		public override void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string body)
		{
			
		}

#endregion
		
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
			if(toolbarButtonData.MenuState == ButtonState.Pressed) {
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
			
			if(toolbarButtonData.ButtonState != ButtonState.None) {
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
							case ButtonState.Normal:   alpha = 70;  break;
						case ButtonState.Selected:
							case ButtonState.Pressed:  alpha = 210; break;
						default:
							Debug.Assert(false, toolbarButtonData.ButtonState.ToString());
							break;
					}
					
					RectangleF fillArea = drawArea;
					Color startColor, endColor;
					if(toolbarButtonData.ButtonState == ButtonState.Pressed) {
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
						if(toolbarButtonData.ButtonState == ButtonState.Selected && toolbarButtonData.MenuState == ButtonState.Pressed) {
							g.SetClip(new Rectangle(menuArea.Left + correction.Left, menuArea.Top + correction.Top, menuArea.Left - correction.Horizontal, menuArea.Bottom - correction.Vertical));
							using(var brush = new LinearGradientBrush(menuArea, endColor, startColor, LinearGradientMode.Vertical)) {
								g.FillPath(brush, path);
							}
							g.ResetClip();
						}
					}
				}
			}
			if(toolbarButtonData.HasArrow && toolbarButtonData.MenuState != ButtonState.None) {
				// 矢印描画
				DrawToolbarArrowImage(toolbarButtonData);
			}
		}

	}

}

