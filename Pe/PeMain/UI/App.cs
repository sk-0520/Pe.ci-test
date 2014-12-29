/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 17:22
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;

using Microsoft.Win32;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.Library.Utility;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.Library.PlatformInvoke.Windows;

namespace ContentTypeTextNet.Pe.PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	public sealed partial class App: IDisposable, IRootSender
	{
		#region Define

		const string menuNameNote = "menu_note";
		const string menuNameSystemEnv = "menu_systemenv";
		const string menuNameSetting = "menu_setting";
		const string menuNameAbout = "menu_about";
		const string menuNameHelp = "menu_help";
		const string menuNameExit = "menu_exit";

		const string menuNameWindowToolbar = "menu_window_toolbar";
		const string menuNameWindowNote = "menu_window_note";
		const string menuNameWindowLogger = "menu_window_logger";

		const string menuNameWindowNoteCreate = "menu_window_note_create";
		const string menuNameWindowNoteHidden = "menu_window_note_hidden";
		const string menuNameWindowNoteCompact = "menu_window_note_compact";
		const string menuNameWindowNoteShowFront = "menu_window_note_show_front";
		const string menuNameWindowNoteSeparator = "menu_window_note_separator";

		const string menuNameSystemEnvHiddenFile = "menu_systemenv_hidden";
		const string menuNameSystemEnvExtension = "menu_systemenv_ext";
		const string menuNameSystemEnvWindow = "menu_systemenv_window";
		const string menuNameSystemEnvWindowSave = "menu_systemenv_window_save";
		const string menuNameSystemEnvWindowLoad = "menu_systemenv_window_load";
		const string menuNameSystemEnvWindowSeparator = "menu_systemenv_window_separator";
		const string menuNameSystemEnvClipboard = "menu_systemenv_clipboard";

		#endregion //////////////////////////////////////////

		public App(CommandLine commandLine, FileLogger fileLogger)
		{
			Initialized = true;
			
			var logger = new StartupLogger(fileLogger);

			ExistsSettingFilePath = Initialize(commandLine, logger);
			
			#if !DISABLED_UPDATE_CHECK
			CheckUpdateProcessAsync(false);
			#endif
		}

		public void Dispose()
		{
			DetachmentSystemEvent();

			this._windowTimer.ToDispose();
			
			this._commonData.ToDispose();
			this._messageWindow.ToDispose();
			this._logForm.ToDispose();
			foreach(var w in this._noteWindowList) {
				w.ToDispose();
			}
			foreach(var w in this._toolbarForms.Values) {
				w.ToDispose();
			}
			this._contextMenu.ToDispose();
			this._notifyIcon.ToDispose();
			
			#if DEBUG
			if(File.Exists(Literal.StartupShortcutPath)) {
				File.Delete(Literal.StartupShortcutPath);
			}
			#endif
		}
		
		private void IconDoubleClick(object sender, EventArgs e)
		{
			/*
			var update = new Update(@"Z:temp", false);
			var info = update.Check();
			if(info.IsUpdate) {
				var s = string.Format("{0} {1}", info.Version, info.IsRcVersion ? "RC": "RELEASE");
				if(MessageBox.Show(s, "UPDATE", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes) {
					update.Execute();
				}
			}
			 */
			//MessageBox.Show("PON!");
			if(!this._pause) {
				ShowHomeDialog();
			}
			//ResetUI();
		}
		
		void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionSwitch", e);
			if(e.Reason == SessionSwitchReason.ConsoleConnect) {
				ResetUI();
			} else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
				AppUtility.SaveSetting(this._commonData);
			}
		}

		void SystemEvents_UserPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			if(e.Category.IsIn(UserPreferenceCategory.VisualStyle, UserPreferenceCategory.Color)) {
				this._logForm.Puts(LogType.Information, "UserPreferenceChanged", e);
				ResetUI();
			}
		}

		void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
		{
			this._logForm.Puts(LogType.Information, "SessionEnding", e);
			AppUtility.SaveSetting(this._commonData);
		}
		
		void SystemEvents_PowerModeChanged(object sender, PowerModeChangedEventArgs e)
		{
			if(e.Mode == PowerModes.Resume) {
				this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/power/resume"], e);
#if !DISABLED_UPDATE_CHECK
				CheckUpdateProcessAsync(false);
#endif
			}
		}
		
		void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
		{
			var windowItemList = GetWindowListItem(false);
			windowItemList.Name = this._commonData.Language["save-window/display"];
			PushWindowListItem(windowItemList);
			this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/display"], windowItemList);
			// #56
			ResetToolbar();
		}
		
		void NoteMenu_DropDownOpening(object sender, EventArgs e)
		{
			OpeningNoteMenu();
		}
		
		/// <summary>
		/// タイマー関連はここにまとめておこうと思う。
		/// 
		/// よって将来的な拡張に対応できるよう実装。
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Timer_Elapsed(object sender, ElapsedEventArgs e)
		{
			var timer = (System.Timers.Timer)sender;
			try {
				timer.Enabled = false;
				if(timer == this._windowTimer) {
					// 停止状態やメニュー表示状態では無視しとく
					if(!(this._pause || this._contextMenu.ShowContextMenu)) {
						var windowItemList = GetWindowListItem(false);
						windowItemList.Name = this._commonData.Language["save-window/timer"];
						PushWindowListItem(windowItemList);
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/timer"], windowItemList);
					} else {
						this._commonData.Logger.Puts(LogType.Information, this._commonData.Language["main/event/save-window/skip"], new { Pause = this._pause, ShowContextMenu = this._contextMenu.ShowContextMenu});
					}
				}
			} finally {
				if(timer.AutoReset) {
					timer.Enabled = true;
				}
			}
		}

		void Keyboard_KeyPress(object sender, KeyPressEventArgs e)
		{
			const char esc = (char)27;

			if(e.KeyChar == esc) {
				var nowTime = DateTime.Now;
				// ダブルクリック時間だけど分かりやすいのでよし
				var time = NativeMethods.GetDoubleClickTime();
				if(nowTime - this._listener.PrevToolbarHiddenTime <= TimeSpan.FromMilliseconds(time)) {
					this._listener.Keyboard.Enabled = false;
					try {
						this._listener.PrevToolbarHiddenTime = nowTime;
						HideAutoHiddenToolbar();
					} finally {
						this._listener.PrevToolbarHiddenTime = DateTime.MinValue;
						this._listener.Keyboard.Enabled = true;
					}
				} else {
					this._listener.PrevToolbarHiddenTime = nowTime;
				}
			}
		}

		void window_FormClosed(object sender, FormClosedEventArgs e)
		{
			var window = (Form)sender;
			Debug.WriteLine(window.Text);
			this._otherWindows.Remove(window);
			this._commonData.Logger.Puts(LogType.Information, sender.ToString(), e);
			window.Dispose();
		}
		

	}
	
}

