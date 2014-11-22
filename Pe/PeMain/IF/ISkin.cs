/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/10/13
 * 時刻: 11:34
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Data;
using PeSkin;
using PeUtility;

namespace PeMain.IF
{
	public struct SkinToolbarButtonLayout
	{
		public System.Drawing.Size Size { get; set; }
		public Padding Padding { get; set; }
		public int MenuWidth { get; set; }
	}
	
	public struct SkinNoteStatus
	{
		public bool Locked { get; set; }
		public bool Topmost { get; set; }
		public bool Compact { get; set; }
	}
		
	public enum SkinButtonState
	{
		None,
		Normal,
		Selected,
		Pressed,
	}
	
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
		
		Padding GetNoteWindowEdgePadding();
		Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, NoteCommand noteCommand);
		
		void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarArrow(ToolStripArrowRenderEventArgs e, int menuWidth);
		void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea);
		void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea);
		void DrawToolbarButtonBackground(ToolStripItemRenderEventArgs e, ToolStripButton item, bool active, Rectangle itemArea);
		
		void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor);
		void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);
		void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption);
		void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, NoteCommand noteCommand, SkinButtonState buttonState);
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
	}
	
}
