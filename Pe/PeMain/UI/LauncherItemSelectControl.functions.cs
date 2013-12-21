/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/22
 * 時刻: 1:21
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of LauncherItemSelectControl_functions.
	/// </summary>
	public partial class LauncherItemSelectControl
	{
		void ResizeInputArea()
		{
			int totalWidth = this.toolLauncherItems.Margin.Horizontal + this.toolLauncherItems.Padding.Horizontal;
			foreach(ToolStripItem item in this.toolLauncherItems.Items) {
				if(item == this.toolLauncherItems_input) {
					continue;
				}
				totalWidth += item.Width + item.Padding.Horizontal + item.Margin.Horizontal;
			}
			var parentWidth = this.toolLauncherItems.Width - this.toolLauncherItems.Padding.Horizontal - this.toolLauncherItems.Margin.Horizontal;
			var inputWidth = parentWidth - totalWidth - this.toolLauncherItems.Margin.Horizontal - this.toolLauncherItems.Padding.Horizontal;
			var size = new Size(inputWidth, this.toolLauncherItems_input.Size.Height);
			this.toolLauncherItems_input.Size = size;
		}
	}
}
