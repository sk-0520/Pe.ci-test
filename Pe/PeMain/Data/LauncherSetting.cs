namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.IO;
	using System.Linq;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.Skin;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// ランチャーアイテム統括。
	/// </summary>
	[Serializable]
	public class LauncherSetting: DisposableItem, IDisposable
	{
		public LauncherSetting()
		{
			Items = new HashSet<LauncherItem>();
			
			StreamFontSetting = new FontSetting(SystemFonts.DefaultFont);

			StreamGeneralColor = new ColorPairItem();
			StreamInputColor = new ColorPairItem();
			StreamErrorColor = new ColorPairItem();
		}
		
		/// <summary>
		/// 各ランチャアイテム
		/// </summary>
		[XmlIgnoreAttribute()]
		public HashSet<LauncherItem> Items { get; set; }
		
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
			foreach(var item in Items) {
				item.ToDispose();
			}

			base.Dispose(disposing);
		}

		public override void CorrectionValue()
		{
			base.CorrectionValue();

			// #228より色追加
			StreamGeneralColor.CorrectionColor(Literal.streamGeneralForeground, Literal.streamGeneralBackground);
			StreamInputColor.CorrectionColor(Literal.streamInputForeground, Literal.streamInputBackground);
			StreamErrorColor.CorrectionColor(Literal.streamErrorForeground, Literal.streamErrorBackground);
		}
	}
}
