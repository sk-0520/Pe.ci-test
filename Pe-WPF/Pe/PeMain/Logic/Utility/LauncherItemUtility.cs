namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public static class LauncherItemUtility
	{
		public static BitmapSource GetIcon(LauncherItemModel model, IconScale iconScale, INonProcess nonProcess)
		{
			CheckUtility.DebugEnforceNotNull(model);
			CheckUtility.DebugEnforceNotNull(model.Icon);
			CheckUtility.DebugEnforceNotNull(nonProcess);

			var hasIcon = false;
			var useIcon = new IconPathModel();

			if(!string.IsNullOrWhiteSpace(model.Icon.Path)) {
				var expandIconPath = Environment.ExpandEnvironmentVariables(model.Icon.Path);
				hasIcon = FileUtility.Exists(expandIconPath);
				if(hasIcon) {
					useIcon.Path = expandIconPath;
					useIcon.Index = model.Icon.Index;
				}
			}
			if(!hasIcon) {
				if(!string.IsNullOrWhiteSpace(model.Command)) {
					var expandCommandPath = Environment.ExpandEnvironmentVariables(model.Command);
					hasIcon = FileUtility.Exists(expandCommandPath);
					if(hasIcon) {
						useIcon.Path = expandCommandPath;
						useIcon.Index = 0;
					}
				}
				if(!hasIcon && model.LauncherKind == LauncherKind.Command) {
					return AppResource.GetLauncherCommandIcon(iconScale, nonProcess.Logger);
				}
			}

			if(hasIcon) {
				return AppUtility.LoadIconDefault(useIcon, iconScale, nonProcess.Logger);
			} else {
				return AppResource.GetNotFoundIcon(iconScale, nonProcess.Logger);
			}
		}

		/// <summary>
		/// コマンド選択用ファイルダイアログお表示する。
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <param name="nonProcess"></param>
		/// <returns>選択されたファイル。未選択の場合は null 。</returns>
		public static string ShowOpenCommandDialog(string defaultPath, INonProcess nonProcess)
		{
			return DialogUtility.ShowOpenSingleFileDialog(defaultPath);
		}

		/// <summary>
		/// オプション選択用ファイルダイアログを表示する。
		/// </summary>
		/// <param name="defaultPath"></param>
		/// <returns>選択されたファイル群をまとめた文字列。未選択の場合は null 。</returns>
		public static string ShowOpenOptionDialog(string defaultPath)
		{
			var files = DialogUtility.ShowOpenMultiFileDialog(defaultPath);
			if(files != null) {
				return string.Join(" ", TextUtility.WhitespaceToQuotation(files));
			} else {
				return null;
			}
		}
	}
}
