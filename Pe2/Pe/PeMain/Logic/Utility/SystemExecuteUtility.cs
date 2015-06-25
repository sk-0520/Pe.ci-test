namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public static class SystemExecuteUtility
	{
		public static Process RunDLL(string command, INonProcess nonProcess)
		{
			var rundll = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "rundll32.exe");
			var startupInfo = new ProcessStartInfo(rundll, command);

			return Process.Start(startupInfo);
		}

		/// <summary>
		/// タスクトレイ通知領域履歴を開く。
		/// </summary>
		/// <param name="nonProcess"></param>
		public static void OpenNotificationAreaHistory(INonProcess nonProcess)
		{
			RunDLL("shell32.dll,Options_RunDLL 5", nonProcess);
		}
	}
}
