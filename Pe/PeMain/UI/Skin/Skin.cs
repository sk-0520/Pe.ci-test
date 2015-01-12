using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;
using ContentTypeTextNet.Pe.Library.Skin;
using ContentTypeTextNet.Pe.PeMain.Data;

namespace ContentTypeTextNet.Pe.PeMain.UI
{


	/// <summary>
	/// ISkinの多分共通だろうって部分の抽象版
	/// </summary>
	public abstract class Skin: ISkin
	{
		#region Skin

		protected class ToolbarButtonData
		{
			public ToolbarButtonData()
			{
				Active = false;
				HasArrow = false;
				HasMenuSplit = false;
				ButtonState = SkinButtonState.None;
				MenuState = SkinButtonState.None;
			}
			
			public Graphics Graphics { get; set; }
			public bool Active { get; set; }
			public bool HasArrow { get; set; }
			public ArrowDirection ArrowDirection { get; set; }
			public bool HasMenuSplit { get; set; }
			public SkinButtonState ButtonState { get; set; }
			public SkinButtonState MenuState { get; set; }
			public Rectangle ButtonArea { get; set; }
			public Rectangle MenuArea { get; set; }
		}

		#region Resource

		static IReadOnlyDictionary<SkinImage, Image> _skinImageMap = new Dictionary<SkinImage, Image>() {
			{ SkinImage.Add, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Add },
			{ SkinImage.AddItem, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_AddItem },
			{ SkinImage.Applications, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Applications },
			{ SkinImage.Changelog, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Changelog },
			{ SkinImage.Clear, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Clear },
			{ SkinImage.Clipboard, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Clipboard },
			{ SkinImage.ClipboardCopy, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardCopy },
			{ SkinImage.ClipboardFile, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardFile },
			{ SkinImage.ClipboardHtml, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardHtml },
			{ SkinImage.ClipboardImage, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardImage },
			{ SkinImage.ClipboardRichTextFormat, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardRichTextFormat },
			{ SkinImage.ClipboardText, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_ClipboardText },
			{ SkinImage.Close, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Close },
			{ SkinImage.Comment, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Comment },
			{ SkinImage.Config, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Config },
			{ SkinImage.CustomColor, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_CustomColor },
			{ SkinImage.Dir, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Dir },
			{ SkinImage.Disk, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Disk },
			{ SkinImage.Down, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Down },
			{ SkinImage.Error, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Error },
			{ SkinImage.File, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_File },
			{ SkinImage.Filter, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Filter },
			{ SkinImage.Find, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Find },
			{ SkinImage.Finder, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Finder },
			{ SkinImage.Flag, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Flag },
			{ SkinImage.Font, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Font },
			{ SkinImage.FontStyle, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_FontStyle },
			{ SkinImage.Group, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Group },
			{ SkinImage.Help, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Help },
			{ SkinImage.Information, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Information },
			{ SkinImage.Kill, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Kill },
			{ SkinImage.Lock, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Lock },
			{ SkinImage.Log, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Log },
			{ SkinImage.Name, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Name },
			{ SkinImage.Note, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Note },
			{ SkinImage.NoteBody, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_NoteBody },
			{ SkinImage.NoteTitle, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_NoteTitle },
			{ SkinImage.NotImpl, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_NotImpl },
			{ SkinImage.OpenDir, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_OpenDir },
			{ SkinImage.Pin, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Pin },
			{ SkinImage.Refresh, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Refresh },
			{ SkinImage.Remove, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Remove },
			{ SkinImage.Save, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Save },
			{ SkinImage.SideContract, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_SideContract },
			{ SkinImage.SideExpand, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_SideExpand },
			{ SkinImage.SystemEnvironment, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_SystemEnvironment },
			{ SkinImage.Tag, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Tag },
			{ SkinImage.Toolbar, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Toolbar },
			{ SkinImage.Unlock, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Unlock },
			{ SkinImage.Up, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Up },
			{ SkinImage.Update, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Update },
			{ SkinImage.Warning, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Warning },
			{ SkinImage.WindowList, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_WindowList },
			{ SkinImage.WindowLoad, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_WindowLoad },
			{ SkinImage.Windows, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_Windows },
			{ SkinImage.WindowSave, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Image_WindowSave },
		};

