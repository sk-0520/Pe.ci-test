/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/02
 * 時刻: 20:22
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
	partial class Pe
	{
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
