namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class DialogFilterValueItem<T>: DialogFilterItem
	{
		public DialogFilterValueItem(T value, string displayText, IEnumerable<string> wildcard)
			: base(displayText, wildcard)
		{
			Value = value;
		}

		public DialogFilterValueItem(T value, string displayText, params string[] wildcard)
			: base(displayText, wildcard)
		{
			Value = value;
		}

		#region property

		public T Value { get; private set; }

		#endregion

	}
}
