namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[DataContract, Serializable]
	public class LauncherGroupItemCollectionModel: TIdCollection<string, LauncherGroupItemModel>, IItemModel
	{
		public LauncherGroupItemCollectionModel()
			: base()
		{ }

		public LauncherGroupItemCollectionModel(IDictionary<string, LauncherGroupItemModel> dictionary)
			: base(dictionary)
		{ }

		public LauncherGroupItemCollectionModel(IEqualityComparer<string> comparer)
			: base(comparer)
		{ }

		public LauncherGroupItemCollectionModel(int capacity)
			: base(capacity)
		{ }

		public LauncherGroupItemCollectionModel(IDictionary<string, LauncherGroupItemModel> dictionary, IEqualityComparer<string> comparer)
			: base(dictionary, comparer)
		{ }

		public LauncherGroupItemCollectionModel(int capacity, IEqualityComparer<string> comparer)
			: base(capacity, comparer)
		{ }

		public LauncherGroupItemCollectionModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }

	}
}
