﻿/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 17:59
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Xml;
using Pe.IF;
using ShareLib;

namespace Pe.Logic
{
	public class DataElement
	{
		public static string Tag { get { return "data"; } }
		public static string Attribute { get { return "value"; } }
		//public string Value { get; set; }
	}
	/// <summary>
	/// アイテムが格納する構造化可能なデータ
	/// 
	/// 使用する基準はXMLのタグに対して属性じゃ見栄え悪くなりそうな場合
	/// </summary>
	public abstract class ItemData: ItemBase
	{
		public ItemData()
		{
			Debug.Assert(Name != "item");
		}
	}
}
