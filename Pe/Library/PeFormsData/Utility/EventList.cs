namespace ContentTypeTextNet.Pe.Library.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// イベント発行するリスト。
	/// 
	/// あーこれラップした方がいいんかねぇ、、、今更無理だけど。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EventList<T>: List<T>
	{
		public event EventHandler ListChanged;

		public EventList()
			: base()
		{ }

		public EventList(IEnumerable<T> collection)
			: base(collection)
		{ }

		public EventList(int capacity)
			: base(capacity)
		{ }

		protected bool DisableEvent { get; set; }

		protected void CallListChangedEvent()
		{
			if(ListChanged != null && !DisableEvent) {
				ListChanged(this, new EventArgs());
			}
		}

		public virtual new void Add(T item)
		{
			base.Add(item);

			CallListChangedEvent();
		}

		public virtual new void AddRange(IEnumerable<T> collection)
		{
			base.AddRange(collection);

			CallListChangedEvent();
		}

		public virtual new void Insert(int index, T item)
		{
			base.Insert(index, item);

			CallListChangedEvent();
		}

		public virtual new void Clear()
		{
			base.Clear();

			CallListChangedEvent();
		}

		public virtual new void RemoveAt(int index)
		{
			base.RemoveAt(index);

			CallListChangedEvent();
		}
	}
}