		static IReadOnlyDictionary<SkinIcon, Icon> _skinIconMap = new Dictionary<SkinIcon, Icon>() {
			{ SkinIcon.App, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_App },
			{ SkinIcon.Command, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_Command },
			{ SkinIcon.NotFound, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_NotFound },
			{ SkinIcon.ToolbarMain, global::ContentTypeTextNet.Pe.PeMain.Properties.Resources.Icon_ToolbarMain },
		};

		#endregion

		protected bool EnabledAeroStyle { get; set; }
		//protected bool EnabledVisualStyle { get; set; }
		
		/// <summary>
		/// http://msdn.microsoft.com/ja-jp/magazine/ee221436.aspx
		/// </summary>
		/// <param name="flags"></param>
		/// <returns></returns>
		protected static StringFormat ToStringFormat(TextFormatFlags flags)
		{
			var format = new StringFormat();
			
			// 縦方向
			if((flags & TextFormatFlags.Left) == TextFormatFlags.Left) {
				format.Alignment = StringAlignment.Near;
			} else if((flags & TextFormatFlags.HorizontalCenter) == TextFormatFlags.HorizontalCenter) {
				format.Alignment = StringAlignment.Center;
			} else if((flags & TextFormatFlags.Right) == TextFormatFlags.Right) {
				format.Alignment = StringAlignment.Far;
			}
			
			// 縦方向
			if((flags & TextFormatFlags.Top) == TextFormatFlags.Top) {
				format.LineAlignment = StringAlignment.Near;
			} else if((flags & TextFormatFlags.VerticalCenter) == TextFormatFlags.VerticalCenter) {
				format.LineAlignment = StringAlignment.Center;
			} else if((flags & TextFormatFlags.Bottom) == TextFormatFlags.Bottom) {
				format.LineAlignment = StringAlignment.Far;
			}
			
			// 省略符号
			if((flags & TextFormatFlags.EndEllipsis) == TextFormatFlags.EndEllipsis) {
				format.Trimming = StringTrimming.EllipsisCharacter;
			} else if((flags & TextFormatFlags.WordEllipsis) == TextFormatFlags.WordEllipsis) {
				format.Trimming = StringTrimming.EllipsisWord;
			} else if((flags & TextFormatFlags.PathEllipsis) == TextFormatFlags.PathEllipsis) {
				format.Trimming = StringTrimming.EllipsisPath;
			}
			
			// ホットキープレフィックス
			if((flags & TextFormatFlags.NoPrefix) == TextFormatFlags.NoPrefix) {
				format.HotkeyPrefix = HotkeyPrefix.None;
			} else if((flags & TextFormatFlags.HidePrefix) == TextFormatFlags.HidePrefix) {
				format.HotkeyPrefix = HotkeyPrefix.Hide;
			}
			
			// テキストの埋め込み
			if((flags & TextFormatFlags.NoPadding) == TextFormatFlags.NoPadding) {
				format.FormatFlags |= StringFormatFlags.FitBlackBox;
			}
			
			// テキストの折り返し
			if((flags & TextFormatFlags.SingleLine) == TextFormatFlags.SingleLine) {
				format.FormatFlags |= StringFormatFlags.NoWrap;
			} else if((flags & (TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl)) == (TextFormatFlags.WordBreak | TextFormatFlags.TextBoxControl)) {
				format.FormatFlags |= StringFormatFlags.LineLimit;
			}
			
			return format;
		}
		
		/// <summary>
		/// TODO: システムからの領域無視
		/// </summary>
		/// <param name="item"></param>
		/// <param name="menuWidth"></param>
		/// <returns></returns>
		protected static Rectangle GetMenuArea(ToolStripItem item, int menuWidth)
		{
			var itemArea = new Rectangle(System.Drawing.Point.Empty, item.Size);
			var arrawSize = new System.Drawing.Size(menuWidth, itemArea.Height);
			var arrowArea = new Rectangle(new System.Drawing.Point(itemArea.Width - arrawSize.Width, itemArea.Height - arrawSize.Height), arrawSize);
			
			return arrowArea ;
		}
		
