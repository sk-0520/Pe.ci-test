namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class ExecuteUtility
	{
		/// <summary>
		/// コマンド文字列の実行。
		/// </summary>
		/// <param name="expandedPath">環境変数展開済みコマンド文字列。</param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		public static Process RunCommand(string expandedPath, INonProcess nonProcess)
		{
			return RunCommand(expandedPath, null, nonProcess);
		}

		/// <summary>
		/// コマンド文字列の実行。
		/// </summary>
		/// <param name="expandedPath">環境変数展開済みコマンド文字列。</param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		public static Process RunCommand(string expandedPath, string arguments, INonProcess nonProcess)
		{
			string exCommand = expandedPath;

			if (string.IsNullOrWhiteSpace(arguments)) {
				return Process.Start(exCommand);
			} else {
				return Process.Start(exCommand, arguments);
			}
		}

		/// <summary>
		/// ファイルパスを規定プログラムで開く。
		/// </summary>
		/// <param name="expandedFilePath">展開済みファイルパス</param>
		/// <param name="nonProcess"></param>
		/// <returns></returns>
		public static Process OpenFile(string expandedFilePath, INonProcess nonProcess)
		{
			return Process.Start(expandedFilePath); 
		}

		/// <summary>
		/// ディレクトリを開く。
		/// </summary>
		/// <param name="expandedDirPath">展開済みディレクトリパス</param>
		/// <param name="nonProcess"></param>
		/// <param name="openItem"></param>
		public static Process OpenDirectory(string expandedDirPath, INonProcess nonProcess, LauncherItemModel openItem)
		{
			return Process.Start(expandedDirPath);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="expandedFilePath"></param>
		/// <param name="nonProcess"></param>
		/// <param name="openItem"></param>
		/// <returns></returns>
		public static Process OpenDirectoryWithFileSelect(string expandedFilePath, INonProcess nonProcess, LauncherItemModel openItem)
		{
			if (FileUtility.Exists(expandedFilePath)) {
				var processName = "explorer.exe";
				var argument = string.Format("/select, {0}", expandedFilePath);
				return Process.Start(processName, argument);
			} else {
				var dirPath = Path.GetDirectoryName(expandedFilePath);
				return OpenDirectory(dirPath, nonProcess, openItem);
			}
		}

		/// <summary>
		/// プロパティを表示。
		/// </summary>
		/// <param name="expandedPath">展開済みパス</param>
		/// <param name="hWnd"></param>
		public static void OpenProperty(string expandedPath, IntPtr hWnd)
		{
			NativeMethods.SHObjectProperties(hWnd, SHOP.SHOP_FILEPATH, expandedPath, string.Empty);
		}

	}
}
