using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ContentTypeTextNet.Pe.Library.Utility
{
	public class DialogFilter
	{
		public DialogFilter()
		{
			Items = new List<DialogFilterItem>();
		}

		public List<DialogFilterItem> Items { get; private set; }

		public void Attachment(FileDialog dialog)
		{
			dialog.Filter = ToString();
		}

		public override string ToString()
		{
			return string.Join("|", Items.Select(i => i.ToString()));
		}
	}
}
