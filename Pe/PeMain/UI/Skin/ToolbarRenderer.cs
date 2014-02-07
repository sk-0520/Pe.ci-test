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
using PeMain.Data;

namespace PeMain.UI
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
			if(Skin.IsDefaultDrawToolbarBackground) {
				base.OnRenderToolStripBackground(e);
			} else {
				Skin.DrawToolbarBackground(e, IsActive(e.ToolStrip), ToolbarItem.ToolbarPosition);
			}
		}
		
		protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarBorder) {
				base.OnRenderToolStripBorder(e);
			} else {
				Skin.DrawToolbarBorder(e, IsActive(e.ToolStrip), ToolbarItem.ToolbarPosition);
			}
		}
		
		protected override void OnRenderItemImage(ToolStripItemImageRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarButtonImage) {
				base.OnRenderItemImage(e);
			} else {
				Skin.DrawToolbarButtonImage(e, IsActive(e.ToolStrip), ToolbarItem);
			}
		}
		
		protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarButtonText) {
				base.OnRenderItemText(e);
			} else {
				Skin.DrawToolbarButtonText(e, IsActive(e.ToolStrip), ToolbarItem);
			}
		}
		
		protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarDropDownButtonBackground) {
				base.OnRenderDropDownButtonBackground(e);
			} else {
				Skin.DrawToolbarDropDownButtonBackground(e, (ToolStripDropDownButton)e.Item, IsActive(e.ToolStrip));
			}
		}
		
		protected override void OnRenderSplitButtonBackground(ToolStripItemRenderEventArgs e)
		{
			if(Skin.IsDefaultDrawToolbarSplitButtonBackground) {
				base.OnRenderSplitButtonBackground(e);
			} else {
				Skin.DrawToolbarSplitButtonBackground(e, (ToolStripSplitButton)e.Item, IsActive(e.ToolStrip));
			}
		}
		
	}
}
