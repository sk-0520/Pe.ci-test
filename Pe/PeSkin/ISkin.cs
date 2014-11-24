using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PeSkin
{
	/// <summary>
	///スキン
	/// </summary>
	public interface ISkin
	{
		void Start(Form target);
		void Refresh(Form target);
		void Close(Form target);

		Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition);
		Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconSize, bool showText, int textWidth);
		void ApplyToolbarToolTipRegion(Form target);

		Padding GetNoteWindowEdgePadding();
		Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, SkinNoteCommand noteCommand);

		void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, int textWidth);
		void DrawToolbarArrow(ToolStripArrowRenderEventArgs e, int menuWidth);
		void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea);
		void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea);
		void DrawToolbarButtonBackground(ToolStripItemRenderEventArgs e, ToolStripButton item, bool active, Rectangle itemArea);
		void DrawToolbarToolTipBackground(Graphics g, Rectangle drawArea);

		void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor);
		void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);
		void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption);
		void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, SkinNoteCommand noteCommand, SkinButtonState buttonState);
		void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string body);

		int MenuWidth { get; }
		int PaddingWidth { get; }

		bool IsDefaultDrawToolbarWindowBackground { get; }
		bool IsDefaultDrawToolbarWindowCaption { get; }
		bool IsDefaultDrawToolbarWindowEdge { get; }
		bool IsDefaultDrawToolbarBackground { get; }
		bool IsDefaultDrawToolbarBorder { get; }
		bool IsDefaultDrawToolbarArrow { get; }
		bool IsDefaultDrawToolbarButtonImage { get; }
		bool IsDefaultDrawToolbarButtonText { get; }
		bool IsDefaultDrawToolbarDropDownButtonBackground { get; }
		bool IsDefaultDrawToolbarSplitButtonBackground { get; }
		bool IsDefaultDrawToolbarButtonBackground { get; }
		bool IsDefaultDrawToolbarToolTipBackground { get; }
	}
}
