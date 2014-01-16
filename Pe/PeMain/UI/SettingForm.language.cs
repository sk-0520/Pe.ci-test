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
			
		}
		
		void ApplyLanguageCommand()
		{
			this.selectCommandTopmost.Text = Language["common/label/topmost"];
		}
		
		void ApplyLanguageLauncher()
		{
			this.selecterLauncher.SetLanguage(Language);
			this.selectToolbarTopmost.Text = Language["common/label/topmost"];
			
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
			ApplyLanguageCommand();
			ApplyLanguageLauncher();
			ApplyLanguageToolbar();
			ApplyLanguageDisplay();
			ApplyLanguageNote();
		}
	}
}
