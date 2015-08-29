namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class AppLanguageManager: LanguageManager
	{
		#region static

		public static IDictionary<string, string> GetAppMap(DateTime dateTime, ILanguage language)
		{
			var appMap = new Dictionary<string, string>() {
				{ LanguageKey.application,          Constants.ApplicationName },
				{ LanguageKey.applicationVersion,  Constants.ApplicationVersion },
				{ LanguageKey.applicationRevision, Constants.ApplicationRevision },

				{ LanguageKey.timestamp,      dateTime.ToString() },
				{ LanguageKey.year,           dateTime.Year.ToString() },
				{ LanguageKey.year04,         dateTime.Year.ToString("D4") },
				{ LanguageKey.month,          dateTime.Month.ToString() },
				{ LanguageKey.month02,        dateTime.Month.ToString("D2") },
				{ LanguageKey.monthShortName, dateTime.ToString("MMM") },
				{ LanguageKey.monthLongName,  dateTime.ToString("MMMM") },
				{ LanguageKey.day,            dateTime.Day.ToString() },
				{ LanguageKey.day02,          dateTime.Day.ToString("D2") },
				{ LanguageKey.hour,           dateTime.Hour.ToString() },
				{ LanguageKey.hour02,         dateTime.Hour.ToString("D2") },
				{ LanguageKey.minute,         dateTime.Minute.ToString() },
				{ LanguageKey.minute02,       dateTime.Minute.ToString("D2") },
				{ LanguageKey.second,         dateTime.Second.ToString() },
				{ LanguageKey.second02,       dateTime.Second.ToString("D2") },
			};

			return appMap;
		}

		#endregion

		public AppLanguageManager(LanguageCollectionModel model, string languageFilePath)
			:base(model, languageFilePath)
		{
			BaseFileName = Path.GetFileNameWithoutExtension(LanguageFilePath);
			BaseDirectoryPath = Path.GetDirectoryName(LanguageFilePath);
		}

		#region property

		public string BaseFileName { get; private set; }
		public string BaseDirectoryPath { get; private set; }
		public string AcceptDocumentFilePath { get { return Path.Combine(BaseDirectoryPath, PathUtility.AppendExtension(BaseFileName, Constants.languageAcceptDocumentExtension)); } }

		#endregion 

		#region LanguageManager

		protected override IDictionary<string, string> GetSystemMap(DateTime dateTime)
		{
			var baseMap = base.GetSystemMap(dateTime);

			var appMap = GetAppMap(dateTime, this);

			foreach(var pair in appMap) {
				baseMap.Add(pair);
			}

			return baseMap;
		}

		#endregion
	}
}
