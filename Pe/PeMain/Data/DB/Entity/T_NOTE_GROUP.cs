/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/10
 * 時刻: 23:13
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using PeUtility;

namespace PeMain.Data.DB
{
	/// <summary>
	/// Description of T_NOTE_GROUP.
	/// </summary>
	[TargetName("T_NOTE_GROUP")]
	public class TNoteGroupEntity: Entity
	{
		[TargetName("GROUP_ID")]
		public long GroupId { get; set; }
		[TargetName("NOTE_ID")]
		public long NoteId { get; set; }
	}
}
