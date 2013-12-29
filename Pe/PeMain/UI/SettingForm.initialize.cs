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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using PeMain.Logic;
using PeMain.Setting;
using PeUtility;

namespace PeMain.UI
{
	/// <summary>
	/// Description of SettingForm_initialize.
	/// </summary>
	public partial class SettingForm
	{
		void InitializeMainSetting(MainSetting mainSetting)
		{
			
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
			this._commandFont = commandSetting.FontSetting;
			SetViewMessage(this.commandCommandFont, this._commandFont);
			
			// アイコンサイズ文言の項目構築
			AttachmentIconSize(this.selectCommandIcon, commandSetting.IconSize);
		}
		
		void InitializeToolbar(ToolbarSetting toolbarSetting)
		{
			this.selecterToolbar.SetItems(this._launcherItems);
			
			this._toolbarFont = toolbarSetting.FontSetting;
			SetViewMessage(this.commandToolbarFont, this._toolbarFont);
			
			// ツールーバー位置の項目構築
			var toolbarPosList = new List<ToolbarPositionItem>();
			foreach(var value in new [] { ToolbarPosition.DesktopFloat, ToolbarPosition.DesktopLeft, ToolbarPosition.DesktopTop, ToolbarPosition.DesktopRight, ToolbarPosition.DesktopBottom, }) {
				var data = new ToolbarPositionItem(value, Language);
				toolbarPosList.Add(data);
			}
			this.selectToolbarPosition.Attachment(toolbarPosList);
			
			// アイコンサイズ文言の項目構築
			AttachmentIconSize(this.selectToolbarIcon, toolbarSetting.IconSize);
		}
		
		void InitializeUI(MainSetting mainSetting)
		{
			ApplyLanguage();
			
			InitializeMainSetting(mainSetting);
			InitializeLauncher(mainSetting.Launcher);
			InitializeCommand(mainSetting.Command);
			InitializeToolbar(mainSetting.Toolbar);
		}
		
		void Initialize(Language language, MainSetting mainSetting)
		{
			this._launcherItems = new HashSet<LauncherItem>();
			
			Language = language;
			
			InitializeUI(mainSetting);
		}

	}
}
