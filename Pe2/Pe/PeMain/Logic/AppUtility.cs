namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
		public static MainWorkerViewModel CreateMainWorkerViewModel(Constants constants)
		{
			return new MainWorkerViewModel(constants);
		}

		public static T LoadSetting<T>(string path)
			where T : SettingModelBase, new()
		{
			return SerializeUtility.LoadXmlDataFromFile<T>(path);
		}
	}
}
