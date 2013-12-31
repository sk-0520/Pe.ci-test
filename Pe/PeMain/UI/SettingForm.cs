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
using System.Linq;
using System.Windows.Forms;
using PeMain.Setting;

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
				e.Cancel = LauncherItemValid();
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
			OpenDialogFilePath(this.inputLauncherCommand);
		}
		
		void CommandLauncherDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherCommand);
		}
		
		void CommandLauncherWorkDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherWorkDirPath);
		}
		
		void CommandLauncherIconPath_Click(object sender, EventArgs e)
		{
			LauncherOpenIcon();
		}
		
		void CommandLauncherOptionFilePath_Click(object sender, EventArgs e)
		{
			OpenDialogFilePath(this.inputLauncherOption);
		}
		
		void CommandLauncherOptionDirPath_Click(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputLauncherOption);
		}
		
		void CommandMainNoteDirPathClick(object sender, EventArgs e)
		{
			OpenDialogDirPath(this.inputMainNoteDirPath);
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
			var fontSetting = OpenDialogFontSetting(this.commandToolbarFont, this._toolbarFont);
			if(fontSetting != null) {
				this._toolbarFont = fontSetting;
				SetViewMessage(this.commandToolbarFont, this._toolbarFont);
			}
		}
		
		
		void ToolToolbarGroup_addGroup_Click(object sender, EventArgs e)
		{
			ToolbarAddGroup();
		}
		
		void ToolToolbarGroup_addItem_Click(object sender, EventArgs e)
		{
			var selectedNode = this.treeToolbarItemGroup.SelectedNode;
			if(selectedNode != null) {
				var parentNode = selectedNode;
				if(selectedNode.Level == TREE_LEVEL_ITEM) {
					parentNode = selectedNode.Parent;
				}
				ToolbarAddItem(parentNode);
			}
		}
		
		void ToolToolbarGroup_up_Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolToolbarGroup_down_Click(object sender, EventArgs e)
		{
			
		}
		
		void ToolToolbarGroup_remove_Click(object sender, EventArgs e)
		{
			
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
		
	}
}
