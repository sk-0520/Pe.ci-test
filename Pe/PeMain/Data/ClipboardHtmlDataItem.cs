namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;

	/// <summary>
	/// クリップボードとしてのHTMLデータ。
	/// </summary>
	public sealed class ClipboardHtmlDataItem: Item
	{
		public ClipboardHtmlDataItem()
			: base()
		{
			Version = decimal.Zero;
			Html = new RangeItem<int>();
			Fragment = new RangeItem<int>();
			Selection = new RangeItem<int>();

			Html.SetRange(-1);
			Fragment.SetRange(-1);
			Selection.SetRange(-1);
		}

		/// <summary>
		/// バージョン。
		/// </summary>
		public decimal Version { get; set; }
		/// <summary>
		/// HTMLデータの長さ。
		/// </summary>
		public RangeItem<int> Html { get; set; }
		/// <summary>
		/// Fragmentデータの長さ。
		/// </summary>
		public RangeItem<int> Fragment { get; set; }
		/// <summary>
		/// Selectionデータの長さ。
		/// </summary>
		public RangeItem<int> Selection { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public Uri SourceURL { get; set; }
		/// <summary>
		/// HTMLテキストデータ。
		/// </summary>
		public string HtmlText { get; set; }
		/// <summary>
		/// Fragmentテキストデータ。
		/// </summary>
		public string FragmentText { get; set; }
		/// <summary>
		/// Selectionテキストデータ。
		/// </summary>
		public string SelectionText { get; set; }

		/// <summary>
		/// それっぽいHTMLの取得。
		/// </summary>
		/// <returns></returns>
		public string ToHtml()
		{
			return HtmlText ?? FragmentText ?? SelectionText ?? string.Empty;
		}
	}
}
