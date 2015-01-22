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

		protected override void Dispose(bool disposing)
		{
			foreach(var item in Items) {
				item.ToDispose();
			}

			base.Dispose(disposing);
		}
	}
}
