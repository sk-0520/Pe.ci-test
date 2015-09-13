namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	public class DialogFilterItem: IDisplayText
	{
		public DialogFilterItem(string displayText, IEnumerable<string> wildcard)
		{
			DisplayText = displayText;
			Wildcard = new List<string>(wildcard);
		}

		public DialogFilterItem(string displayText, params string[] wildcard)
			: this(displayText, (IEnumerable<string>)wildcard)
		{ }

		#region property

		public IReadOnlyList<string> Wildcard { get; private set; }

		#endregion

		#region IDisplayText

		public string DisplayText { get; private set; }

		#endregion

		#region object

		public override string ToString()
		{
			return string.Format("{0}|{1}", DisplayText, string.Join(";", Wildcard));
		}

		#endregion
	}
}
