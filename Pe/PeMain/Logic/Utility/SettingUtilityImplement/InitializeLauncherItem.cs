namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	internal static class InitializeLauncherItem
	{
		public static void Correction(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			V_First(item, previousVersion, nonProcess);
			V_Last(item, previousVersion, nonProcess);
		}

		static void V_Last(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			item.LauncherKind = EnumUtility.GetNormalization(item.LauncherKind, LauncherKind.File);
			// あるだけ
			if (item.LauncherKind == LauncherKind.Directory) {
				item.LauncherKind = LauncherKind.File;
			}

			if(SettingUtility.IsIllegalString(item.Command)) {
				item.Command = string.Empty;
			}
		}

		static void V_First(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			item.LauncherKind = LauncherKind.File;
		}
	}
}
