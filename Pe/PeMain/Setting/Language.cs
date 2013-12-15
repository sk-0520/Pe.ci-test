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
using System.Linq;

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
		[System.Xml.Serialization.XmlAttribute("Text")]
		public string Text { get; set; }
	}
	
	/// <summary>
	/// Description of Language.
	/// </summary>
	[Serializable]
	public class Language: NameItem
	{
		private Dictionary<string, List<Word>> _map;
			
		public Language()
		{
			Define = new List<Word>();
			Common = new List<Word>();
			this._map = new Dictionary<string, List<Word>>() {
				{"Define", Define},
				{"Common", Common},
			};
		}
		public List<Word> Define { get; set; }
		public List<Word> Common { get; set; }
		
		[System.Xml.Serialization.XmlAttribute("Code")]
		public string Code { get; set; }
		
		private Word getWord(string group, string key)
		{
			var list = this._map[group];
			var word = list.SingleOrDefault(item => item.Name == key);
			if(word == null) {
				word = new Word();
				word.Name = key;
				word.Text = "<" + key + ">";
			}
			
			return word;
		}
		
	}
}
