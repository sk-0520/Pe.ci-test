namespace ContentTypeTextNet.Pe.PeMain.Data.DB
{
	using System.Drawing;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.Library.Utility.DB;

	/// <summary>
	/// Description of T_NOTE_STYLE.
	/// </summary>
	[EntityMapping("T_NOTE_STYLE")]
	public class TNoteStyleEntity: CommonDataEntity
	{
		[EntityMapping("NOTE_ID", true)]
		public long Id { get; set; }

		[EntityMapping("WINDOW_VISIBLED")]
		public bool Visibled { get; set; }
		[EntityMapping("WINDOW_LOCKED")]
		public bool Locked { get; set; }
		[EntityMapping("WINDOW_TOPMOST")]
		public bool Topmost { get; set; }
		[EntityMapping("WINDOW_COMPACT")]
		public bool Compact { get; set; }
		[EntityMapping("WINDOW_POS_X")]
		public int X { get; set; }
		[EntityMapping("WINDOW_POS_Y")]
		public int Y { get; set; }
		[EntityMapping("WINDOW_SIZE_W")]
		public int Width { get; set; }
		[EntityMapping("WINDOW_SIZE_H")]
		public int Height { get; set; }
		
		[EntityMapping("FONT_FAMILY")]
		public string FontFamily { get; set; }
		[EntityMapping("FONT_SIZE")]
		public float FontHeight { get; set; }
		[EntityMapping("FONT_ITALIC")]
		public bool FontItalic { get; set; }
		[EntityMapping("FONT_BOLD")]
		public bool FontBold { get; set; }
		[EntityMapping("COLOR_FORE")]
		public Color ForeColor { get; set; }
		[EntityMapping("COLOR_BACK")]
		public Color BackColor { get; set; }
		
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
