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
	public enum LogType
	{
		Information,
		Warning,
		Error
	}
	public class LogItem
	{
		public LogItem(LogType logtype, string title, object detail, int frame = 1)
		{
			Debug.Assert(!string.IsNullOrEmpty(title));
			Debug.Assert(detail != null);
			Debug.Assert(frame >= 1);
			
			LogType = logtype;
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
