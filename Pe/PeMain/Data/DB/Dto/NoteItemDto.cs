namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// ノート。
	/// </summary>
	public class NoteItemDto: Dto
	{
		/// <summary>
		/// 有効データであるか。
		/// </summary>
		[EntityMapping("CMN_ENABLED")]
		public bool CommonEnabled { get; set; }
		
		/// <summary>
		/// ノートID。
		/// </summary>
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }
		/// <summary>
		/// タイトル。
		/// </summary>
		[EntityMapping("NOTE_TITLE")]
		public string Title { get; set; }
		/// <summary>
		/// ノートの種別。死に項目。
		/// </summary>
		[EntityMapping("NOTE_TYPE")]
		public int RawType { get; set; }
		/// <summary>
		/// 本文。
		/// </summary>
		[EntityMapping("NOTE_BODY")]
		public string Body { get; set ;}

		/// <summary>
		/// 表示状態。
		/// </summary>
		[EntityMapping("WINDOW_VISIBLED")]
		public bool Visibled { get; set; }
		/// <summary>
		/// 固定されているか。
		/// </summary>
		[EntityMapping("WINDOW_LOCKED")]
		public bool Locked { get; set; }
		/// <summary>
		/// 最前面状態。
		/// </summary>
		[EntityMapping("WINDOW_TOPMOST")]
		public bool Topmost { get; set; }
		/// <summary>
		/// 最小化状態。
		/// </summary>
		[EntityMapping("WINDOW_COMPACT")]
		public bool Compact { get; set; }
		/// <summary>
		/// X座標。
		/// </summary>
		[EntityMapping("WINDOW_POS_X")]
		public int X { get; set; }
		/// <summary>
		/// Y座標。
		/// </summary>
		[EntityMapping("WINDOW_POS_Y")]
		public int Y { get; set; }
		/// <summary>
		/// 横幅。
		/// </summary>
		[EntityMapping("WINDOW_SIZE_W")]
		public int Width { get; set; }
		/// <summary>
		/// 高さ。
		/// </summary>
		[EntityMapping("WINDOW_SIZE_H")]
		public int Height { get; set; }
		
		/// <summary>
		/// フォントファミリ。
		/// </summary>
		[EntityMapping("FONT_FAMILY")]
		public string FontFamily { get; set; }
		/// <summary>
		/// フォントの高さ。
		/// </summary>
		[EntityMapping("FONT_SIZE")]
		public float FontHeight { get; set; }
		/// <summary>
		/// イタリック体。
		/// </summary>
		[EntityMapping("FONT_ITALIC")]
		public bool FontItalic { get; set; }
		/// <summary>
		/// ボールド体。
		/// </summary>
		[EntityMapping("FONT_BOLD")]
		public bool FontBold { get; set; }
		/// <summary>
		/// 前景色。
		/// </summary>
		[EntityMapping("COLOR_FORE")]
		public Color ForeColor { get; set; }
		/// <summary>
		/// 背景色。
		/// </summary>
		[EntityMapping("COLOR_BACK")]
		public Color BackColor { get; set; }
	}
}
