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

	internal static class InitializeLauncherGroupItem
	{
		public static void Correction(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			V_First(item, previousVersion, nonProcess);
			V_Last(item, previousVersion, nonProcess);
		}

		static void V_Last(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			item.GroupKind = EnumUtility.GetNormalization(item.GroupKind, GroupKind.LauncherItems);
		}

		static void V_First(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			item.GroupKind = GroupKind.LauncherItems;
		}
	}
}
