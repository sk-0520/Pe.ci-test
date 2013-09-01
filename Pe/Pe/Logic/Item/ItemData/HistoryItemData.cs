/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 19:25
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Xml;
using Pe.IF;

namespace Pe.Logic
{
	public class HistoryItemData: ItemData
	{
		public HistoryItemData()
		{
			WorkDirectory = new List<string>();
			OptionCommand = new List<string>();
		}
		
		public int ExecuteCount { get; set; }
		public List<string> WorkDirectory { get; private set; }
		public List<string> OptionCommand { get; private set; }
	}
}
