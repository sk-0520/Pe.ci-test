namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility.SettingUtilityImplement
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;

	internal static class InitializeNoteIndexSetting
	{
		public static void Correction(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{
			V_First(setting, previousVersion, nonProcess);
			V_Last(setting, previousVersion, nonProcess);
		}

		static void V_Last(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{ }

		static void V_First(NoteIndexSettingModel setting, Version previousVersion, INonProcess nonProcess)
		{ }
	}
}
