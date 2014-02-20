/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 01/10/2014
 * 時刻: 23:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace PeMain.Data
{
	[Flags]
	public enum LogType
	{
		None = 0x00,
		Information = 0x01,
		Warning = 0x02,
		Error = 0x04,
	}
	
	public struct LogData
	{
		public LogType LogType { get; set; }
		public string Title { get; set; }
		public string Detail { get; set; }
	}
	
	public class LogItem
	{
		public LogItem(LogType logType, string title, object detail, int frame = 1)
		{
			Debug.Assert(!string.IsNullOrEmpty(title));
			Debug.Assert(detail != null);
			Debug.Assert(frame >= 1);
			
			LogType = logType;
			Title = title;
			Detail = detail;
			StackTrace = new StackTrace(frame, true);
			DateTime = DateTime.Now;
		}
		
		public LogType LogType { get; private set; }
		public string Title { get; private set; }
		public object Detail { get; private set; }
		public StackTrace StackTrace { get; private set; }
		public DateTime DateTime { get; private set; }
	}
}
