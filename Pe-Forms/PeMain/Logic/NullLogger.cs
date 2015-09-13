namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Diagnostics;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// 無効なログにあれやこれや。
	/// </summary>
	public sealed class NullLogger: ILogger
	{
		public void Puts(LogType logType, string title, object detail, int frame = 2)
		{
			Debug.WriteLine("{0}: {1}, {2}", logType, title, detail);
		}

		public void PutsDebug(string title, Func<object> detail, int frame = 3)
		{
			Puts(LogType.Debug, title, detail(), frame);
		}
	}
}
