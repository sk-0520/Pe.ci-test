namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	internal static class InitializeLauncherGroupSetting
	{
		public static void Correction(LauncherGroupSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(LauncherGroupSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(!setting.Groups.Any()) {
				var initGroup = SettingUtility.CreateLauncherGroup(setting.Groups, nonProcess);

				setting.Groups.Add(initGroup);
			}
		}

		static void V_First(LauncherGroupSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}
		}
	}
}
