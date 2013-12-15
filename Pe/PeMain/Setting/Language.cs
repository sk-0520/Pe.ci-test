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
		public Language()
		{
			Define = new Dictionary<string, Word>();
			Common = new Dictionary<string, Word>();
		}
		public Dictionary<string, Word> Define { get; private set; }
		public Dictionary<string, Word> Common { get; private set; }
		
		[System.Xml.Serialization.XmlAttribute("Code")]
		public string Code { get; set; }
		
	}
}
