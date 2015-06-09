namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
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
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
	using ContentTypeTextNet.Pe.Library.PeData.Setting;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.ViewModel;

	public static class AppUtility
	{
		public static T LoadSetting<T>(string path, ILogger logger)
			where T: SettingModelBase, new()
		{
			logger.Information("load setting", path);
			T result = null;
			if(File.Exists(path)) {
				result = SerializeUtility.LoadJsonDataFromFile<T>(path);
				logger.Debug("load data", result != null ? typeof(T).Name: "null");
			} else {
				logger.Debug("file not found", path);
			}

			return result ?? new T();
		}

		public static void SaveSetting<T>(string path, T model, ILogger logger)
			where T: SettingModelBase
		{
			logger.Information("load setting", path);
			SerializeUtility.SaveJsonDataToFile(path, model);
		}

		/// <summary>
		/// 指定ディレクトリ内から指定した言語名の言語ファイルを取得する。
		/// </summary>
		/// <param name="baseDir">検索ディレクトリ</param>
		/// <param name="name">検索名</param>
		/// <param name="code">検索コード</param>
		/// <returns></returns>
		public static AppLanguageManager LoadLanguageFile(string baseDir, string name, string code, ILogger logger)
		{
			logger.Information("load language file", baseDir);
			var langPairList = new List<KeyValuePair<string, LanguageCollectionModel>?>();
			foreach(var path in Directory.EnumerateFiles(baseDir, Constants.languageSearchPattern)) {
				try {
					var model = SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(path);
					var pair = new KeyValuePair<string, LanguageCollectionModel>(path, model);
					langPairList.Add(pair);
				} catch(Exception ex) {
					logger.Error(ex);
				}
			}

			var defaultPath = Path.Combine(baseDir, Constants.languageDefaultFileName);
			var lang = langPairList.FirstOrDefault(p => p.Value.Value.Name == name)
				?? langPairList.FirstOrDefault(l => l.Value.Value.Code == code)
				?? new KeyValuePair<string, LanguageCollectionModel>(defaultPath, SerializeUtility.LoadXmlSerializeFromFile<LanguageCollectionModel>(defaultPath))
			;
			return new AppLanguageManager(lang.Value, lang.Key);
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
