/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/09
 * 時刻: 21:29
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using PeUtility;

namespace PeMain.Data.DB
{
	/// <summary>
	/// 
	/// 
	/// NOTE: この継承はなんかダメな気がする
	/// </summary>
	[TargetName("M_NOTE_GROUP")]
	public class MNoteGroupEntity: NoteStyleEntity
	{
		[TargetName("GROUP_ID")]
		public long Id { get; set ;}
		[TargetName("GROUP_TITLE")]
		public string Title {get; set; }
		/*
		// mixinつかいてぇ
		[TargetName("FONT_FAMILY")]
		public string FontFamily { get; set; }
		[TargetName("FONT_SIZE")]
		public float FontHeight { get; set; }
		[TargetName("FONT_ITALIC")]
		public bool FontItalic { get; set; }
		[TargetName("FONT_BOLD")]
		public bool FontBold { get; set; }
		[TargetName("COLOR_FORE")]
		public Color ForeColor { get; set; }
		[TargetName("COLOR_BACK")]
		public Color BackColor { get; set; }
		*/
	}
}
