/*
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
using System.Windows.Forms;

using PeMain.Data;
using PeUtility;
using PI.Windows;

namespace PeMain.UI
{
	public struct SkinToolbarButtonLayout
	{
		public System.Drawing.Size Size { get; set; }
		public Padding Padding { get; set; }
		public int MenuWidth { get; set; }
	}

	/// <summary>
	/// Description of Skin.
	/// </summary>
	public interface ISkin
	{
		void Start(Form target);
		void Refresh(Form target);
		void Close(Form target);
		
		
		Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		SkinToolbarButtonLayout GetToolbarButtonLayout(IconSize iconSize, bool showText, int textWidth);
		void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active);
		void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active);
		bool IsDefaultDrawToolbarBackground { get; }
		bool IsDefaultDrawToolbarBorder { get; }
		bool IsDefaultDrawToolbarButtonImage { get; }
		bool IsDefaultDrawToolbarButtonText { get; }
		bool IsDefaultDrawToolbarDropDownButtonBackground { get; }
		bool IsDefaultDrawToolbarSplitButtonBackground { get; }
	}
	
	public abstract class Skin: ISkin
	{
		public bool EnabledVisualStyle { get; set; }
		
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
		
		public abstract Padding GetToolbarBorderPadding(ToolbarPosition toolbarPosition);
		public abstract Rectangle GetToolbarCaptionArea(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		public abstract Padding GetToolbarTotalPadding(ToolbarPosition toolbarPosition, System.Drawing.Size parentSize);
		public abstract SkinToolbarButtonLayout GetToolbarButtonLayout(IconSize iconSize, bool showText, int textWidth);
		
		public abstract void DrawToolbarEdge(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarCaption(Graphics g, Rectangle drawArea, bool active, ToolbarPosition position);
		public abstract void DrawToolbarBackground(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		public abstract void DrawToolbarBorder(ToolStripRenderEventArgs e, bool active, ToolbarPosition position);
		public abstract void DrawToolbarButtonImage(ToolStripItemImageRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		public abstract void DrawToolbarButtonText(ToolStripItemTextRenderEventArgs e, bool active, ToolbarItem toolbarItem);
		public abstract void DrawToolbarDropDownButtonBackground(ToolStripItemRenderEventArgs e, ToolStripDropDownButton item, bool active);
		public abstract void DrawToolbarSplitButtonBackground(ToolStripItemRenderEventArgs e, ToolStripSplitButton item, bool active);
		public virtual bool IsDefaultDrawToolbarBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarBorder { get { return true; } }
		public virtual bool IsDefaultDrawToolbarButtonImage { get { return true; } }
		public virtual bool IsDefaultDrawToolbarButtonText { get { return true; } }
		public virtual bool IsDefaultDrawToolbarDropDownButtonBackground { get { return true; } }
		public virtual bool IsDefaultDrawToolbarSplitButtonBackground { get { return true; } }
	}
	
	
}
