/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/21
 * 時刻: 0:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_Page_launcher.
	/// </summary>
	public partial class SettingForm
	{
		void LauncherSizeChanged()
		{
			/*
			var inputWidth = this.toolLauncherItems_input.Width + this.toolLauncherItems_input.Margin.Horizontal + this.toolLauncherItems_input.Padding.Horizontal + this.toolLauncherItems_input.Margin.Horizontal;
			var size = new Size(parentWidth - inputWidth, this.toolLauncherItems_input.Size.Height);
			this.toolLauncherItems_input.Size = size;
			*/
			int totalWidth = this.toolLauncherItems.Margin.Horizontal + this.toolLauncherItems.Padding.Horizontal;
			foreach(ToolStripItem item in this.toolLauncherItems.Items) {
				if(item == this.toolLauncherItems_input) {
					continue;
				}
				totalWidth += item.Width + item.Padding.Horizontal + item.Margin.Horizontal;
			}
			var parentWidth = this.toolLauncherItems.ClientSize.Width - this.toolLauncherItems.Padding.Horizontal - this.toolLauncherItems.Margin.Horizontal;
			var inputWidth = parentWidth - totalWidth - this.toolLauncherItems.Margin.Horizontal - this.toolLauncherItems.Padding.Horizontal;
			var size = new Size(inputWidth, this.toolLauncherItems_input.Size.Height);
			this.toolLauncherItems_input.Size = size;
		}
	}
}
