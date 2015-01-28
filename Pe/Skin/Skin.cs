//#define SELECT_TARGET

#if BUILD
#	error Deinfed BUILD!
#endif

namespace ContentTypeTextNet.Pe.Library.Skin
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Drawing.Drawing2D;
	using System.Drawing.Text;
	using System.Runtime.InteropServices;
	using System.Windows.Forms;
	using System.Windows.Forms.VisualStyles;
	using ContentTypeTextNet.Pe.Library.Skin;

	public enum SkinTarget
	{
		/// <summary>
		/// 7以下
		/// </summary>
		Windows7,
		/// <summary>
		/// 8以上
		/// </summary>
		Windows8,
	}

	public class ToolbarButtonData
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

	/// <summary>
	/// ISkinの多分共通だろうって部分の抽象版
	/// </summary>
	public abstract class Skin: ISkin
	{
		#region Static

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"), System.Security.SuppressUnmanagedCodeSecurity]
		[DllImport("dwmapi.dll")]
		static extern int DwmIsCompositionEnabled(out bool enabled);

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

			return arrowArea;
		}

		protected static bool IsEnabledAeroStyle()
		{
			bool isAero;
			DwmIsCompositionEnabled(out isAero);
			return isAero;
		}

		protected static SkinTarget GetSkinTarget()
		{
#if !SELECT_TARGET
			var version = Environment.OSVersion;
			if(version.Version.Major >= 6 && version.Version.Minor > 1) {
				return SkinTarget.Windows8;
			} else {
				return SkinTarget.Windows7;
			}
#else
			return SkinTarget.Windows8;
#endif
		}

		#endregion

		protected ISkinAbout _about;
		protected bool EnabledAeroStyle { get; set; }
		//protected bool EnabledVisualStyle { get; set; }

		protected SkinTarget SkinTarget { get; set; }

		public Skin()
		{
			SkinTarget = GetSkinTarget();
		}

		#region Initialize

		public abstract void Load();
		public abstract void Initialize();
		public abstract void Unload();

		#endregion

		#region Setting About

		public ISkinAbout About { get { return this._about; } }

		#endregion

		#region Style

		public virtual void AttachmentStyle(Form target, SkinWindow skinWindow)
		{
			EnabledAeroStyle = IsEnabledAeroStyle();
		}
		public abstract void DetachmentStyle(Form target, SkinWindow skinWindow);

		public void RefreshStyle(Form target, SkinWindow skinWindow)
		{
			DetachmentStyle(target, skinWindow);
			AttachmentStyle(target, skinWindow);
		}

		#endregion

		#region Resource

		public abstract Image GetImage(SkinImage skinImage);
		public abstract Icon GetIcon(SkinIcon skinIcon);

		#endregion

		#region Create

		public virtual Image CreateColorBoxImage(Color borderColor, Color backColor, Size size)
		{
			var image = new Bitmap(size.Width, size.Height);

			using(var g = Graphics.FromImage(image)) {
				using(var pen = new Pen(borderColor)) {
					using(var brush = new SolidBrush(backColor)) {
						var fillArea = new Rectangle(new Point(1, 1), new Size(size.Width - 2, size.Height - 2));
						g.FillRectangle(brush, fillArea);
						if(SkinTarget == Library.Skin.SkinTarget.Windows7) {
							using(var gradation = new LinearGradientBrush(fillArea, Color.FromArgb(92, Color.White), Color.Transparent, LinearGradientMode.Vertical)) {
								g.FillRectangle(gradation, fillArea);
							}
						}
						g.DrawRectangle(pen, new Rectangle(Point.Empty, new Size(size.Width - 1, size.Height - 1)));
					}
				}
			}

			return image;
		}

		public abstract Image CreateNoteBoxImage(Color color, Size size);

		#endregion ///////////////////////////////////

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
		#endregion

		#region Draw Tooltip
		
		public virtual void DrawToolTipBackground(Graphics g, Rectangle drawArea)
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
		public abstract void DrawNoteBody(Graphics g, Rectangle drawArea, bool active, SkinNoteStatus noteStatus, Color foreColor, Color backColor);

		#endregion

		#region Property

		public abstract int MenuWidth { get; }
		public abstract int PaddingWidth { get; }

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

		#endregion

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
