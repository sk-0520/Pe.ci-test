/*
 * SharpDevelopによって生成
 * ユーザ: sk
 * 日付: 2014/03/10
 * 時刻: 23:02
 * 
 * このテンプレートを変更する場合「ツール→オプション→コーディング→標準ヘッダの編集」
 */
using System.Drawing;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Application.Data.DB
{
	/// <summary>
	/// Description of T_NOTE_STYLE.
	/// </summary>
	[TargetName("T_NOTE_STYLE")]
	public class TNoteStyleEntity: CommonDataEntity
	{
		[TargetName("NOTE_ID", true)]
		public long Id { get; set; }

		[TargetName("WINDOW_VISIBLED")]
		public bool Visibled { get; set; }
		[TargetName("WINDOW_LOCKED")]
		public bool Locked { get; set; }
		[TargetName("WINDOW_TOPMOST")]
		public bool Topmost { get; set; }
		[TargetName("WINDOW_COMPACT")]
		public bool Compact { get; set; }
		[TargetName("WINDOW_POS_X")]
		public int X { get; set; }
		[TargetName("WINDOW_POS_Y")]
		public int Y { get; set; }
		[TargetName("WINDOW_SIZE_W")]
		public int Width { get; set; }
		[TargetName("WINDOW_SIZE_H")]
		public int Height { get; set; }
		
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
