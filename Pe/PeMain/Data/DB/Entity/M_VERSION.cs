/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/08
 * 時刻: 15:02
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using PeUtility;

namespace PeMain.Data.DB
{
	[TargetName("M_VERSION")]
	public class MVersionEntity: Entity
	{
		[TargetName("NAME", true)]
		public string Name { get; set; }
		[TargetName("VERSION")]
		public int Version { get; set; }
	}
}
