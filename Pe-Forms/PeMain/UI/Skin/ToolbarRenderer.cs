namespace ContentTypeTextNet.Pe.PeMain.UI.Skin
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.PeMain.Data;

	/// <summary>
	/// ISkinを元に描画する
	/// </summary>
	public class ToolbarRenderer: ToolStripSystemRenderer
	{
		public ISkin Skin { get; set; }
		public ToolbarItem ToolbarItem { get; set; }
		
		private static bool IsActive(ToolStrip drawTarget)
		{
			return drawTarget.FindForm() == Form.ActiveForm;
		}
		
		protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
		{
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarBackground) {
				base.OnRenderToolStripBackground(e);
			} else {
				Skin.DrawToolbarBackground(e, IsActive(e.ToolStrip), ToolbarItem.ToolbarPosition);
			}
		}
		
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarBorder) {
				base.OnRenderToolStripBorder(e);
			} else {
				Skin.DrawToolbarBorder(e, IsActive(e.ToolStrip), ToolbarItem.ToolbarPosition);
			}
		}
		
		protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarArrow || e.Item is ToolStripMenuItem) {
				base.OnRenderArrow(e);
			} else {
				var buttonLayout = Skin.GetToolbarButtonLayout(ToolbarItem.IconScale, false, new Tuple<int, int, int>(0, 0, 0));
				Skin.DrawToolbarArrow(e, buttonLayout.MenuWidth);
			}
		}
		
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarButtonImage) {
				base.OnRenderItemImage(e);
			} else {
				Skin.DrawToolbarButtonImage(e, IsActive(e.ToolStrip), ToolbarItem.IconScale);
			}
		}
		
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if((e.ToolStrip.IsDropDown && e.Item is ToolStripMenuItem) || Skin.IsDefaultDrawToolbarButtonText) {
				base.OnRenderItemText(e);
			} else {
				Skin.DrawToolbarButtonText(e, IsActive(e.ToolStrip), ToolbarItem.IconScale, ToolbarItem.ShowText, new Tuple<int, int, int>(Literal.toolbarTextWidth.minimum, ToolbarItem.TextWidth, Literal.toolbarTextWidth.maximum));
			}
		}
		
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarDropDownButtonBackground) {
				base.OnRenderDropDownButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarDropDownButtonBackground(e, (ToolStripDropDownButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}
		
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarSplitButtonBackground) {
				base.OnRenderSplitButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarSplitButtonBackground(e, (ToolStripSplitButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarButtonBackground) {
				base.OnRenderButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarButtonBackground(e, (ToolStripButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}
		
	}
}
