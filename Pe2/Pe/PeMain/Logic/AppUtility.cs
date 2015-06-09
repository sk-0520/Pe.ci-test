namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
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
			where T: SettingModelBase, new()
		{
			if(File.Exists(path)) {
				return SerializeUtility.LoadJsonDataFromFile<T>(path);
			}

			return new T();
		}

		public static void SaveSetting<T>(string path, T model)
			where T: SettingModelBase
		{
			SerializeUtility.SaveJsonDataToFile(path, model);
		}
	}
}
