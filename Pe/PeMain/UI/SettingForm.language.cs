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
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	partial class SettingForm
	{
		void ApplyLanguageTab()
		{
			this.tabSetting_pageMain.SetLanguage(Language);
			this.tabSetting_pageLauncher.SetLanguage(Language);
			this.tabSetting_pageToolbar.SetLanguage(Language);
			this.tabSetting_pageCommand.SetLanguage(Language);
			this.tabSetting_pageNote.SetLanguage(Language);
			this.tabSetting_pageDisplay.SetLanguage(Language);
			this.tabSetting_pageClipboard.SetLanguage(Language);
		}
		
		void ApplyLanguageLog()
		{
			this.selectLogVisible.SetLanguage(Language);
			this.selectLogAddShow.SetLanguage(Language);
			this.selectLogFullDetail.SetLanguage(Language);
			this.selectLogTrigger_information.Text = LogType.Information.ToText(Language);
			this.selectLogTrigger_warning.Text = LogType.Warning.ToText(Language);
			this.selectLogTrigger_error.Text = LogType.Error.ToText(Language);
		}
		
		void ApplyLanguageSystemEnv()
		{
			this.inputSystemEnvExt.SetLanguage(Language);
			this.inputSystemEnvHiddenFile.SetLanguage(Language);
			
			this.labelSystemEnvExt.SetLanguage(Language);
			this.labelSystemEnvHiddenFile.SetLanguage(Language);
		}
		
		void ApplyLanguageRunningInfo()
		{
			this.groupUpdateCheck.SetLanguage(Language);
			this.selectUpdateCheck.SetLanguage(Language);
			this.selectUpdateCheckRC.SetLanguage(Language);
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.SetLanguage(Language);
			this.groupMainSystemEnv.SetLanguage(Language);
			this.labelMainLanguage.SetLanguage(Language);
			this.selectMainStartup.SetLanguage(Language);
			
			ApplyLanguageLog();
			ApplyLanguageSystemEnv();
			ApplyLanguageRunningInfo();
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.envLauncherUpdate.SetLanguage(Language);
			this.envLauncherRemove.SetLanguage(Language);
			
			this.tabLauncher_pageCommon.SetLanguage(Language);
			this.tabLauncher_pageEnv.SetLanguage(Language);
			this.tabLauncher_pageOthers.SetLanguage(Language);
			
			this.groupLauncherType.SetLanguage(Language);
			this.selectLauncherType_file.Text = LauncherType.File.ToText(Language);
			this.selectLauncherType_directory.Text = LauncherType.Directory.ToText(Language);
			this.selectLauncherType_uri.Text = LauncherType.URI.ToText(Language);
			this.selectLauncherType_embedded.Text = LauncherType.Embedded.ToText(Language);
			
			this.labelLauncherName.SetLanguage(Language);
			this.labelLauncherCommand.SetLanguage(Language);
			this.labelLauncherOption.SetLanguage(Language);
			this.labelLauncherWorkDirPath.SetLanguage(Language);
			this.labelLauncherIconPath.SetLanguage(Language);
			
			this.selectLauncherEnv.SetLanguage(Language);
			
			this.labelLauncherTag.SetLanguage(Language);
			this.labelLauncherNote.SetLanguage(Language);
			
			this.selectLauncherStdStream.SetLanguage(Language);
			this.selectLauncherAdmin.SetLanguage(Language);
		}
		
		void ApplyLanguageToolbar()
		{
			this.selecterToolbar.SetLanguage(Language);
			this.commandToolbarFont.SetLanguage(Language);
			
			this.selectToolbarTopmost.SetLanguage(Language);
			this.selectToolbarVisible.SetLanguage(Language);
			this.selectToolbarAutoHide.SetLanguage(Language);
			this.selectToolbarShowText.SetLanguage(Language);
			this.labelToolbarGroup.SetLanguage(Language);
			this.labelToolbarTextWidth.SetLanguage(Language);
			this.labelToolbarPosition.SetLanguage(Language);
			this.labelToolbarIcon.SetLanguage(Language);
			this.labelToolbarFont.SetLanguage(Language);
			
			this.toolToolbarGroup_addGroup.SetLanguage(Language);
			this.toolToolbarGroup_addItem.SetLanguage(Language);
			this.toolToolbarGroup_up.SetLanguage(Language);
			this.toolToolbarGroup_down.SetLanguage(Language);
			this.toolToolbarGroup_remove.SetLanguage(Language);

		}
		
		void ApplyLanguageCommand()
		{
			this.commandCommandFont.SetLanguage(Language);
			this.inputCommandHotkey.SetLanguage(Language);
			
			this.selectCommandTopmost.SetLanguage(Language);
			this.labelCommandFont.SetLanguage(Language);
			this.labelCommandIcon.SetLanguage(Language);
		}
		
		void ApplyLanguageNote()
		{
			this.groupNoteKey.SetLanguage(Language);
			
			this.inputNoteCreate.SetLanguage(Language);
			this.inputNoteHidden.SetLanguage(Language);
			this.inputNoteCompact.SetLanguage(Language);
			this.inputNoteShowFront.SetLanguage(Language);
			
			this.commandNoteCaptionFont.SetLanguage(Language);

			this.groupNoteItem.SetLanguage(Language);
			this.gridNoteItems_columnRemove.SetLanguage(Language);
			this.gridNoteItems_columnId.SetLanguage(Language);
			this.gridNoteItems_columnVisible.SetLanguage(Language);
			this.gridNoteItems_columnLocked.SetLanguage(Language);
			this.gridNoteItems_columnBody.SetLanguage(Language);
			this.gridNoteItems_columnTitle.SetLanguage(Language);
			this.gridNoteItems_columnFont.SetLanguage(Language);
			this.gridNoteItems_columnFore.SetLanguage(Language);
			this.gridNoteItems_columnBack.SetLanguage(Language);
			
			this.labelNoteCreate.SetLanguage(Language);
			this.labelNoteHiddent.SetLanguage(Language);
			this.labelNoteCompact.SetLanguage(Language);
			this.labelNoteShowFront.SetLanguage(Language);
			this.labelNoteCaptionFont.SetLanguage(Language);
		}
		
		void ApplyLanguageDisplay()
		{
			
		}
		
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			UIUtility.SetDefaultText(this, Language);
			
			ApplyLanguageTab();
			ApplyLanguageMain();
			ApplyLanguageLauncher();
			ApplyLanguageToolbar();
			ApplyLanguageCommand();
			ApplyLanguageNote();
			ApplyLanguageDisplay();
			ApplyLanguageClipboard();
		}
	}
}
