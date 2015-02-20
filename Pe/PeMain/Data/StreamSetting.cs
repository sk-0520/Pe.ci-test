namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.Utility;

	public class StreamSetting: DisposableItem
	{
		public StreamSetting()
		{
			StreamFontSetting = new FontSetting(SystemFonts.DefaultFont);

			StreamGeneralColor = new ColorPairItem();
			StreamInputColor = new ColorPairItem();
			StreamErrorColor = new ColorPairItem();

			StreamGeneralColor.CorrectionColor(Literal.streamGeneralForeground, Literal.streamGeneralBackground);
			StreamInputColor.CorrectionColor(Literal.streamInputForeground, Literal.streamInputBackground);
			StreamErrorColor.CorrectionColor(Literal.streamErrorForeground, Literal.streamErrorBackground);
		}

		/// <summary>
		/// 標準出力フォント。
		/// </summary>
		public FontSetting StreamFontSetting { get; set; }

		/// <summary>
		/// 通常。
		/// </summary>
		public ColorPairItem StreamGeneralColor { get; set; }
		/// <summary>
		/// 入力時。
		/// </summary>
		public ColorPairItem StreamInputColor { get; set; }
		/// <summary>
		/// エラー。
		/// </summary>
		public ColorPairItem StreamErrorColor { get; set; }

		protected override void Dispose(bool disposing)
		{
			StreamFontSetting.ToDispose();
			base.Dispose(disposing);
		}
	}
}
