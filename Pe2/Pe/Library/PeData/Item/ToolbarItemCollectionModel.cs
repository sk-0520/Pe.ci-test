using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Library.PeData.IF;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	[Serializable]
	public class ToolbarItemCollectionModel : TIdCollection<string, ToolbarItemModel>, IItemModel
	{
		public ToolbarItemCollectionModel()
			: base()
		{ }

		public ToolbarItemCollectionModel(IDictionary<string, ToolbarItemModel> dictionary)
			: base(dictionary)
		{ }

		public ToolbarItemCollectionModel(IEqualityComparer<string> comparer)
			: base(comparer)
		{ }

		public ToolbarItemCollectionModel(int capacity)
			: base(capacity)
		{ }

		public ToolbarItemCollectionModel(IDictionary<string, ToolbarItemModel> dictionary, IEqualityComparer<string> comparer)
			: base(dictionary, comparer)
		{ }

		public ToolbarItemCollectionModel(int capacity, IEqualityComparer<string> comparer)
			: base(capacity, comparer)
		{ }

		public ToolbarItemCollectionModel(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
