/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 15:17
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

using PeUtility;

namespace PeMain.Data
{
	/// <summary>
	/// NonSerializedAttribute
	/// </summary>
	[Serializable]
	public class Word: NameItem
	{
		/// <summary>
		/// 
		/// </summary>
		[System.Xml.Serialization.XmlAttribute]
		public string Text { get; set; }
	}
	
	/// <summary>
	/// Description of Language.
	/// </summary>
	[Serializable]
	public class Language: NameItem
	{
		public Language()
		{
			Define = new List<Word>();
			Words = new List<Word>();
		}

		public List<Word> Define { get; set; }
		public List<Word> Words  { get; set; }
		
		[System.Xml.Serialization.XmlAttribute]
		public string Code { get; set; }
		
		private Word getWord(IEnumerable<Word> list, string key)
		{
			Word word = null;
			
			word = list.SingleOrDefault(item => item.Name == key);
			
			if(word == null) {
				word = new Word();
				word.Name = key;
				word.Text = "<" + key + ">";
			}
			
			return word;
		}
		
		public string getPlain(string key)
		{
			var word = getWord(Words, key);
			
			return word.Text;
		}
		
		private Dictionary<string, string> GetSystemMap()
		{
			var nowDateTime = DateTime.Now;
			var systemMap = new Dictionary<string, string>() {
				{ SystemLanguageName.application,  Literal.programName },
				{ SystemLanguageName.year,         nowDateTime.Year.ToString() },
				{ SystemLanguageName.year04,       nowDateTime.Year.ToString("D4") },
				{ SystemLanguageName.month,        nowDateTime.Month.ToString() },
				{ SystemLanguageName.month02,      nowDateTime.Month.ToString("D2") },
				{ SystemLanguageName.day,          nowDateTime.Day.ToString() },
				{ SystemLanguageName.day02,        nowDateTime.Day.ToString("D2") },
				{ SystemLanguageName.hour,         nowDateTime.Hour.ToString() },
				{ SystemLanguageName.hour02,       nowDateTime.Hour.ToString("D2") },
				{ SystemLanguageName.minute,       nowDateTime.Minute.ToString() },
				{ SystemLanguageName.minute02,     nowDateTime.Minute.ToString("D2") },
				{ SystemLanguageName.second,       nowDateTime.Second.ToString() },
				{ SystemLanguageName.second02,     nowDateTime.Second.ToString("D2") },
			};
			
			return systemMap;
		}
		
		/// <summary>
		/// 変換済み文字列の取得。
		/// 
		/// 定義済み文字列は展開される。
		/// </summary>
		public string this[string key, Dictionary<string, string> map = null]
		{
			get
			{
				var text = getPlain(key);
				if(text.Any(c => c == '$')) {
					// ${...}
					text = text.ReplaceRange("${", "}", s => getWord(Define, s).Text);
				}
				if(text.Any(c => c == '@')) {
					var systemMap = GetSystemMap();
					Dictionary<string, string> useMap;
					if(map == null) {
						useMap = systemMap;
					} else {
						useMap = map;
						foreach(var pair in systemMap) {
							useMap[pair.Key] = pair.Value;
						}
					}
					text = text.ReplaceRangeFromDictionary("@[", "]", useMap);
				}
				
				return text;
			}
		}
	}
}
