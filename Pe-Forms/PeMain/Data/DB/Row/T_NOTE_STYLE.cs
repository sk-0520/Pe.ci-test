namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;
	using ContentTypeTextNet.Pe.PeMain.IF;

	/// <summary>
	/// T_NOTE_STYLE行。
	/// </summary>
	[EntityMapping("T_NOTE_STYLE")]
	public class TNoteStyleRow: CommonDataRow, INoteId
	{
		/// <summary>
		/// ノートID。
		/// </summary>
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }

		/// <summary>
		/// 表示状態。
		/// </summary>
		[EntityMapping("WINDOW_VISIBLED")]
		public bool Visibled { get; set; }
		/// <summary>
		/// 固定状態。
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
		/// フォントサイズ。
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
		
		/// <summary>
		/// 座標。
		/// </summary>
		public Point Location
		{
			get
			{
				return new Point(X, Y);
			}
			set
			{
				X =  value.X;
				Y = value.Y;
			}
		}
		
		/// <summary>
		/// サイズ。
		/// </summary>
		public Size Size
		{
			get
			{
				return new Size(Width, Height);
			}
			set
			{
				Width  = value.Width;
				Height = value.Height;
			}
		}
	}
}
