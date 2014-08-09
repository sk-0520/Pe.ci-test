/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/16
 * 時刻: 23:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using PeMain.Data;
using PeMain.Logic;

namespace PeMain.UI
{
	public partial class SettingForm
	{
		void ApplyLanguageTab()
		{
			this.tabSetting_pageMain.Text = Language["setting/tab/main"];
			this.tabSetting_pageLauncher.Text = Language["setting/tab/launcher"];
			this.pageLauncherCommon.Text = Language["setting/tab/launcher/basic"];
			this.pageLauncherEnv.Text = Language["common/tab/env"];
			this.pageLauncherOthers.Text = Language["setting/tab/launcher/others"];
			this.tabSetting_pageToolbar.Text = Language["setting/tab/toolbar"];
			this.tabSetting_pageCommand.Text = Language["setting/tab/command"];
			this.tabSetting_pageDisplay.Text = Language["setting/tab/display"];
			this.tabSetting_pageNote.Text = Language["setting/tab/note"];
		}
		
		void ApplyLanguageLog()
		{
			this.selectLogVisible.Text = Language["common/label/visible"];
			this.selectLogAddShow.Text = Language["setting/check/add-show"];
			this.selectLogTrigger_information.Text = LogType.Information.ToText(Language);
			this.selectLogTrigger_warning.Text = LogType.Warning.ToText(Language);
			this.selectLogTrigger_error.Text = LogType.Error.ToText(Language);
		}
		
		void ApplyLanguageSystemEnv()
		{
			this.inputSystemEnvExt.SetLanguage(Language);
			this.inputSystemEnvHiddenFile.SetLanguage(Language);
			
			this.labelSystemEnvExt.Text = Language["setting/label/extension"];
			this.labelSystemEnvHiddenFile.Text = Language["setting/label/hiddenfile"];
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.Text = Language["setting/group/log"];
			this.groupMainSystemEnv.Text = Language["setting/group/system-env"];
			this.labelMainLanguage.Text = Language["setting/label/language"];
			
			ApplyLanguageLog();
			ApplyLanguageSystemEnv();
		}
		
		void ApplyLanguageCommand()
		{
			this.commandCommandFont.SetLanguage(Language);
			this.inputCommandHotkey.SetLanguage(Language);
			
			this.selectCommandTopmost.Text = Language["common/label/topmost"];
			this.labelCommandFont.Text = Language["common/label/font"];
			this.labelCommandIcon.Text = Language["enum/icon-size"];
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.envLauncherUpdate.SetLanguage(Language);
			this.envLauncherRemove.SetLanguage(Language);
			this.groupLauncherType.Text = Language["setting/group/item-type"];
			this.selectLauncherType_file.Text = LauncherType.File.ToText(Language);
			this.selectLauncherType_uri.Text = LauncherType.URI.ToText(Language);
			this.selectLauncherStdStream.Text = Language["setting/check/std-stream"];
			this.labelLauncherName.Text = Language["setting/label/item-name"];
			this.labelLauncherCommand.Text = Language["setting/label/command"];
			this.labelLauncherOption.Text = Language["setting/label/option"];
			this.labelLauncherWorkDirPath.Text = Language["setting/label/work-dir"];
			this.labelLauncherIconPath.Text = Language["setting/label/icon-path"];
			this.labelLauncherTag.Text = Language["setting/label/tags"];
			this.labelLauncherNote.Text = Language["setting/label/note"];
			this.selectLauncherAdmin.Text = Language["common/check/admin"];
			this.selectLauncherEnv.Text = Language["execute/check/edit-env"];
			
		}
		
		void ApplyLanguageToolbar()
		{
			this.selecterToolbar.SetLanguage(Language);
			this.commandToolbarFont.SetLanguage(Language);
			
			this.selectToolbarTopmost.Text = Language["common/label/topmost"];
			this.selectToolbarVisible.Text = Language["common/label/visible"];
			this.selectToolbarShowText.Text = Language["setting/check/show-text"];
			this.labelToolbarIcon.Text = Language["enum/icon-size"];
			this.selectToolbarAutoHide.Text = Language["setting/check/auto-hide"];
			this.labelToolbarTextWidth.Text = Language["setting/label/text-width"];
			this.labelToolbarPosition.Text = Language["enum/toolbar-position"];
			this.labelToolbarFont.Text = Language["common/label/font"];
			
			this.toolToolbarGroup_addGroup.ToolTipText = Language["setting/tips/add-group"];
			this.toolToolbarGroup_addItem.ToolTipText = Language["setting/tips/add-item"];
			this.toolToolbarGroup_up.ToolTipText = Language["setting/tips/up-item"];
			this.toolToolbarGroup_down.ToolTipText = Language["setting/tips/down-item"];
			this.toolToolbarGroup_remove.ToolTipText = Language["setting/tips/remove-item"];

		}
		
		void ApplyLanguageDisplay()
		{
			
		}
		
		void ApplyLanguageNote()
		{
			this.inputNoteCreate.SetLanguage(Language);
			this.inputNoteHidden.SetLanguage(Language);
			this.inputNoteCompact.SetLanguage(Language);
			
			this.commandNoteCaptionFont.SetLanguage(Language);
			
			this.labelNoteCreate.Text = Language["setting/label/note-create"];
			this.labelNoteHiddent.Text = Language["setting/label/note-hidden"];
			this.labelNoteCompact.Text = Language["setting/label/note-compact"];
			this.labelNoteCaptionFont.Text = Language["common/label/font"];
		}
		
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			DialogUtility.SetDefaultText(this, Language);
			
			ApplyLanguageTab();
			ApplyLanguageMain();
			ApplyLanguageLauncher();
			ApplyLanguageToolbar();
			ApplyLanguageCommand();
			ApplyLanguageDisplay();
			ApplyLanguageNote();
		}
	}
}
