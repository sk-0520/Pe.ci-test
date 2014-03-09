/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/09
 * 時刻: 20:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Diagnostics;
using System.Drawing;

namespace PeMain.Data
{
	public enum NoteType
	{
		Text,
		Rtf,
	}
	
	public static class NoteTypeUtility
	{
		public static long ToLong(this NoteType type)
		{
			switch(type) {
				case NoteType.Text: return 1;
				case NoteType.Rtf:  return 2;
				default:
					Debug.Assert(false, type.ToString());
					return -1;
			}
		}
	}
	
	/// <summary>
	/// ノートのデータ保持
	/// 
	/// 主要データはDBに格納するためシリアライズ処理は行わない
	/// </summary>
	public class NoteItem
	{
		public NoteItem()
		{
		}
		
		public long NoteId { get; set; }
		
		public string Title { get; set; }
		public string Body { get; set; }
		
		public NoteStyle Style { get; set; }
		
		public bool Visibled { get; set; }
		public bool Topmost { get; set; }
		public bool Compact { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }
	}
	
	public class NoteStyle
	{
		public FontSetting FontSetting { get; set; }
		public Color ForeColor { get; set; }
		public Color BaclColor { get; set; }
	}
}
