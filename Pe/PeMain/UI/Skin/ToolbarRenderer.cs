/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/02/06
 * 時刻: 23:30
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Application.Data;
using ContentTypeTextNet.Pe.Application.IF;
using ContentTypeTextNet.Pe.Library.Skin;

namespace ContentTypeTextNet.Pe.Application.UI
{
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
				var buttonLayout = Skin.GetToolbarButtonLayout(ToolbarItem.IconScale, false, 0);
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
			if(e.ToolStrip.IsDropDown || Skin.IsDefaultDrawToolbarButtonText) {
				base.OnRenderItemText(e);
			} else {
				Skin.DrawToolbarButtonText(e, IsActive(e.ToolStrip), ToolbarItem.IconScale, ToolbarItem.ShowText, ToolbarItem.TextWidth);
			}
		}
		
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarDropDownButtonBackground) {
				base.OnRenderDropDownButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarDropDownButtonBackground(e, (ToolStripDropDownButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}
		
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarSplitButtonBackground) {
				base.OnRenderSplitButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarSplitButtonBackground(e, (ToolStripSplitButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}

		protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarButtonBackground) {
				base.OnRenderButtonBackground(e);
			} else {
				var itemArea = new Rectangle(Point.Empty, e.Item.Size);
				Skin.DrawToolbarButtonBackground(e, (ToolStripButton)e.Item, IsActive(e.ToolStrip), itemArea);
			}
		}
		
	}
}
