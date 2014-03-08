/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/08
 * 時刻: 16:44
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;

namespace PeMain.Data.DB
{
	public class CountDto: Dto
	{
		[TargetName("NUM")]
		public long Count { get; set; }
		
		public bool Has { get { return Count > 0; } }
	}
}
