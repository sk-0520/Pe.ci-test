/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/08/02
 * 時刻: 17:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.IO;
using ObjectDumper;
using PeMain.Data;

namespace PeMain.Logic
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
		
		public void Dispose()
		{
			Puts(LogType.None, "FileLogger", "End");
			if(this._stream != null) {
				this._stream.Dispose();
			}
		}
		
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			var logItem = new LogItem(logType, title, detail, frame);
			WiteItem(logItem);
		}
		
		public void WiteItem(LogItem logItem)
		{
			if(this._stream != null) {
				this._stream.WriteLine(
					"====================================={0}" +
					"{1} {2}{3}" +
					"{4}" +
					"{5}{6}",
					Environment.NewLine, 
					logItem.DateTime, logItem.Title, Environment.NewLine,
					//logItem.Detail.DumpToString(logItem.Title),
					logItem.Detail,
					logItem.StackTrace, Environment.NewLine
				);
				this._stream.Flush();
			}
		}
	}
}
