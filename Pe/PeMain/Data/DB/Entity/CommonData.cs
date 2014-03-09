/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/08
 * 時刻: 13:24
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Drawing;
using PeUtility;

namespace PeMain.Data.DB
{
	public abstract class CommonDataEntity: Entity
	{
		[TargetName("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
		[TargetName("CMN_CREATE")]
		public DateTime CommonCreate { get; set; }
		[TargetName("CMN_UPDATE")]
		public DateTime CommonUpdate { get; set; }
	}
	
	public interface INoteStyleEntity
	{
		string FontFamily { get; set; }
		float FontHeight { get; set; }
		bool FontItalic { get; set; }
		bool FontBold { get; set; }
		Color ForeColor { get; set; }
		Color BackColor { get; set; }
	}
	
	public abstract class NoteStyleEntity: CommonDataEntity, INoteStyleEntity
	{
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
	}
}
