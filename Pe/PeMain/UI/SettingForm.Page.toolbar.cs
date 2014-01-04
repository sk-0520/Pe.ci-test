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
			this._imageToolbarItemGroup.Images.Clear();
			this._imageToolbarItemGroup.Images.Add(PeMain.Properties.Images.Group);
			
			var seq = this.selecterLauncher.Items.Select(item => new { Name = item.Name, Icon = item.GetIcon(IconSize.Small)}).Where(item => item.Icon != null);
			foreach(var elemet in seq) {
				this._imageToolbarItemGroup.Images.Add(elemet.Name, elemet.Icon);
			}
			
			// イメージリスト再設定のために一度null初期化
			this.treeToolbarItemGroup.ImageList = null;
			this.treeToolbarItemGroup.StateImageList = null;
			// イメージリスト再設定
			this.treeToolbarItemGroup.ImageList = this._imageToolbarItemGroup;
			this.treeToolbarItemGroup.StateImageList = this._imageToolbarItemGroup;
		}
		
		TreeNode ToolbarAddGroup(string groupName)
		{
			var node = new TreeNode();
			node.Text = groupName;
			this.treeToolbarItemGroup.Nodes.Add(node);
			
			return node;
		}
		
		void ToolbarSetItem(TreeNode node, LauncherItem item)
		{
			Debug.Assert(node != null);
			Debug.Assert(item != null);
			
			node.Text = item.Name;
			node.ImageKey = item.Name;
			node.SelectedImageKey = item.Name;
			node.Tag = item;
		}
		
		void ToolbarAddItem(TreeNode parentNode, LauncherItem item)
		{
			Debug.Assert(parentNode != null);
			/*
			var items = this.selecterToolbar.Items;
			if(items != null && items.Count() > 0) {
				var item = this.selecterToolbar.SelectedItem;
				if(item == null) {
					item = items.First();
				}
				var node = new TreeNode();
				ToolbarSetItem(node, item);
				parentNode.Nodes.Add(node);
				if(!parentNode.IsExpanded) {
					parentNode.Expand();
				}
			}
			*/
			var node = new TreeNode();
			ToolbarSetItem(node, item);
			parentNode.Nodes.Add(node);
			if(!parentNode.IsExpanded) {
				parentNode.Expand();
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
		
		void ToolbarExportSetting(ToolbarSetting setting)
		{
			setting.ToolbarPosition = (ToolbarPosition)this.selectToolbarPosition.SelectedValue;
			setting.Topmost = this.selectToolbarTopmost.Checked;
			setting.AutoHide = this.selectToolbarAutoHide.Checked;
			setting.Visible = this.selectToolbarVisible.Checked;
			
			setting.FontSetting = this._toolbarFont;
			
			// ツリーからグループ項目構築
			foreach(TreeNode groupNode in this.treeToolbarItemGroup.Nodes) {
				var toolbarGroupItem = new ToolbarGroupItem();
				
				// グループ項目
				var groupName = groupNode.Text;
				toolbarGroupItem.Name = groupName;
				
				// グループに紐付くアイテム名
				toolbarGroupItem.ItemNames.AddRange(groupNode.Nodes.Cast<TreeNode>().Select(node => node.Text));

				setting.ToolbarGroup.Groups.Add(toolbarGroupItem);
			}
		}
	}
}
