namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.PeMain.Kind;
	using ObjectDumper;

	/// <summary>
	/// ログ出力時の必要データ。
	/// </summary>
	public struct LogData
	{
		public LogType LogType { get; set; }
		public string Title { get; set; }
		public string Detail { get; set; }
	}
	
	/// <summary>
	/// ログアイテム。
	/// 
	/// アイテムだけどシリアライズは特に何も。
	/// </summary>
	public class LogItem
	{
		public LogItem(LogType logType, string title, object detail, int frame = 1)
		{
			Debug.Assert(!string.IsNullOrEmpty(title));
			//Debug.Assert(detail != null);
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
		
		public override string ToString()
		{
			string detailText = "<NULL>";
			if(Detail is Exception || Detail is string) {
				detailText = Detail.ToString();
			} else if(Detail != null) {
				try {
					detailText = Detail.DumpToString(Title);
				} catch(Exception ex) {
					detailText = Detail.ToString() + "@" + ex.ToString();
				}
			}

			return string.Format(
				"=====================================" + Environment.NewLine +
				"{0} {1}" + Environment.NewLine +
				"{2}" +
				"{3}" + Environment.NewLine,
				DateTime, Title,
				detailText,
				StackTrace
			);
		}
	}
}
