/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/15
 * 時刻: 15:03
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Runtime.Serialization;

namespace PeMain.Data
{
	/// <summary>
	/// なんかを保持するアイテムの基底。
	/// </summary>
	[Serializable]
	public abstract class Item
	{
	}
	
	/// <summary>
	/// 名前付きアイテム
	/// </summary>
	[Serializable]
	public abstract class NameItem: Item
	{
		/// <summary>
		/// 名前
		/// </summary>
		[System.Xml.Serialization.XmlAttribute("Name")]
		public string Name { get; set; }
	}
}
