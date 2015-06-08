namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[DataContract, Serializable]
	public class TIdCollection<TKey, TValue>: DictionaryModel<TKey, TValue>, IIsDisposed
		where TValue: ITId<TKey>
		where TKey: IComparable
	{
		public TIdCollection()
			: base()
		{ }

		public TIdCollection(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{ }

		public TIdCollection(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{ }

		public TIdCollection(int capacity)
			: base(capacity)
		{ }

		public TIdCollection(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: base(dictionary, comparer)
		{ }

		public TIdCollection(int capacity, IEqualityComparer<TKey> comparer)
			: base(capacity, comparer)
		{ }

		public TIdCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

		/*
		public new void Add(T item)
		{
			if (item.Key.CompareTo(item.Value.Id) != 0) {
				throw new ArgumentException("item.Key != item.Value.Id", "item");
			}
			base.Add(item);
		}
		 * */

		public new void Add(TKey key, TValue value)
		{
			if (value != null && key.CompareTo(value.Id) != 0) {
				throw new ArgumentException("key != value.Id");
			}

			base.Add(key, value);
		}

		public void Add(TValue value)
		{
			if (value == null) {
				throw new ArgumentNullException("value");
			}

			Add(value.Id, value);
		}
	}
}
