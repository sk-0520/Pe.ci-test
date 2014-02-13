/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:04
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl.
	/// </summary>
	public partial class LauncherItemSelectControl : UserControl
	{
		public LauncherItemSelectControl()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize();
		}
		
		void LauncherItemSelectControlResize(object sender, EventArgs e)
		{
			ResizeInputArea();
		}
		
		void LauncherItemSelectControlLoad(object sender, EventArgs e)
		{
			TuneItemHeight();
			ResizeInputArea();
		}
		
		void ToolLauncherItems_type_Click(object sender, EventArgs e)
		{
			var type = new Dictionary<ToolStripItem, LauncherItemSelecterType>() {
				{this.toolLauncherItems_type_full, LauncherItemSelecterType.Full},
				{this.toolLauncherItems_type_name, LauncherItemSelecterType.Name},
				{this.toolLauncherItems_type_tag, LauncherItemSelecterType.Tag},
			}[(ToolStripItem)sender];
			
			FilterType = type;
		}
		
		void ToolLauncherItems_filter_Click(object sender, EventArgs e)
		{
			if(this.toolLauncherItems_filter.Checked) {
				Filtering = false;
			}
		}
		
		void ToolLauncherItems_createClick(object sender, EventArgs e)
		{
			CreateLauncherItem();
		}
		
		void ToolLauncherItems_removeClick(object sender, EventArgs e)
		{
			var item = this.listLauncherItems.SelectedItem;
			if(item != null) {
				RemoveLauncherItem((LauncherItem)item);
			}
		}
		
		void ListLauncherItemsSelectedIndexChanged(object sender, EventArgs e)
		{
			if(SelectChangedItem != null) {
				var index = this.listLauncherItems.SelectedIndex;
				var ev = new SelectedItemEventArg();
				if(index != -1) {
					ev.Item = (LauncherItem)this.listLauncherItems.Items[index];
				}
				SelectChangedItem(this, ev);
			}
		}
		
		void ListLauncherItems_DrawItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();
			
			var g = e.Graphics;
			if(e.Index != -1) {
				// TODO: アイコン位置と文字列位置の補正が必要
				var item = (LauncherItem)this.listLauncherItems.Items[e.Index];
				var icon = item.GetIcon(IconSize, item.IconIndex);
				if(icon != null) {
					g.DrawIcon(icon, e.Bounds.X, e.Bounds.Y);
				}
				var textArea = new RectangleF(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
				textArea.X += this.listLauncherItems.ItemHeight;
				textArea.Width -= this.listLauncherItems.ItemHeight;
				using(var brush = new SolidBrush(e.ForeColor))
				using(var format = new StringFormat()) {
					format.Alignment = StringAlignment.Near;
					format.LineAlignment = StringAlignment.Center;
					g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
					g.DrawString(item.Name, e.Font, brush, textArea, format);
				}
			}
			
			e.DrawFocusRectangle();
		}
		
		void ToolLauncherItems_input_TextChanged(object sender, EventArgs e)
		{
			Filtering = this.toolLauncherItems_input.TextLength > 0;
		}
	}
}
