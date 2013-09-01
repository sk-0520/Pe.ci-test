/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/09/01
 * 時刻: 19:33
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;

namespace Pe.Logic
{
	/// <summary>
	/// Description of TagItemData.
	/// </summary>
	public class TagItemData: ItemData
	{
		public TagItemData()
		{
			Tags = new List<string>();
		}
		
		public List<string> Tags { get; set; }
	}
}
