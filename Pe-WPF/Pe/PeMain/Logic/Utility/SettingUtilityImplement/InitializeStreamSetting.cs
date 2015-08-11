namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeStreamSetting
	{
		internal static void Correction(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
		}

		private static void V_First(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			setting.Font.Size = Constants.streamFontSize.median;
			Constants.streamOutputColor.DeepCloneTo(setting.OutputColor);
			Constants.streamErrorColor.DeepCloneTo(setting.ErrorColor);
		}
	}
}
