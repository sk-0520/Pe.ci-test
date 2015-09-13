namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Media;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;

	internal static class InitializeStreamSetting
	{
		internal static void Correction(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(StreamSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			setting.Font.Size = Constants.streamFontSize.GetClamp(setting.Font.Size);

			if(setting.OutputColor.ForeColor == default(Color)) {
				setting.OutputColor.ForeColor = Constants.streamOutputColor.ForeColor;
			}
			if(setting.OutputColor.BackColor == default(Color)) {
				setting.OutputColor.BackColor = Constants.streamOutputColor.BackColor;
			}

			if(setting.ErrorColor.ForeColor == default(Color)) {
				setting.ErrorColor.ForeColor = Constants.streamErrorColor.ForeColor;
			}
			if(setting.ErrorColor.BackColor == default(Color)) {
				setting.ErrorColor.BackColor = Constants.streamErrorColor.BackColor;
			}
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
