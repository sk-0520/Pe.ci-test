namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using Microsoft.Win32;

	public static class DialogUtility
	{
		static IEnumerable<string> ShowOpenFileDialog(string defaultPath, bool multiSelect, DialogFilterList filter)
		{
			var tempPath = Environment.ExpandEnvironmentVariables(defaultPath);
			var usingFilePath = File.Exists(tempPath) ? tempPath : string.Empty;

			var dialog = new OpenFileDialog() {
				CheckFileExists = true,
				FileName = usingFilePath,
				Multiselect = multiSelect,
			};
			if(!string.IsNullOrWhiteSpace(usingFilePath)) {
				dialog.InitialDirectory = Path.GetDirectoryName(usingFilePath);
			}
			if(filter != null) {
				dialog.Filter = filter.FilterText;
			}

			var dialogResult = dialog.ShowDialog();
			if(dialogResult.GetValueOrDefault()) {
				if(multiSelect) {
					return dialog.FileNames;
				} else {
					return new[] {dialog.FileName};
				}
			} else {
				return null;
			}
		}

		/// <summary>
		/// 単独選択ファイルオープンダイアログを表示する。
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <param name="filter"></param>
		/// <returns>選択されたファイル。未選択の場合は null 。</returns>
		public static string ShowOpenSingleFileDialog(string defaultPath, DialogFilterList filter = null)
		{
			var result = ShowOpenFileDialog(defaultPath, false, filter);
			if(result != null) {
				return result.FirstOrDefault();
			} else {
				return null;
			}
		}

		/// <summary>
		/// 複数選択可能なファイルオープンダイアログを表示する。
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <param name="filter"></param>
		/// <returns>選択されたファイル群。未選択の場合は null 。</returns>
		public static IEnumerable<string> ShowOpenMultiFileDialog(string defaultPath, DialogFilterList filter = null)
		{
			return ShowOpenFileDialog(defaultPath, true, filter);
		}

		/// <summary>
		/// ディレクトリダイアログを表示する。
		/// </summary>
		/// <param name="defaultPath">初期パス</param>
		/// <returns>選択されたディレクトリパス。未選択の場合は null 。</returns>
		public static string ShowDirectoryDialog(string defaultPath)
		{
			var usingFilePath = Directory.Exists(defaultPath) ? defaultPath : string.Empty;

			using(var dialog = new FolderBrowserDialog()) {
				var expandedPath = Environment.ExpandEnvironmentVariables(usingFilePath);
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
