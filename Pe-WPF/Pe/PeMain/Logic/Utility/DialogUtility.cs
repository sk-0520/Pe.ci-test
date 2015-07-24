namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;

	public static class DialogUtility
	{
		/// <summary>
		/// ディレクトリを開く。
		/// </summary>
		/// <param name="defPath">初期パス</param>
		/// <returns>選択されたディレクトリパス。未選択の場合は null 。</returns>
		public static string OpenDirectoryDialog(string defPath)
		{
			using(var dialog = new FolderBrowserDialog()) {
				var expandedPath = Environment.ExpandEnvironmentVariables(defPath);
				if(Directory.Exists(expandedPath)) {
					dialog.SelectedPath = expandedPath;
				}
				var dialogResult = dialog.ShowDialog();
				if(dialogResult.GetValueOrDefault()) {
					return dialog.SelectedPath;
				} else {
					return null;
				}
			}
		}

	}
}
