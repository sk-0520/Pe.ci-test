﻿namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	internal static class InitializeLauncherGroupItem
	{
		public static void Correction(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			V_First(item, previousVersion, nonProcess);
		}

		static void V_First(LauncherGroupItemModel item, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}
		}
	}
}
