namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class ClipboardHtmlItemModel: ItemModelBase
	{
		public ClipboardHtmlItemModel()
			: base()
		{
			Html = new RangeModel<int>();
			Fragment = new RangeModel<int>();
			Selection = new RangeModel<int>();
		}

		/// <summary>
		/// バージョン。
		/// </summary>
		public decimal Version { get; set; }

		/// <summary>
		/// HTMLデータの長さ。
		/// </summary>
		public RangeModel<int> Html { get; set; }
		/// <summary>
		/// Fragmentデータの長さ。
		/// </summary>
		public RangeModel<int> Fragment { get; set; }
		/// <summary>
		/// Selectionデータの長さ。
		/// </summary>
		public RangeModel<int> Selection { get; set; }
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
