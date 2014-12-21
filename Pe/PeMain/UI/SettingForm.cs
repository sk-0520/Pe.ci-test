/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 21:39
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

using ContentTypeTextNet.Pe.Library.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// 設定。
	/// </summary>
	public partial class SettingForm : Form
	{
		public SettingForm(Language language, MainSetting setting, AppDBManager db)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			Initialize(language, setting, db);
		}

		#region Function
		#endregion ////////////////////////////////////////////////////


		#region Language

		void ApplyLanguageClipboard()
		{ }

		#endregion


		#region Initialize

		void InitializeClipboard(ClipboardSetting setting)
		{ }
		
		#endregion ////////////////////////////////////////////////////

		
		#region Export

		void ExportClipboardSetting(ClipboardSetting setting)
		{ }

		#endregion ////////////////////////////////////////////////////

		
		#region Page
		#region Page/Clipboard
		#endregion ////////////////////////////////////////////////////
		#endregion ////////////////////////////////////////////////////


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
			if(this._nowSelectedTabPage == this.tabSetting_pageLauncher) {
				e.Cancel = !LauncherItemValid();
			}
			if(!e.Cancel) {
				if(e.TabPage == this.tabSetting_pageToolbar) {
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
		
		void ToolToolbarGroup_addGroup_Click(object sender, EventArgs e)
		{
			ToolbarAddGroup(Language["group/new"]);
			ToolbarChangedGroupCount();
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
				ToolbarChangedGroupCount();
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
				Debug.Assert(false);
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

			/*
			if(sender == this.selectLauncherEnv) {
				//this.panelEnv.Enabled = this.selectLauncherEnv.Checked;
				var enabled = this.selectLauncherEnv.Checked;
				this.envLauncherUpdate.Enabled = enabled;
				this.envLauncherRemove.Enabled = enabled;
			}
			 */
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
		
		void EnvLauncherUpdate_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void EnvLauncherRemove_ValueChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}
		}
		
		void GridNoteItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if(e.ColumnIndex == this.gridNoteItems_columnFont.Index) {
				// フォント変更
				// TODO: ダイアログ表示を一元化する必要あり
				using(var dialog = new FontDialog()) {
					var row = this._noteItemList[e.RowIndex];
					var fontSetting = row.Font;
					if(fontSetting.IsDefault) {
						dialog.Font = fontSetting.Font;
					}
					
					if(dialog.ShowDialog() == DialogResult.OK) {
						var result = new FontSetting();
						var font = dialog.Font;
						fontSetting.Family = font.FontFamily.Name;
						fontSetting.Height = font.Size;
					}
				}
			} else if(e.ColumnIndex == this.gridNoteItems_columnFore.Index || e.ColumnIndex == this.gridNoteItems_columnBack.Index) {
				// 前景色・背景色
				var row = this._noteItemList[e.RowIndex];
				var isFore = e.ColumnIndex == this.gridNoteItems_columnFore.Index;
				using(var dialog = new ColorDialog()) {
					if(dialog.ShowDialog() == DialogResult.OK) {
						var color = dialog.Color;
						if(isFore) {
							row.Fore = color;
						} else {
							row.Back = color;
						}
					}
				}
			}
			
		}
		
		void GridNoteItems_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			if(e.ColumnIndex == this.gridNoteItems_columnFont.Index) {
				// フォント
				var row = this._noteItemList[e.RowIndex];
				e.Value = LanguageUtility.FontSettingToDisplayText(Language, row.Font);
				e.FormattingApplied = true;
			}
		}
		
		void selectLauncherEnv_CheckedChanged(object sender, EventArgs e)
		{
			var enabled = this.selectLauncherEnv.Checked;
			this.envLauncherUpdate.Enabled = enabled;
			this.envLauncherRemove.Enabled = enabled;
		}
		
		void inputLauncherIconPath_IconIndexChanged(object sender, EventArgs e)
		{
			if(this._launcherItemEvent) {
				LauncherInputChange();
			}	
		}
		
		void treeToolbarItemGroup_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			Debug.Assert(e.Node.Level == TREE_LEVEL_GROUP);
			if(e.Label == null) {
				// なんもしてない
				return;
			}
			// 編集されたのでラベル名が変更されているか
			if(e.Node.Text.Trim() == e.Label.Trim()) {
				e.CancelEdit = true;
				return;
			}
			
			// グループ名が空白は変更対象としない
			if(string.IsNullOrWhiteSpace(e.Label)) {
				e.CancelEdit = true;
				return;
			}
			
			var oldName = e.Node.Text;
			var nodes = this.treeToolbarItemGroup.Nodes.Cast<TreeNode>();
			//var changedNowSelectedToolbarItem = ToolbarItem.CheckNameEqual(this._toolbarSelectedToolbarItem.DefaultGroup, e.Node.Text);
			// グループ名重複は色々まずい
			var uniqName = TextUtility.ToUniqueDefault(e.Label.Trim(), nodes.Where(n => n != e.Node).Select(n => n.Text.Trim()));
			var useName = e.Label.Trim();
			if(uniqName != useName) {
				// 変更データを無視して採番値を設定
				e.CancelEdit = true;
				useName = e.Node.Text = uniqName;
			}
			
			
			// 変更値をツールバーの初期グループ名に反映
			var toolbarItems = this.selectToolbarItem.Items.Cast<ToolbarDisplayValue>().Select(dv => dv.Value);
			var groupList = new List<ToolbarGroupNameDisplayValue>();
			var changedGroupToolbarItem = toolbarItems.SingleOrDefault(t => t.DefaultGroup == oldName);
			if(changedGroupToolbarItem != null) {
				changedGroupToolbarItem.DefaultGroup = e.CancelEdit ? uniqName:  e.Label.Trim();
			}
			groupList.Add(new ToolbarGroupNameDisplayValue(string.Empty));
			foreach(var node in nodes) {
				if(node == e.Node) {
					groupList.Add(new ToolbarGroupNameDisplayValue(useName));
				} else {
					groupList.Add(new ToolbarGroupNameDisplayValue(node.Text));
				}
			}
			var nowIndex = this.selectToolbarGroup.SelectedIndex;
			this.selectToolbarGroup.Attachment(groupList);
			this.selectToolbarGroup.SelectedIndex = nowIndex;
		}
	}
}
