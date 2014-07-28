﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/05
 * 時刻: 22:01
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using PeMain.Data;
using PeUtility;
using PInvoke.Windows;

namespace PeMain.UI
{
	public struct SkinToolbarButtonLayout
	{
		public System.Drawing.Size Size { get; set; }
		public Padding Padding { get; set; }
		public int MenuWidth { get; set; }
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
		
		void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarArrow(ToolStripArrowRenderEventArgs e, int menuWidth);
		void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active, Rectangle itemArea);
		void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active, Rectangle itemArea);
		
		void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, Color backColor);
		void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, Color foreColor, Color backColor);

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
	}
	
	/// <summary>
	/// ISkinの多分共通だろうって部分の抽象版
	/// </summary>
	public abstract class Skin: ISkin
	{
		protected enum ToolbarButtonState
		{
			None,
			Normal,
			Selected,
			Pressed,
		}
		
		protected class ToolbarButtonData
		{
			public ToolbarButtonData()
			{
				Active = false;
				HasArrow = false;
				HasMenuSplit = false;
				ButtonState = ToolbarButtonState.None;
				MenuState = ToolbarButtonState.None;
			}
			
			public Graphics Graphics { get; set; }
			public bool Active { get; set; }
			public bool HasArrow { get; set; }
			public ArrowDirection ArrowDirection { get; set; }
			public bool HasMenuSplit { get; set; }
			public ToolbarButtonState ButtonState { get; set; }
			public ToolbarButtonState MenuState { get; set; }
			public Rectangle ButtonArea { get; set; }
			public Rectangle MenuArea { get; set; }
		}
		
		protected bool EnabledVisualStyle { get; set; }
		
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
		/// <param name="itemArea"></param>
		/// <param name="menuWidth"></param>
		/// <returns></returns>
		protected static Rectangle GetMenuArea(ToolStripItem item, int menuWidth)
		{
			var itemArea = new Rectangle(System.Drawing.Point.Empty, item.Size);
			var arrawSize = new System.Drawing.Size(menuWidth, itemArea.Height);
			var arrowArea = new Rectangle(new System.Drawing.Point(itemArea.Width - arrawSize.Width, itemArea.Height - arrawSize.Height), arrawSize);
			
			return arrowArea ;
		}
		
		protected static bool IsEnabledVisualStyle()
		{
			bool isAero;
			API.DwmIsCompositionEnabled(out isAero);
			return isAero;
		}
		
		public virtual void Start(Form target)
		{
			EnabledVisualStyle = IsEnabledVisualStyle();
		}
		public virtual void Refresh(Form target)
		{
			EnabledVisualStyle = IsEnabledVisualStyle();
		}
		public abstract void Close(Form target);
		
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
		
		public abstract SkinToolbarButtonLayout GetToolbarButtonLayout(IconScale iconSize, bool showText, int textWidth);
		
#endregion
		
#region Layout Note

		public abstract Padding GetNoteWindowEdgePadding();
		public abstract Rectangle GetNoteCaptionArea(System.Drawing.Size parentSize);
		
#endregion

#region Draw Toolbar
		public abstract void DrawToolbarWindowBackground(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarWindowEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarWindowCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		public abstract void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		public abstract void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		public abstract void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem);

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
				toolbarButtonData.MenuState = ToolbarButtonState.Pressed;
			} else if(e.Item.Selected) {
				// 選ばれている
				toolbarButtonData.MenuState = ToolbarButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.MenuState = ToolbarButtonState.Normal;
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
				toolbarButtonData.ButtonState = ToolbarButtonState.Pressed;
			} else if(item.Selected) {
				// 選ばれている
				toolbarButtonData.ButtonState = ToolbarButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.ButtonState = ToolbarButtonState.Normal;
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
				toolbarButtonData.ButtonState = ToolbarButtonState.Selected;
				toolbarButtonData.MenuState = ToolbarButtonState.Pressed;
			} else if(item.ButtonPressed) {
				// ボタンが押されている
				toolbarButtonData.ButtonState = ToolbarButtonState.Pressed;
				toolbarButtonData.MenuState = ToolbarButtonState.Pressed;
			} else if(item.Selected) {
				// ボタンが選ばれている
				toolbarButtonData.ButtonState = ToolbarButtonState.Selected;
				toolbarButtonData.MenuState = ToolbarButtonState.Selected;
			} else {
				// 通常
				toolbarButtonData.ButtonState = ToolbarButtonState.Normal;
				toolbarButtonData.MenuState = ToolbarButtonState.Normal;
			}
			DrawToolbarButton(toolbarButtonData);
		}

#endregion
			
#region Draw Note

		public abstract void DrawNoteWindowBackground(Graphics g, Rectangle drawArea, bool active, Color backColor);
		public abstract void DrawNoteWindowEdge(Graphics g, Rectangle drawArea, bool active, Color foreColor, Color backColor);

#endregion


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
