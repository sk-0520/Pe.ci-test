namespace DodSkin
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Text;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;

	public class DotSkin: Skin
	{
		#region Initialize

		public override void Load() 
		{
		}

		public override void Initialize()
		{
		}

		#endregion

		#region Style

		public override void AttachmentStyle(Form target)
		{
		}

		public override void DetachmentStyle(Form target)
		{
		}

		#endregion

		#region Resource

		public override Image GetImage(SkinImage skinImage)
		{
			return null;
		}

		public override Icon GetIcon(SkinIcon skinIcon)
		{
			return null;
		}

		#endregion ///////////////////////////////////

		#region CreateColorBox

		public override Image CreateNoteBoxImage(Color color, Size size)
		{
			var borderColor = Color.FromArgb(255 - color.R, 255 - color.G, 255 - color.B);
			return CreateColorBoxImage(color, color, size);
		}

		#endregion ///////////////////////////////////

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
				//buttonSize.Width += Literal.toolbarTextWidth.ToRounding(textWidth);
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
				case SkinNoteCommand.Close: pos = 1; break;
				case SkinNoteCommand.Topmost: pos = 2; break;
				case SkinNoteCommand.Compact: pos = 3; break;
				default:
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
		}

		public override void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
		}


		public override void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition)
		{
		}


		public override void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition)
		{
		}

		public override void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition)
		{
		}


		public override void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale)
		{
		}

		public override void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, int textWidth)
		{
		}

		protected override void DrawToolbarArrowImage(ToolbarButtonData toolbarButtonData)
		{
		}

		protected override void DrawToolbarButton(ToolbarButtonData toolbarButtonData)
		{
		}
		#endregion

		#region Draw Note

		public override void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor)
		{
		}

		public override void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor)
		{
		}

		public override void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption)
		{
		}

		public override void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, SkinNoteCommand noteCommand, SkinButtonState buttonState)
		{
		}

		public override void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string body)
		{
		}

		#endregion

	}
}
