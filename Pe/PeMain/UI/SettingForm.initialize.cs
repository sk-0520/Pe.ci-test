/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 12/16/2013
 * 時刻: 22:52
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using PeMain.Data;
using PeMain.Logic;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_initialize.
	/// </summary>
	public partial class SettingForm
	{
		void InitializeLog(LogSetting logSetting)
		{
			this.selectLogVisible.Checked = logSetting.Visible;
			this.selectLogAddShow.Checked = logSetting.AddShow;
			
			this.selectLogTrigger_information.Checked = (logSetting.AddShowTrigger & LogType.Information) == LogType.Information;
			this.selectLogTrigger_warning.Checked     = (logSetting.AddShowTrigger & LogType.Warning) == LogType.Warning;
			this.selectLogTrigger_error.Checked       = (logSetting.AddShowTrigger & LogType.Error) == LogType.Error;
		}
		
		void InitializeSystemEnv(SystemEnvSetting systemEnvSetting)
		{
			/*
			this.inputSystemEnvHiddenFile.Hotkey = systemEnvSetting.HiddenFileShowHotKey.Key;
			this.inputSystemEnvHiddenFile.Modifiers = systemEnvSetting.HiddenFileShowHotKey.Modifiers;
			this.inputSystemEnvHiddenFile.Registered = systemEnvSetting.HiddenFileShowHotKey.Registered;
			
			this.inputSystemEnvExt.Hotkey = systemEnvSetting.ExtensionShowHotKey.Key;
			this.inputSystemEnvExt.Modifiers = systemEnvSetting.ExtensionShowHotKey.Modifiers;
			this.inputSystemEnvExt.Registered = systemEnvSetting.ExtensionShowHotKey.Registered;
			*/
			this.inputSystemEnvHiddenFile.HotKeySetting = systemEnvSetting.HiddenFileShowHotKey;
			this.inputSystemEnvExt.HotKeySetting = systemEnvSetting.ExtensionShowHotKey;
		}
		
		void InitializeMainSetting(MainSetting mainSetting)
		{
			InitializeLog(mainSetting.Log);
			InitializeSystemEnv(mainSetting.SystemEnv);
		}
		
		void InitializeLauncher(LauncherSetting launcherSetting)
		{
			this._launcherItems.Clear();
			foreach(var item in launcherSetting.Items) {
				this._launcherItems.Add((LauncherItem)item.Clone());
			}
			this.selecterLauncher.SetItems(this._launcherItems);
		}
		
		void InitializeCommand(CommandSetting commandSetting)
		{
			//this._commandFont = commandSetting.FontSetting;
			this.commandCommandFont.FontSetting.Include(commandSetting.FontSetting);
			this.commandCommandFont.RefreshView();
			
			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectCommandIcon, commandSetting.IconScale);
			
			// ホットキー
			/*
			this.inputCommandHotkey.Hotkey = commandSetting.HotKey.Key;
			this.inputCommandHotkey.Modifiers = commandSetting.HotKey.Modifiers;
			this.inputCommandHotkey.Registered = commandSetting.HotKey.Registered;
			*/
			this.inputCommandHotkey.HotKeySetting = commandSetting.HotKey;
		}
		
		void InitializeNote(NoteSetting noteSetting)
		{
			// ホットキー
			this.inputNoteCreate.HotKeySetting = noteSetting.CreateHotKey;
			this.inputNoteCompact.HotKeySetting = noteSetting.CompactHotKey;
			this.inputNoteHidden.HotKeySetting = noteSetting.HiddenHotKey;
			
			this.commandNoteCaptionFont.FontSetting.Include(noteSetting.CaptionFontSetting);
			this.commandNoteCaptionFont.RefreshView();
			
			// 全リスト
			this.gridNoteItems.AutoGenerateColumns = false;
			var noteRawList = noteSetting.GetNoteItemList(true);
			this._noteItemList = new List<NoteWrapItem>(noteRawList.Count());
			foreach(var item in noteRawList) {
				var wrap = new NoteWrapItem(item);
				this._noteItemList.Add(wrap);
			}
			this.gridNoteItems_remove.DataPropertyName = "Remove";
			this.gridNoteItems_id.DataPropertyName = "Id";
			this.gridNoteItems_visible.DataPropertyName = "Visible";
			this.gridNoteItems_title.DataPropertyName = "Title";
			this.gridNoteItems_body.DataPropertyName = "Body";
			this.gridNoteItems_font.DataPropertyName = "Font";
			this.gridNoteItems_fore.DataPropertyName = "Fore";
			this.gridNoteItems_back.DataPropertyName = "Back";
            this.gridNoteItems.DataSource = new BindingSource(this._noteItemList, string.Empty); 
            
//			this.gridNoteItems.GetRowDisplayRectangle = noteList;
		}
		
		
		void InitializeToolbar(ToolbarSetting toolbarSetting)
		{
			this.selecterToolbar.SetItems(this._launcherItems);
			
			// ツールーバー位置の項目構築
			var toolbarPosList = new List<ToolbarPositionItem>();
			foreach(var value in new [] { ToolbarPosition.DesktopFloat, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopBottom, ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopRight, }) {
				var data = new ToolbarPositionItem(value, Language);
				toolbarPosList.Add(data);
			}
			this.selectToolbarPosition.Attachment(toolbarPosList);
			
			// アイコンサイズ文言の項目構築
			AttachmentIconScale(this.selectToolbarIcon, IconScale.Small);

			ToolbarItem initToolbarItem = null;
			var toolbarItemDataList = new List<ToolbarItemData>();
			foreach(var toolbarItem in toolbarSetting.Items) {
				if(initToolbarItem == null && toolbarItem.IsNameEqual(Screen.PrimaryScreen.DeviceName)) {
					initToolbarItem = toolbarItem;
				}
				var toolbarItemData = new ToolbarItemData(toolbarItem, Language);
				toolbarItemDataList.Add(toolbarItemData);
			}
			//this.selectToolbarItem.Attachment(toolbarItemDataList, initToolbarItem);
			this.selectToolbarItem.Attachment(toolbarItemDataList, initToolbarItem);
			this.selectToolbarItem.SelectedIndex = 0;
			ToolbarSelectedChangeToolbarItem(initToolbarItem);
			
			// グループ用項目
			this._imageToolbarItemGroup = new ImageList();
			this._imageToolbarItemGroup.ColorDepth = ColorDepth.Depth32Bit;
			
			// 各グループ構築
			foreach(var groupItem in toolbarSetting.ToolbarGroup.Groups) {
				// メイングループ
				var parentNode = ToolbarAddGroup(groupItem.Name);
				// メイングループに紐付くアイテム
				foreach(var itemName in groupItem.ItemNames) {
					var relItem = this._launcherItems.SingleOrDefault(item => item.IsNameEqual(itemName));
					if(relItem != null) {
						ToolbarAddItem(parentNode, relItem);
					}
				}
			}
		}
		
		void InitializeUI(MainSetting mainSetting)
		{
			ApplyLanguage();
			
			InitializeMainSetting(mainSetting);
			InitializeLauncher(mainSetting.Launcher);
			InitializeToolbar(mainSetting.Toolbar);
			InitializeCommand(mainSetting.Command);
			InitializeNote(mainSetting.Note);

#if RELEASE
			var debugPage = new [] { this.pageCommand, this.pageDisplay };
			foreach(var page in debugPage) {
				this.tabSetting.TabPages.Remove(page);
			}
#endif
		}
		
		void Initialize(Language language, MainSetting mainSetting)
		{
			this._launcherItems = new HashSet<LauncherItem>();
			
			Language = language;
			
			InitializeUI(mainSetting);
		}

	}
}
