namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Define;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeCommandSetting
	{
		public static void Correction(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);

			setting.IconScale = EnumUtility.GetNormalization(setting.IconScale, IconScale.Small);
			setting.WindowWidth = Constants.commandWindowWidth.GetClamp(setting.WindowWidth);
			setting.Font.Size = Constants.commandFontSize.GetClamp(setting.Font.Size);
		}

		static void V_First(CommandSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			nonProcess.Logger.Trace("version setting: first");

			setting.IconScale = IconScale.Small;
			setting.WindowWidth = Constants.commandWindowWidth.median;
			setting.Font.Size = Constants.commandFontSize.median;
		}
	}
}
