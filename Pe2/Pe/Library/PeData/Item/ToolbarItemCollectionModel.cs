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
	}
}
