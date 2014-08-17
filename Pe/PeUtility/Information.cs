/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 03/07/2014
 * 時刻: 00:34
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Windows.Forms;

namespace PeUtility
{
	public class InfoGroup
	{
		public InfoGroup(string title)
		{
			Title = title;
			Items = new Dictionary<string, object>();
		}
			
		public string Title { get; private set; }
		public Dictionary<string, object> Items { get; private set; }
		
	}
	
	/// <summary>
	/// 各種情報を取得する。
	/// </summary>
	public class Information
	{
		protected virtual InfoGroup GetMemory()
		{
			var result = new InfoGroup("memory");
			
			return result;
		}
		
		public virtual IEnumerable<InfoGroup> Get()
		{
			return new [] {
				GetMemory(),
			};
		}
	}
}
