namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.IF;

	public class NoteBodyItemModel: ItemModelBase, IIndexBody
	{
		public NoteBodyItemModel()
			: base()
		{ }

		#region property

		public string Text { get; set; }

		#endregion

		#region IIndexBody

		public IndexKind IndexKind { get { return IndexKind.Note; } }

		#endregion
	}
}
