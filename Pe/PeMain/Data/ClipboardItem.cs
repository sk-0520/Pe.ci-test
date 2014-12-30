using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	public class ClipboardHtmlDataItem: Item
	{
		public ClipboardHtmlDataItem(): base()
		{
			Version = decimal.Zero;
			Html = new RangeItem<int>();
			Fragment = new RangeItem<int>();
			Selection = new RangeItem<int>();

			Html.SetRange(-1);
			Fragment.SetRange(-1);
			Selection.SetRange(-1);
		}

		public decimal Version { get; set; }
		public RangeItem<int> Html { get; set; }
		public RangeItem<int> Fragment { get; set; }
		public RangeItem<int> Selection { get; set; }
		public Uri SourceURL { get; set; }

		public string HtmlText { get; set; }
		public string FragmentText { get; set; }
		public string SelectionText { get; set; }
	}
}
