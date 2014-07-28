/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/09
 * 時刻: 20:20
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace PeMain.Data
{
	public enum NoteType
	{
		Text,
		Rtf,
	}
	
	public enum NoteCommand
	{
		Close,
		Compact,
		Lock,
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
		public static NoteType ToNoteType(long value)
		{
			switch(value) {
				case 1: return NoteType.Text;
				case 2: return NoteType.Rtf;
				default:
					Debug.Assert(false, value.ToString());
					return NoteType.Text;
			}
		}
	}
	
	/// <summary>
	/// ノートのデータ保持
	/// 
	/// 主要データはDBに格納するためシリアライズ処理は行わない
	/// </summary>
	public class NoteItem: IDisposable
	{
		public NoteItem()
		{
			Style = new NoteStyle();
			
			Visibled = true;
			Locked = false;
			Topmost = false;
			Compact = false;
			
			Size = Literal.noteSize;
		}
		
		public long NoteId { get; set; }
		
		public string Title { get; set; }
		public string Body { get; set; }
		
		public NoteStyle Style { get; set; }
		
		public bool Visibled { get; set; }
		public bool Locked { get; set; }
		public bool Topmost { get; set; }
		public bool Compact { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }
		
		public void Dispose()
		{
			Style.Dispose();
		}		
	}
	
	public class NoteStyle: IDisposable
	{
		public NoteStyle()
		{
			FontSetting = new FontSetting();
			ForeColor = Color.Black;
			BackColor = Color.LightYellow;
		}
		
		public FontSetting FontSetting { get; set; }
		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }
		
		public void Dispose()
		{
			FontSetting.Dispose();
		}
	}
	
	public class NoteGroup
	{
		public long GroupId { get; set; }
		public string Title { get; set; }
	}
}
