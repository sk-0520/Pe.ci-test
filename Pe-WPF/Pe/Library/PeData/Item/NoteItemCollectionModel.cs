using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Library.SharedLibrary.Model;

namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	[Serializable]
	public class NoteItemCollectionModel: GuidCollectionBase<NoteItemModel>
	{
		public NoteItemCollectionModel()
			: base()
		{ }
	}
}
