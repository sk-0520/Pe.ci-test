namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeClipboardSetting
	{
		public static void Correction(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			setting.WaitTime = Constants.clipboardWaitTime.GetClamp(setting.WaitTime);
			setting.DuplicationCount = Constants.clipboardDuplicationCount.GetClamp(setting.DuplicationCount);
			setting.Font.Size = Constants.clipboardFontSize.GetClamp(setting.Font.Size);

			if(SettingUtility.IsIllegalPlusNumber(setting.ItemsListWidth)) {
				setting.ItemsListWidth = Constants.clipboardItemsListWidth;
			}

			if(SettingUtility.IsIllegalPlusNumber(setting.WindowWidth)) {
				setting.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
			}
			if(SettingUtility.IsIllegalPlusNumber(setting.WindowHeight)) {
				setting.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
			}
		}

		static void V_First(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			nonProcess.Logger.Trace("version setting: first");

			setting.IsEnabled = true;
			setting.EnabledClipboardTypes = ClipboardType.All;
			setting.UsingClipboard = false;
			setting.SaveCount = 0;
			setting.DuplicationCount = Constants.clipboardDuplicationCount.median;
			setting.WaitTime = Constants.clipboardWaitTime.median;
			setting.Font.Size = Constants.clipboardFontSize.median;
			setting.ItemsListWidth = Constants.clipboardItemsListWidth;
			setting.WindowWidth = Constants.clipboardDefaultWindowSize.Width;
			setting.WindowHeight = Constants.clipboardDefaultWindowSize.Height;
		}
	}
}
