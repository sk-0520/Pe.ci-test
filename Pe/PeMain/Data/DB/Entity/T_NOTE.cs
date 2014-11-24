/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/10
 * 時刻: 23:00
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using PeUtility;

namespace PeMain.Data.DB
{
	[TargetName("T_NOTE")]
	public class TNoteEntity: CommonDataEntity
	{
		[TargetName("NOTE_ID", true)]
		public long Id { get; set; }
		[TargetName("NOTE_BODY")]
		public string Body { get; set ;}
	}
}
