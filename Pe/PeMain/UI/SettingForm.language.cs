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
			this.pageMain.Text = Language["setting/tab/main"];
			this.pageLauncher.Text = Language["setting/tab/launcher"];
			this.pageLauncherCommon.Text = Language["setting/tab/launcher/basic"];
			this.pageLauncherEnv.Text = Language["common/tab/env"];
			this.pageLauncherOthers.Text = Language["setting/tab/launcher/others"];
			this.pageToolbar.Text = Language["setting/tab/toolbar"];
			this.pageCommand.Text = Language["setting/tab/command"];
			this.pageDisplay.Text = Language["setting/tab/display"];
			this.pageNote.Text = Language["setting/tab/note"];
		}
		
		void ApplyLanguageLog()
		{
			this.selectLogVisible.Text = Language["common/label/visible"];
			this.selectLogAddShow.Text = Language["setting/check/add-show"];
		}
		
		void ApplyLanguageSystemEnv()
		{
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.Text = Language["setting/group/log"];
			this.labelMainLanguage.Text = Language["setting/label/language"];
			
			ApplyLanguageLog();
			ApplyLanguageSystemEnv();
		}
		
		void ApplyLanguageCommand()
		{
			this.selectCommandTopmost.Text = Language["common/label/topmost"];
			this.labelCommandFont.Text = Language["common/label/font"];
			this.labelCommandIcon.Text = Language["enum/icon-size"];
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.groupLauncherType.Text = Language["setting/group/item-type"];
			this.selectLauncherType_file.Text = EnumLang.ToText(LauncherType.File, Language);
			this.selectLauncherType_uri.Text = EnumLang.ToText(LauncherType.URI, Language);
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
			this.selectToolbarTopmost.Text = Language["common/label/topmost"];
			this.selectToolbarVisible.Text = Language["common/label/visible"];
			this.selectToolbarShowText.Text = Language["setting/check/show-text"];
			this.labelToolbarIcon.Text = Language["enum/icon-size"];
			this.selectToolbarAutoHide.Text = Language["setting/check/auto-hide"];
			this.labelToolbarTextWidth.Text = Language["setting/label/text-width"];
			this.labelToolbarPosition.Text = Language["enum/toolbar-position"];
			this.labelToolbarFont.Text = Language["common/label/font"];
		}
		
		void ApplyLanguageDisplay()
		{
			
		}
		
		void ApplyLanguageNote()
		{
			
		}
		
		void ApplyLanguage()
		{
			Debug.Assert(Language != null);
			
			DialogUtility.SetDefaultText(this, Language, Language["window/setting"]);
			
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
