/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/31
 * 時刻: 11:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using PeMain.Setting;
using PeUtility;

namespace PeMain.UI
{
	public partial class SettingForm
	{
		void ToolbarSelectingPage()
		{
			this.selecterToolbar.SetItems(this.selecterLauncher.Items);
			this.imageToolbarItemGroup.Images.Clear();
			var seq = this.selecterLauncher.Items.Select(item => new { Name = item.Name, Icon = item.GetIcon(IconSize.Small)}).Where(item => item.Icon != null);
			foreach(var elemet in seq) {
				this.imageToolbarItemGroup.Images.Add(elemet.Name, elemet.Icon);
			}
		}
		
		void ToolbarAddGroup()
		{
			var node = new TreeNode();
			node.Text = Language["setting/toolbar/add-group"];
			this.treeToolbarItemGroup.Nodes.Add(node);
		}
		
		void ToolbarSetItem(TreeNode node, LauncherItem item)
		{
			Debug.Assert(node != null);
			Debug.Assert(item != null);
			
			node.Text = item.Name;
			node.ImageKey = item.Name;
			node.Tag = item;
		}
		
		void ToolbarAddItem(TreeNode parentNode)
		{
			Debug.Assert(parentNode != null);
			var items = this.selecterToolbar.Items;
			if(items != null && items.Count() > 0) {
				var item = this.selecterToolbar.SelectedItem;
				if(item == null) {
					item = items.First();
				}
				var node = new TreeNode();
				ToolbarSetItem(node, item);
				parentNode.Nodes.Add(node);
			}
		}
		
		void ToolbarSelectedChangeGroupItem(LauncherItem item)
		{
			Debug.Assert(item != null);
			var showItem = this.selecterToolbar.ViewItems.Any(i => i == item);
			if(!showItem) {
				this.selecterToolbar.Filtering = false;
			}
			this.selecterToolbar.SelectedItem = item;
		}
	}
}
