using System;
using System.IO;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	/// <summary>
	/// Description of FileLogger.
	/// </summary>
	public class FileLogger: ILogger, IDisposable
	{
		private StreamWriter _stream;
		
		public FileLogger() { }
		
		public FileLogger(string path)
		{
			this._stream = new StreamWriter(new FileStream(path, FileMode.CreateNew));
			Puts(LogType.None, "FileLogger", "Start");
		}

		protected virtual void Dispose(bool disposing)
		{
			Puts(LogType.None, "FileLogger", "Dispose");

			if(this._stream != null) {
				this._stream.Dispose();
			}
		}
		
		public void Dispose()
		{
			Dispose(true);
		}

		#region ILogger

		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			WiteItem(logItem);
		}

		public void PutsDebug(string title, object detail, int frame = 3)
		{
			Puts(LogType.Debug, title, detail, frame);
		}

		#endregion

		public void WiteItem(LogItem logItem)
		{
			if(this._stream != null) {
				this._stream.WriteLine(logItem.ToString());
				this._stream.Flush();
			}
		}
	}
}
