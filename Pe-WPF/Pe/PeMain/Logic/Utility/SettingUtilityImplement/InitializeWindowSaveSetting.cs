namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeWindowSaveSetting
	{
		public static void Correction(WindowSaveSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);

			setting.SaveCount = Constants.windowSaveCount.GetClamp(setting.SaveCount);
			setting.SaveIntervalTime = Constants.windowSaveIntervalTime.GetClamp(setting.SaveIntervalTime);
		}

		static void V_First(WindowSaveSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			setting.IsEnabled = true;
			setting.SaveCount = Constants.windowSaveCount.median;
			setting.SaveIntervalTime = Constants.windowSaveIntervalTime.median;
		}
	}
}