		protected static bool IsEnabledAeroStyle()
		{
			bool isAero;
			NativeMethods.DwmIsCompositionEnabled(out isAero);
			return isAero;
		}
		
		#endregion

		public virtual void Start(Form target)
		{
			EnabledAeroStyle = IsEnabledAeroStyle();
		}
		public abstract void Close(Form target);
		
		public void Refresh(Form target)
		{
			Close(target);
			Start(target);
		}

		#region Resource
		
		public Image GetImage(SkinImage skinImage)
		{
			return _skinImageMap[skinImage];
		}
		
		public Icon GetIcon(SkinIcon skinIcon)
		{
			return _skinIconMap[skinIcon];
		}

		#endregion

#region Layout Toolbar

		public abstract Padding GetToolbarWindowEdgePadding(ToolbarPosition toolbarPosition);
		public abstract Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		public abstract Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		
		public virtual Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize)
		{
			var edgePadding = GetToolbarWindowEdgePadding(toolbarPosition);
			var borderPadding = GetToolbarBorderPadding(toolbarPosition);
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

		public virtual void ApplyToolbarToolTipRegion(Form target)
		{
			if(EnabledAeroStyle && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal)) {
				var visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);

				using(Graphics g = Graphics.FromHwnd(IntPtr.Zero)) {
					target.Region = visualStyleRenderer.GetBackgroundRegion(g, new Rectangle(Point.Empty, target.Size));
				}
			}
		}

		public abstract SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconScale, bool showText, int textWidth);
		
#endregion
		
#region Layout Note

		public abstract Padding GetNoteWindowEdgePadding();
		public abstract Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		public abstract Rectangle GetNoteCommandArea(System.Drawing.Rectangle parentArea, SkinNoteCommand noteCommand);

		
#endregion

