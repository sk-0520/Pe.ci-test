﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
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

			item.LauncherKind = EnumUtility.GetNormalization(item.LauncherKind, LauncherKind.File);
		}

		static void V_First(LauncherItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

		}
	}
}
