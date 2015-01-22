namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Diagnostics;
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// NoteTypeに対してなんかする用ユーティリティだが現状死んでますしおすし。
	/// </summary>
	public static class NoteTypeUtility
	{
		public static int ToNumber(this NoteType type)
		{
			switch(type) {
				case NoteType.Text: return 0;
				case NoteType.Rtf:  return 1;
				default:
					throw new NotImplementedException();
			}
		}
		public static NoteType ToNoteType(long value)
		{
			switch(value) {
				case 0: return NoteType.Text;
				case 1: return NoteType.Rtf;
				default:
					throw new NotImplementedException();
			}
		}
	}
	
	/// <summary>
	/// ノートのデータ保持
	/// 
	/// 主要データはDBに格納するためシリアライズ処理は行わない
	/// </summary>
	public class NoteItem: DisposableItem, IDisposable
	{
		public NoteItem()
		{
			Style = new NoteStyle();
			
			Visible = true;
			Locked = false;
			Topmost = false;
			Compact = false;
			
			Title = string.Empty;
			Body = string.Empty;
			NoteType = NoteType.Text;
			
			Size = Literal.noteSize;
		}
		
		public long NoteId { get; set; }
		
		public string Title { get; set; }
		public string Body { get; set; }
		
		public NoteType NoteType { get; set; }
		
		public NoteStyle Style { get; set; }
		
		public bool Visible { get; set; }
		public bool Locked { get; set; }
		public bool Topmost { get; set; }
		public bool Compact { get; set; }
		public Point Location { get; set; }
		public Size Size { get; set; }

		protected override void Dispose(bool disposing)
		{
			Style.ToDispose();

			base.Dispose(disposing);
		}
	}

	public class NoteStyle: DisposableItem, IDisposable
	{
		public NoteStyle()
		{
			FontSetting = new FontSetting();
			ForeColor = Literal.noteFore;
			BackColor = Literal.noteBack;
		}
		
		public FontSetting FontSetting { get; set; }
		public Color ForeColor { get; set; }
		public Color BackColor { get; set; }

		protected override void Dispose(bool disposing)
		{
			FontSetting.ToDispose();

			base.Dispose(disposing);
		}
	}
}
