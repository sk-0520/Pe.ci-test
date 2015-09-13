namespace ContentTypeTextNet.Library.SharedLibrary.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Library.SharedLibrary.ViewModel;

	/// <summary>
	/// 
	/// </summary>
	public class LanguageManager: SingleModelWrapperViewModelBase<LanguageCollectionModel>, ILanguage
	{
		public LanguageManager(LanguageCollectionModel model, string languageFilePath)
			: base(model)
		{
			LanguageFilePath = languageFilePath;
		}

		#region property

		public string LanguageFilePath { get; private set; }

		public string CultureCode { get { return Model.CultureCode; } }

		#endregion

		#region function

		static LanguageItemModel GetItem(IList<LanguageItemModel> list, string key)
		{
			var result = list
				.FirstOrDefault(l => l.Id == key)
				?? new LanguageItemModel() {
					Id = key,
					Word = key,
				}
			; 

			return result;
		}

		/// <summary>
		/// キーからテキスト取得。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetPlainText(string key)
		{
			var item = GetItem(Model.Words, key);
			return item.Word;
		}

		/// <summary>
		/// システム用
		/// </summary>
		/// <param name="dateTime"></param>
		/// <returns></returns>
		protected virtual IDictionary<string, string> GetSystemMap(DateTime dateTime)
		{
			return new Dictionary<string, string>();

			//return new Dictionary<string, string>() {
			//	{ "TIMESTAMP",  dateTime.ToString() },
			//	{ "Y",          dateTime.Year.ToString() },
			//	{ "YYYY",       dateTime.Year.ToString("D4") },
			//	{ "M",          dateTime.Month.ToString() },
			//	{ "MM",         dateTime.Month.ToString("D2") },
			//	{ "MMM",        dateTime.ToString("MMM") },
			//	{ "MMMM",       dateTime.ToString("MMMM") },
			//	{ "D",          dateTime.Day.ToString() },
			//	{ "DD",         dateTime.Day.ToString("D2") },
			//	{ "h",          dateTime.Hour.ToString() },
			//	{ "hh",         dateTime.Hour.ToString("D2") },
			//	{ "m",          dateTime.Minute.ToString() },
			//	{ "mm",         dateTime.Minute.ToString("D2") },
			//	{ "s",          dateTime.Second.ToString() },
			//	{ "ss",         dateTime.Second.ToString("D2") },
			//};
		}

		public string GetReplacedWordText(string words, DateTime dateTime, IReadOnlyDictionary<string, string> map)
		{
			var plainText = GetPlainText(words);

			var usingMap = GetSystemMap(dateTime);
			if(map != null && map.Any()) {
				foreach(var pair in map) {
					usingMap[pair.Key] = pair.Value;
				}
			}

			var replacedSystemMapText = plainText.ReplaceRangeFromDictionary("@[", "]", usingMap);
			var replacedDefineText = replacedSystemMapText.ReplaceRange("${", "}", s => GetItem(Model.Define, s).Word);

			return replacedDefineText;
		}

		#endregion

		#region indexer

		public string this[string key, IReadOnlyDictionary<string, string> map = null]
		{
			get { return GetReplacedWordText(key, DateTime.Now, map); }
		}

		#endregion
	}
}
