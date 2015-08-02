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
		}

		static void V_First(ClipboardSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			nonProcess.Logger.Trace("version setting: first");

			setting.IsEnabled = true;
			setting.EnabledClipboardTypes = ClipboardType.All;
		}
	}
}
