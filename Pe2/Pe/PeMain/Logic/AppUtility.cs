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
		/// <param name="baseDir"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public static LanguageCollectionModel LoadLanguageFile(string baseDir, string name)
		{
			var la = new LanguageCollectionModel();
			la.Name = "日本語";
			la.Code = "ja-JP";
			la.Define.Add(new LanguageItemModel() { Id = "common/ok", Word = "OK" });
			la.Define.Add(new LanguageItemModel() { Id = "common/cancel", Word = "Cancel" });
			la.Words.Add(new LanguageItemModel() { Id = "window/setting", Word = "setting" });
			la.Words.Add(new LanguageItemModel() { Id = "tasktray/tooltip", Word = "ぴーいー" });
			SerializeUtility.SaveXmlSerializeToFile(@"Z:\a.xml", la);
			return Directory.EnumerateFiles(baseDir, "*.xml")
				.Select(f => SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(f))
				.FirstOrDefault(l => l.Name == name)
			;
		}
	}
}
