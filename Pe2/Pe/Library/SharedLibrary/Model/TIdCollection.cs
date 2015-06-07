namespace ContentTypeTextNet.Library.SharedLibrary.Model
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;

	[DataContract]
	public class TIdCollection<TKey, TValue>: DictionaryModel<TKey, TValue>
		where TValue: ITId<TValue>
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

	}
}
