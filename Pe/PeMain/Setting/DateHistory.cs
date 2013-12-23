/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2013/12/23
 * 時刻: 13:53
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;

namespace PeMain.Setting
{
	/// <summary>
	/// 作成・更新日時保持(UTC)
	/// </summary>
	[Serializable]
	public class DateHistory: Item
	{
		public DateHistory()
		{
			Create = DateTime.UtcNow;
			Update = DateTime.UtcNow;
		}
		public DateTime Create { get; set; }
		public DateTime Update { get; set; }
	}
}
