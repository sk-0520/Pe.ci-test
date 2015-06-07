namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[DataContract, Serializable]
	public class LauncherItemCollectionItemModel: TIdCollection<string, LauncherItemModel>
	{
		public LauncherItemCollectionItemModel()
			: base()
		{ }

		public LauncherItemCollectionItemModel(IDictionary<string, LauncherItemModel> dictionary)
			: base(dictionary)
		{ }

		public LauncherItemCollectionItemModel(IEqualityComparer<string> comparer)
			: base(comparer)
		{ }

		public LauncherItemCollectionItemModel(int capacity)
			: base(capacity)
		{ }

		public LauncherItemCollectionItemModel(IDictionary<string, LauncherItemModel> dictionary, IEqualityComparer<string> comparer)
			: base(dictionary, comparer)
		{ }

		public LauncherItemCollectionItemModel(int capacity, IEqualityComparer<string> comparer)
			: base(capacity, comparer)
		{ }

		public LauncherItemCollectionItemModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
