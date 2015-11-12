namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Windows.Forms;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Utility;

	/// <summary>
	/// ワード。
	/// </summary>
	[Serializable]
	public class Word: NameItem
	{
		/// <summary>
		/// 名前(キー)に対するテキスト。
		/// </summary>
		[System.Xml.Serialization.XmlAttribute]
		public string Text { get; set; }
	}
	
	/// <summary>
	/// 言語データ。
	/// </summary>
	[Serializable]
	public class Language: NameItem
	{
		Tuple<string, string> _rangeApp = new Tuple<string, string>("@[", "]");
		Tuple<string, string> _rangeReplace = new Tuple<string, string>("${", "}");
		
		public Language()
		{
			BaseName = "unknown";
			
			Define = new List<Word>();
			Words = new List<Word>();
		}
		
		public Tuple<string, string> RangeApp { get { return this._rangeApp; } }
		public Tuple<string, string> RangeReplace { get { return this._rangeReplace; } }
		
		/// <summary>
		/// 現在の言語を指す名称。
		/// </summary>
		[XmlIgnore]
		public string BaseName { get; set; }
		/// <summary>
		/// 許諾ダイアログで使用するHTMLファイル名。
		/// </summary>
		public string AcceptFileName { get { return string.Format("{0}.accept.html", BaseName); } }
		///// <summary>
		///// クリップボードテンプレートで使用するHTMLファイル名。
		///// </summary>
		//public string TemplateFileName { get { return string.Format("{0}.template.html", BaseName); } }

		/// <summary>
		/// 定義済みワード一覧。
		/// </summary>
		public List<Word> Define { get; set; }
		/// <summary>
		/// ワード一覧。
		/// </summary>
		public List<Word> Words  { get; set; }
		
		/// <summary>
		/// 言語コード。
		/// </summary>
		[System.Xml.Serialization.XmlAttribute]
		public string Code { get; set; }
		
		/// <summary>
		/// キーからワードを取得。
		/// </summary>
		/// <param name="list"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		private Word GetWord(IEnumerable<Word> list, string key)
		{
			var word = list.SingleOrDefault(item => item.Name == key);
			
			if(word == null) {
				word = new Word();
				word.Name = key;
				word.Text = key;
			}
			
			return word;
		}
		
		/// <summary>
		/// キーからテキスト取得。
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public string GetPlain(string key)
		{
			var word = GetWord(Words, key);
			
			return word.Text;
		}
		/// <summary>
		/// システム定義済み置き換えマップ。
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, string> GetAppMap()
		{
			var nowDateTime = DateTime.Now;
			var systemMap = new Dictionary<string, string>() {
				{ AppLanguageName.application,    Literal.programName },
				{ AppLanguageName.versionFull,    Literal.ApplicationVersion },
				{ AppLanguageName.versionNumber,  Literal.Version.FileVersion },
				{ AppLanguageName.versionHash,    Literal.Version.ProductVersion },
				//　#199
				{ AppLanguageName.versionNumberOld,  Literal.Version.FileVersion },
				{ AppLanguageName.versionHashOld,    Application.ProductVersion },
				{ AppLanguageName.versionFullOld,    Literal.ApplicationVersion },

				{ AppLanguageName.timestamp,      nowDateTime.ToString() },
				{ AppLanguageName.year,           nowDateTime.Year.ToString() },
				{ AppLanguageName.year04,         nowDateTime.Year.ToString("D4") },
				{ AppLanguageName.month,          nowDateTime.Month.ToString() },
				{ AppLanguageName.month02,        nowDateTime.Month.ToString("D2") },
				{ AppLanguageName.monthShortName, nowDateTime.ToString("MMM") },
				{ AppLanguageName.monthLongName,  nowDateTime.ToString("MMMM") },
				{ AppLanguageName.day,            nowDateTime.Day.ToString() },
				{ AppLanguageName.day02,          nowDateTime.Day.ToString("D2") },
				{ AppLanguageName.hour,           nowDateTime.Hour.ToString() },
				{ AppLanguageName.hour02,         nowDateTime.Hour.ToString("D2") },
				{ AppLanguageName.minute,         nowDateTime.Minute.ToString() },
				{ AppLanguageName.minute02,       nowDateTime.Minute.ToString("D2") },
				{ AppLanguageName.second,         nowDateTime.Second.ToString() },
				{ AppLanguageName.second02,       nowDateTime.Second.ToString("D2") },
			};
			
			return systemMap;
		}
		
		/// <summary>
		/// 変換済み文字列の取得。
		/// 
		/// 定義済み文字列は展開される。
		/// </summary>
		public string this[string key, IDictionary<string, string> map = null]
		{
			get
			{
				var text = GetPlain(key);

				//if(text.Any(c => c == '@')) {
					var systemMap = GetAppMap();
					IDictionary<string, string> usingMap;
					if(map == null) {
						usingMap = systemMap;
					} else {
						usingMap = systemMap;
						foreach(var pair in map) {
							usingMap[pair.Key] = pair.Value;
						}
					}
					text = text.ReplaceRangeFromDictionary(RangeApp.Item1, RangeApp.Item2, usingMap);
				//}
				
				//if(text.Any(c => c == '$')) {
					// ${...}
					text = text.ReplaceRange(RangeReplace.Item1, RangeReplace.Item2, s => GetWord(Define, s).Text);
				//}
				
				return text;
			}
		}
		
		public string ReplaceAllLanguage(string text)
		{
			return text.ReplaceRange(RangeReplace.Item1, RangeReplace.Item2, s => this[s]);
		}

		public string ReplaceAllAppMap(string text, IDictionary<string, string> map, Tuple<char, char> evil)
		{
			var systemMap = GetAppMap();
			IDictionary<string, string> usingMap;
			usingMap = systemMap;
			foreach(var pair in map) {
				usingMap[pair.Key] = pair.Value;
			}
			if(evil != null) {
				usingMap = usingMap.ToDictionary(pair => pair.Key, pair => evil.Item1 + pair.Value + evil.Item2);
			}
			
			var replacedAppMap = text.ReplaceRangeFromDictionary(RangeApp.Item1, RangeApp.Item2, usingMap);
			//System.Diagnostics.Debug.WriteLine(BitConverter.ToString(System.Text.Encoding.UTF8.GetBytes(replacedAppMap)));
			//var replacedUserMap = replacedAppMap.ReplaceRangeFromDictionary("#(", ")", map);
			//var replacedLang = replacedUserMap.ReplaceRange(RangeReplace.Item1, RangeReplace.Item2, s => GetWord(Words, s).Text);
			//var replacedLang = replacedAppMap.ReplaceRange(RangeReplace.Item1, RangeReplace.Item2, s => this[s, usingMap]);

			return replacedAppMap;
		}

	}
}
