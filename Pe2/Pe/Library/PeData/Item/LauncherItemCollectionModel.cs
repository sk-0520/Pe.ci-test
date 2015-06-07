namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[DataContract, Serializable]
	public class LauncherItemCollectionModel: TIdCollection<string, LauncherItemModel>, IItemModel
	{
		public LauncherItemCollectionModel()
			: base()
		{ }

		public LauncherItemCollectionModel(IDictionary<string, LauncherItemModel> dictionary)
			: base(dictionary)
		{ }

		public LauncherItemCollectionModel(IEqualityComparer<string> comparer)
			: base(comparer)
		{ }

		public LauncherItemCollectionModel(int capacity)
			: base(capacity)
		{ }

		public LauncherItemCollectionModel(IDictionary<string, LauncherItemModel> dictionary, IEqualityComparer<string> comparer)
			: base(dictionary, comparer)
		{ }

		public LauncherItemCollectionModel(int capacity, IEqualityComparer<string> comparer)
			: base(capacity, comparer)
		{ }

		public LauncherItemCollectionModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
