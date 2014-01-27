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
			this.pageToolbar.Text = Language["setting/tab/toolbar"];
			this.pageCommand.Text = Language["setting/tab/command"];
			this.pageDisplay.Text = Language["setting/tab/display"];
			this.pageNote.Text = Language["setting/tab/note"];
		}
		
		void ApplyLanguageMain()
		{
			this.groupMainLog.Text = Language["setting/group/log"];
			this.labelMainLanguage.Text = Language["setting/label/language"];
			this.selectLogVisible.Text = Language["setting/check/visible"];
			this.selectLogAddShow.Text = Language["setting/check/add-show"];
		}
		
		void ApplyLanguageCommand()
		{
			this.selectCommandTopmost.Text = Language["common/label/topmost"];
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.selectToolbarTopmost.Text = Language["common/label/topmost"];
			this.groupLauncherType.Text = Language["setting/group/item-type"];
			this.selectLauncherType_file.Text = EnumLang.ToText(LauncherType.File, Language);
			this.selectLauncherType_uri.Text = EnumLang.ToText(LauncherType.URI, Language);
			this.selectLauncherStdStream.Text = Language["setting/check/std-stream"];
		}
		
		void ApplyLanguageToolbar()
		{
			this.selecterToolbar.SetLanguage(Language);
			
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
			
			this.commandSubmit.Text= Language["common/button/ok"];
			this.commandCancel.Text = Language["common/button/cancel"];
			
			
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
