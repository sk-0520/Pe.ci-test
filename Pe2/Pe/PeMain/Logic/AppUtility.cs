namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
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
		public static LanguageCollectionModel LoadLanguageFile(string baseDir, string name, string code, out string loadFilePath)
		{
			var langItems = Directory.EnumerateFiles(baseDir, Constants.languageSearchPattern)
				.Select(f => new {
					Item = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(f),
					Path = f,
				})
				.ToArray();
			;
			var lang = langItems.FirstOrDefault(l => l.Item.Name == name)
				?? langItems.FirstOrDefault(l => l.Item.Code == code)
				?? new {
					Item = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(Path.Combine(baseDir, Constants.languageDefaultFileName)),
					Path = Path.Combine(baseDir, Constants.languageDefaultFileName)
				}
			;
			loadFilePath = lang.Path;
			return lang.Item;
		}

		/// <summary>
		/// ログ取りくん作成。
		/// <para>UI・設定に影響されない</para>
		/// </summary>
		/// <param name="outputFile"></param>
		/// <param name="path"></param>
		/// <returns></returns>
		public static SystemLogger CreateSystemLogger(bool outputFile, string baseDir)
		{
			var logger = new SystemLogger();

			if(outputFile) {
				logger.LoggerConfig.PutsFile = true;
				var filePath = PathUtility.AppendExtension(Path.Combine(baseDir, DateTime.Now.ToString(Constants.timestampFileName)), "log");
				FileUtility.MakeFileParentDirectory(filePath);
				logger.FilePath = filePath;
			}

			return logger;
		}
	}
}
