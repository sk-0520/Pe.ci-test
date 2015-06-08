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

		#region function

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

		void Swap(TKey a, TKey b)
		{
			var valueA = this[a];
			var valueB = this[b];
			valueA.Id = b;
			valueB.Id = a;
			this[a] = valueB;
			this[b] = valueA;
		}

		void ChangeId(TKey src, TKey dst)
		{
			TValue temp;
			if (TryGetValue(dst, out temp)) {
				throw new ArgumentException(string.Format("exists key({0})", dst));
			}

			var valueSrc = this[src];
			valueSrc.Id = dst;
			Remove(src);
			Add(valueSrc);
		}

		#endregion
	}
}
