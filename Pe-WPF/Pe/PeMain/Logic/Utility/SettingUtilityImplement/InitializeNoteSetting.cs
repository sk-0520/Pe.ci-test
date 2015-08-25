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

	internal static class InitializeNoteSetting
	{
		public static void Correction(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			setting.Font.Size = Constants.noteFontSize.GetClamp(setting.Font.Size);

			if(setting.ForeColor == default(Color)) {
				setting.ForeColor = Constants.noteColor.ForeColor;
			}
			if(setting.BackColor == default(Color)) {
				setting.BackColor = Constants.noteColor.BackColor;
			}
		}

		static void V_First(NoteSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			if(previousVersion != null) {
				return;
			}

			setting.Font.Size = Constants.noteFontSize.median;
			setting.ForeColor = Constants.noteColor.ForeColor;
			setting.BackColor = Constants.noteColor.BackColor;
		}
	}
}
