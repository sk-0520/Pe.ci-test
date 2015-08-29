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
				{ "APPLICATION",          Constants.ApplicationName },
				{ "APPLICATION:VERSION",  Constants.ApplicationVersion },
				{ "APPLICATION:REVISION", Constants.ApplicationRevision },

				{ "TIMESTAMP",            dateTime.ToString() },
				{ "Y",                    dateTime.Year.ToString() },
				{ "YYYY",                 dateTime.Year.ToString("D4") },
				{ "M",                    dateTime.Month.ToString() },
				{ "MM",                   dateTime.Month.ToString("D2") },
				{ "MMM",                  dateTime.ToString("MMM") },
				{ "MMMM",                 dateTime.ToString("MMMM") },
				{ "D",                    dateTime.Day.ToString() },
				{ "DD",                   dateTime.Day.ToString("D2") },
				{ "h",                    dateTime.Hour.ToString() },
				{ "hh",                   dateTime.Hour.ToString("D2") },
				{ "m",                    dateTime.Minute.ToString() },
				{ "mm",                   dateTime.Minute.ToString("D2") },
				{ "s",                    dateTime.Second.ToString() },
				{ "ss",                   dateTime.Second.ToString("D2") },
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
