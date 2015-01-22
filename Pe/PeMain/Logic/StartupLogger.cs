namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System.Collections.Generic;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	class StartupLogger: ILogger
	{
		List<LogItem> _logList;

		public FileLogger FileLogger { get; set; }

		public StartupLogger(FileLogger fileLogger)
		{
			FileLogger = fileLogger;
			this._logList = new List<LogItem>();
		}

		#region ILogger

		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			FileLogger.WiteItem(logItem);
			this._logList.Add(logItem);
		}

		public void PutsDebug(string title, object detail, int frame = 3)
		{
			Puts(LogType.Debug, title, detail, frame);
		}

		#endregion

		public IEnumerable<LogItem> GetList()
		{
			return this._logList;
		}
	}
}
