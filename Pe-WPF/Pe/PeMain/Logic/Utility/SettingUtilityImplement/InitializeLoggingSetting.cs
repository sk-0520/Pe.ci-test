namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	public static class InitializeLoggingSetting
	{
		public static void Correction(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);

			if(SettingUtility.IsIllegalPlusNumber(setting.WindowWidth)) {
				setting.WindowWidth = Constants.loggingDefaultWindowSize.Width;
			}
			if(SettingUtility.IsIllegalPlusNumber(setting.WindowHeight)) {
				setting.WindowHeight = Constants.loggingDefaultWindowSize.Height;
			}
		}

		static void V_First(LoggingSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			nonProcess.Logger.Trace("version setting: first");

			setting.WindowWidth = Constants.loggingDefaultWindowSize.Width;
			setting.WindowHeight = Constants.loggingDefaultWindowSize.Height;
		}
	}
}
