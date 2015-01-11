using System.Collections.Generic;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic
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
