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
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
		public static MainWorkerViewModel CreateMainWorkerViewModel(VariableConstants constants)
		{
			return new MainWorkerViewModel(constants);
		}

		public static T LoadSetting<T>(string path)
			where T: SettingModelBase, new()
		{
			T result = null;
			if(File.Exists(path)) {
				result = SerializeUtility.LoadJsonDataFromFile<T>(path);
			}

			return result ?? new T();
		}

		public static void SaveSetting<T>(string path, T model)
			where T: SettingModelBase
		{
			SerializeUtility.SaveJsonDataToFile(path, model);
		}

		/// <summary>
		/// 指定ディレクトリ内から指定した言語名の言語ファイルを取得する。
		/// </summary>
		/// <param name="baseDir">検索ディレクトリ</param>
		/// <param name="name">検索名</param>
		/// <param name="code">検索コード</param>
		/// <returns></returns>
		public static LanguageCollectionModel LoadLanguageFile(string baseDir, string name, string code)
		{
			var langItems = Directory.EnumerateFiles(baseDir, Constants.languageSearchPattern)
				.Select(f => SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(f))
				.ToArray();
			;
			return langItems.FirstOrDefault(l => l.Name == name)
				?? langItems.FirstOrDefault(l => l.Code == code)
				?? SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(Path.Combine(baseDir, Constants.languageDefaultFileName))
			;
		}
	}
}
