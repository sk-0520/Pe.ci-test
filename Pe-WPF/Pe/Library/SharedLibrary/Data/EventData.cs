namespace ContentTypeTextNet.Library.SharedLibrary.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class EventData<TEvent>
		where TEvent: EventArgs
	{
		public EventData(object sender, TEvent e)
		{
			Sender = sender;
			EventArgs = e;
		}

		#region property

		public object Sender { get; private set; }
		public TEvent EventArgs { get; private set; }

		#endregion
	}
}
