/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.Data
{
	/// <summary>
	/// 作成・更新日時保持
	/// </summary>
	[Serializable]
	public class DateHistory: Item, ICloneable
	{
		public DateHistory()
		{
			Create = DateTime.Now;
			Update = DateTime.Now;
		}
		public DateTime Create { get; set; }
		public DateTime Update { get; set; }
		
		public object Clone()
		{
			var result = new DateHistory();
			
			result.Create = Create;
			result.Update = Update;
			
			return result;
		}
	}
}