#region Draw Toolbar
		public abstract void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		public abstract void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		public abstract void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition toolbarPosition);
		public abstract void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		public abstract void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition toolbarPosition);
		public abstract void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, IconScale iconScale);
		public abstract void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, IconScale iconScale, bool showText, int textWidth);

		public virtual void DrawToolbarArrow(ToolStripArrowRenderEventArgs e, int menuWidth)
		{
			var toolbarButtonData = new ToolbarButtonData();
			toolbarButtonData.Graphics = e.Graphics;
			toolbarButtonData.Active = true;
			toolbarButtonData.HasArrow = true;
			toolbarButtonData.ArrowDirection = ArrowDirection.Down;
			toolbarButtonData.MenuArea = GetMenuArea(e.Item, menuWidth);
			
			if(e.Item.Pressed) {
				// 押されている
				toolbarButtonData.MenuState = SkinButtonState.Pressed;
			} else if(e.Item.Selected) {
				// 選ばれている
				toolbarButtonData.MenuState = SkinButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.MenuState = SkinButtonState.Normal;
			}
			
			DrawToolbarButton(toolbarButtonData);
		}
		
		public virtual void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea)
		{
			var toolbarButtonData = new ToolbarButtonData();
			toolbarButtonData.Graphics = e.Graphics;
			toolbarButtonData.Active = active;
			toolbarButtonData.HasArrow = true;
			toolbarButtonData.ArrowDirection = ArrowDirection.Down;
			toolbarButtonData.ButtonArea = itemArea;
			
			if(e.Item.Pressed) {
				// 押されている
				toolbarButtonData.ButtonState = SkinButtonState.Pressed;
			} else if(item.Selected) {
				// 選ばれている
				toolbarButtonData.ButtonState = SkinButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.ButtonState = SkinButtonState.Normal;
			}
			DrawToolbarButton(toolbarButtonData);
		}
		
		public virtual void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea)
		{
			var toolbarButtonData = new ToolbarButtonData();
			toolbarButtonData.Graphics = e.Graphics;
			toolbarButtonData.Active = active;
			toolbarButtonData.HasArrow = true;
			toolbarButtonData.ArrowDirection = ArrowDirection.Down;
			toolbarButtonData.HasMenuSplit = true;
			toolbarButtonData.ButtonArea = itemArea;
			toolbarButtonData.MenuArea = GetMenuArea(e.Item, item.DropDownButtonWidth);
			
			if(item.DropDownButtonPressed) {
				// ドロップダウンが押されている
				toolbarButtonData.ButtonState = SkinButtonState.Selected;
				toolbarButtonData.MenuState = SkinButtonState.Pressed;
			} else if(item.ButtonPressed) {
				// ボタンが押されている
				toolbarButtonData.ButtonState = SkinButtonState.Pressed;
				toolbarButtonData.MenuState = SkinButtonState.Pressed;
			} else if(item.Selected) {
				// ボタンが選ばれている
				toolbarButtonData.ButtonState = SkinButtonState.Selected;
				toolbarButtonData.MenuState = SkinButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.ButtonState = SkinButtonState.Normal;
				toolbarButtonData.MenuState = SkinButtonState.Normal;
			}
			DrawToolbarButton(toolbarButtonData);
		}
		
		public virtual void DrawToolbarButtonBackground(ToolStripItemRenderEventArgs e, ToolStripButton item, bool active, Rectangle itemArea)
		{
			var toolbarButtonData = new ToolbarButtonData();
			toolbarButtonData.Graphics = e.Graphics;
			toolbarButtonData.Active = active;
			toolbarButtonData.HasArrow = false;
			//toolbarButtonData.ArrowDirection = ArrowDirection.Down;
			//toolbarButtonData.HasMenuSplit = true;
			toolbarButtonData.ButtonArea = itemArea;
			//toolbarButtonData.MenuArea = GetMenuArea(e.Item, item.DropDownButtonWidth);
			
			if(item.Pressed) {
				// ボタンが押されている
				toolbarButtonData.ButtonState = SkinButtonState.Pressed;
				toolbarButtonData.MenuState = SkinButtonState.Pressed;
			} else if(item.Selected) {
				// ボタンが選ばれている
				toolbarButtonData.ButtonState = SkinButtonState.Selected;
				toolbarButtonData.MenuState = SkinButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.ButtonState = SkinButtonState.Normal;
				toolbarButtonData.MenuState = SkinButtonState.Normal;
			}
			DrawToolbarButton(toolbarButtonData);
		}

		public virtual void DrawToolbarToolTipBackground(Graphics g, Rectangle drawArea)
		{
			if(EnabledAeroStyle && VisualStyleRenderer.IsElementDefined(VisualStyleElement.ToolTip.Standard.Normal)) {
				var visualStyleRenderer = new VisualStyleRenderer(VisualStyleElement.ToolTip.Standard.Normal);
				visualStyleRenderer.DrawBackground(g, drawArea);
			}
		}
#endregion
			
#region Draw Note

		public abstract void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color backColor);
		public abstract void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);
		public abstract void DrawNoteCaption(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string caption);
		public abstract void DrawNoteCommand(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, SkinNoteCommand noteCommand, SkinButtonState buttonState);
		public abstract void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor, Font font, string body);


#endregion

		public virtual int MenuWidth { get { throw new NotImplementedException(); } }
		public virtual int PaddingWidth { get { throw new NotImplementedException(); } }

		public virtual bool IsDefaultDrawToolbarWindowBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarWindowEdge { get { return true; } }
		public virtual bool IsDefaultDrawToolbarWindowCaption { get { return true; } }
		public virtual bool IsDefaultDrawToolbarBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarBorder { get { return true; } }
		public virtual bool IsDefaultDrawToolbarArrow { get { return true; } }
		public virtual bool IsDefaultDrawToolbarButtonImage { get { return true; } }
		public virtual bool IsDefaultDrawToolbarButtonText { get { return true; } }
		public virtual bool IsDefaultDrawToolbarDropDownButtonBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarSplitButtonBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarButtonBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarToolTipBackground { get{ return true; } }
		
		protected virtual void DrawToolbarArrowImage(ToolbarButtonData toolbarButtonData)
		{
			throw new NotImplementedException();
		}
		
		protected virtual void DrawToolbarButton(ToolbarButtonData toolbarButtonData)
		{
			throw new NotImplementedException();
		}
	}
	
	
}
