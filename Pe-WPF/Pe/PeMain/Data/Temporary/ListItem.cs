namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class ListItem<T>
	{
		public ListItem(string displayText, T value)
		{
			DisplayText = displayText;
			Value = value;
		}

		public string DisplayText { get; private set; }
		public T Value { get; private set; }
	}
}
