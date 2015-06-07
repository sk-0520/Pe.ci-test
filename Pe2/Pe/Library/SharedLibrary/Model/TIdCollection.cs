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
	public class TIdCollection<TId>: DictionaryModel<TId, ITId<TId>>
	{
		public TIdCollection()
			: base()
		{ }

		public TIdCollection(IDictionary<TId, ITId<TId>> dictionary)
			: base(dictionary)
		{ }

		public TIdCollection(IEqualityComparer<TId> comparer)
			: base(comparer)
		{ }

		public TIdCollection(int capacity)
			: base(capacity)
		{ }

		public TIdCollection(IDictionary<TId, ITId<TId>> dictionary, IEqualityComparer<TId> comparer)
			: base(dictionary, comparer)
		{ }

		public TIdCollection(int capacity, IEqualityComparer<TId> comparer)
			: base(capacity, comparer)
		{ }

		public TIdCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

	}
}
