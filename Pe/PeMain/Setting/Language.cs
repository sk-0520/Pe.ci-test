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

namespace PeMain.Setting
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
		private const string DEFINE = "Define";
		private const string COMMON = "Common";
		
		private Dictionary<string, List<Word>> _map;
			
		public Language()
		{
			Define = new List<Word>();
			Common = new List<Word>();
			this._map = new Dictionary<string, List<Word>>() {
				{DEFINE, Define},
				{COMMON, Common},
			};
		}

		public List<Word> Define { get; set; }
		public List<Word> Common { get; set; }
		[System.Xml.Serialization.XmlAttribute]
		public string Code { get; set; }
		
		private Word getWord(string group, string name)
		{
			Word word = null;
			
			if(this._map.ContainsKey(group)) {
				var list = this._map[group];
				word = list.SingleOrDefault(item => item.Name == name);
			}
			
			if(word == null) {
				word = new Word();
				word.Name = name;
				word.Text = "<" + name + ">";
			}
			
			return word;
		}
		
		public string getPlain(string key)
		{
			var splitter = key.IndexOf('/');
			if(splitter == -1) {
				throw new ArgumentException(key);
			}
			var group = key.Substring(0, splitter);
			var name = key.Substring(splitter + 1);
			
			var word = getWord(group, name);
			
			return word.Text;
		}
		
		/// <summary>
		/// 変換済み文字列の取得。
		/// 
		/// 定義済み文字列は展開される。
		/// </summary>
		public string this[string key]
		{
			get 
			{
				var text = getPlain(key);
				if(text.Any(c => c == '$')) {
					// ${...}
					var replacedText = Regex.Replace(text, @"\$\{(.+)\}", (Match m) => 
						getWord(DEFINE, m.Groups[1].Value).Text
					);
					return replacedText;
				}
				
				return text;
			}
		}
	}
}
