/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using PeMain.Logic;
using PeMain.Data;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm.
	/// </summary>
	public partial class SettingForm : Form
	{
		public SettingForm(Language language, MainSetting setting)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize(language, setting);
		}
		
		
		void SelecterLauncher_SelectChnagedItem(object sender, SelectedItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			if(e.Item == null) {
				// 未選択状態
				LauncherInputClear();
				this.splitContainer1.Panel2.Enabled = false; // NOTE: 暫定対応
				return;
			}
			if(e.Item == this._launcherSelectedItem) {
				// 現在選択中アイテム
				return;
			}
			this.splitContainer1.Panel2.Enabled = true; // NOTE: 暫定対応
			LauncherSelectItem(e.Item);
		}
		
		void SelecterLauncher_CreateItem(object sender, CreateItemEventArg e)
		{
			if(this._launcherSelectedItem != null) {
				// 現在アイテムに入力内容を退避
				LauncherInputValueToItem(this._launcherSelectedItem);
			}
			LauncherSelectItem(e.Item);
		}
		
		void TabSetting_Selecting(object sender, TabControlCancelEventArgs e)
		{
			if(this._nowSelectedTabPage == this.pageLauncher) {
				e.Cancel = !LauncherItemValid();
			}
			if(!e.Cancel) {
				if(e.TabPage == this.pageToolbar) {
					ToolbarSelectingPage();
				}
				this._nowSelectedTabPage =  e.TabPage;
			}
		}
		
		void CommandLauncherFilePath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogFilePath(this.inputLauncherCommand);
		}
		
		void CommandLauncherDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherCommand);
		}
		
		void CommandLauncherWorkDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherWorkDirPath);
		}
		
		void CommandLauncherIconPath_Click(object sender, EventArgs e)
		{
			LauncherOpenIcon();
		}
		
		void CommandLauncherOptionFilePath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogFilePath(this.inputLauncherOption);
		}
		
		void CommandLauncherOptionDirPath_Click(object sender, EventArgs e)
		{
			DialogUtility.OpenDialogDirPath(this.inputLauncherOption);
		}
		
		void CommandCommandFont_Click(object sender, EventArgs e)
		{
			var fontSetting = OpenDialogFontSetting(this.commandCommandFont, this._commandFont);
			if(fontSetting != null) {
				this._commandFont = fontSetting;
				SetViewMessage(this.commandCommandFont, this._commandFont);
			}
		}
		
		void CommandToolbarFont_Click(object sender, EventArgs e)
		{
			var fontSetting = OpenDialogFontSetting(this.commandToolbarFont, this._toolbarSelectedToolbarItem.FontSetting);
			if(fontSetting != null) {
				this._toolbarSelectedToolbarItem.FontSetting = fontSetting;
				SetViewMessage(this.commandToolbarFont, this._toolbarSelectedToolbarItem.FontSetting);
			}
		}
		
		
		void ToolToolbarGroup_addGroup_Click(object sender, EventArgs e)
		{
			ToolbarAddGroup(Language["group/new"]);
		}
		
		void ToolToolbarGroup_addItem_Click(object sender, EventArgs e)
		{
			var selectedNode = this.treeToolbarItemGroup.SelectedNode;
			if(selectedNode != null) {
				var parentNode = selectedNode;
				if(selectedNode.Level == TREE_LEVEL_ITEM) {
					parentNode = selectedNode.Parent;
				}
				
				var items = this.selecterToolbar.Items;
				if(items != null && items.Count() > 0) {
					var item = this.selecterToolbar.SelectedItem;
					if(item == null) {
						item = items.First();
					}
					ToolbarAddItem(parentNode, item);
				}
			}
		}
		
		void ToolToolbarGroup_up_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.MoveToUp(true);
			}
		}
		
		void ToolToolbarGroup_down_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.MoveToDown(true);
			}
		}
		
		void ToolToolbarGroup_remove_Click(object sender, EventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node != null) {
				node.Remove();
			}
		}
		
		void TreeToolbarItemGroup_AfterSelect(object sender, TreeViewEventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node.Level == TREE_LEVEL_ITEM) {
				ToolbarSelectedChangeGroupItem((LauncherItem)node.Tag);
			}
		}
		
		void SelecterToolbar_SelectChangedItem(object sender, SelectedItemEventArg e)
		{
			var item = this.selecterToolbar.SelectedItem;
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(item != null && node != null && node.Level == TREE_LEVEL_ITEM) {
				ToolbarSetItem(node, item);
			}
		}
		
		void PageLauncher_DragEnter(object sender, DragEventArgs e)
		{
			if(e.Data.GetDataPresent(DataFormats.FileDrop)) {
				var datas = (string[])e.Data.GetData(DataFormats.FileDrop, false);
				if(datas.Length == 1) {
					e.Effect = DragDropEffects.Copy;
				} else {
					e.Effect = DragDropEffects.None;
				}
			} else {
				e.Effect = DragDropEffects.None;
			}
		}
		
		void PageLauncher_DragDrop(object sender, DragEventArgs e)
		{
			var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop, false)).First();
			LauncherAddFile(filePath);
		}
		
		
		void TreeToolbarItemGroup_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			var node = this.treeToolbarItemGroup.SelectedNode;
			if(node == null) {
				// 到達不可のはず
				return;
			}
			e.CancelEdit = node.Level != TREE_LEVEL_GROUP;
		}
		
		
		void TreeToolbarItemGroup_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.F2) {
				var node = this.treeToolbarItemGroup.SelectedNode;
				if(node != null) {
					node.BeginEdit();
				}
			}
		}
		
		void CommandSubmit_Click(object sender, System.EventArgs e)
		{
			if(CheckValidate()) {
				// 設定データ生成
				CreateSettingData();
				DialogResult = DialogResult.OK;
			}
		}

		
		void InputLauncherName_TextChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void SelectLauncherType_file_CheckedChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
			if(sender == this.selectLauncherEnv) {
				this.panelEnv.Enabled = this.selectLauncherEnv.Checked;
			}
		}
		
		void InputLauncherIconIndex_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void SelectToolbarItem_SelectedValueChanged(object sender, EventArgs e)
		{
			var toolbarItem = this.selectToolbarItem.SelectedValue as ToolbarItem;
			if(this._toolbarSelectedToolbarItem != null && toolbarItem != null) {
				ToolbarSetSelectedItem(this._toolbarSelectedToolbarItem);
				ToolbarSelectedChangeToolbarItem(toolbarItem);
			}
		}
	}
}
