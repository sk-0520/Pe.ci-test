/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/31
 * 時刻: 11:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Windows.Forms;

namespace PeMain.UI
{
	public partial class SettingForm
	{
		void ToolbarAddGroup()
		{
			var node = new TreeNode();
			node.Text = Language["setting/toolbar/add-group"];
			this.treeToolbarItemGroup.Nodes.Add(node);
		}
		
		void ToolbarAddItem(TreeNode parentNode)
		{
			
		}
	}
}
