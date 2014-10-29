/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/01/25
 * 時刻: 1:07
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using PeMain.Data;
using PeMain.IF;
using PeMain.Logic;

namespace PeMain.UI
{
	/// <summary>
	/// Description of Pe.
	/// </summary>
	partial class App
	{
		//const string menuNameWindow = "menu_window";
		const string menuNameNote = "menu_note";
		const string menuNameSystemEnv = "menu_systemenv";
		const string menuNameSetting = "menu_setting";
		const string menuNameAbout = "menu_about";
		const string menuNameHelp = "menu_help";
		const string menuNameExit = "menu_exit";
		
		const string menuNameWindowToolbar = "menu_window_toolbar";
		const string menuNameWindowNote  = "menu_window_note";
		const string menuNameWindowLogger = "menu_window_logger";

		const string menuNameWindowNoteCreate    = "menu_window_note_create";
		const string menuNameWindowNoteHidden    = "menu_window_note_hidden";
		const string menuNameWindowNoteCompact   = "menu_window_note_compact";
		const string menuNameWindowNoteShowFront = "menu_window_note_show_front";
		const string menuNameWindowNoteSeparator = "menu_window_note_separator";
		
		const string menuNameSystemEnvHiddenFile = "menu_systemenv_hidden";
		const string menuNameSystemEnvExtension = "menu_systemenv_ext";
		

		class StartupLogger: ILogger
		{
			List<LogItem> _logList;
			
			public FileLogger FileLogger { get; set; }
			
			public StartupLogger(FileLogger fileLogger)
			{
				FileLogger = fileLogger;
				this._logList = new List<LogItem>();
			}
			
			public void Puts(LogType logType, string title, object detail, int frame = 2)
			{
				var logItem = new LogItem(logType, title, detail, frame);
				FileLogger.WiteItem(logItem);
				this._logList.Add(logItem);
			}
			
			public IEnumerable<LogItem> GetList()
			{
				return this._logList;
			}
		}
		
	}
}
