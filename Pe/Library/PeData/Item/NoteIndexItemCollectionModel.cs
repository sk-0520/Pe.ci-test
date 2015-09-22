namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// ノートインデックスの統括データ。
	/// </summary>
	[Serializable]
	public class NoteIndexItemCollectionModel: IndexItemCollectionModel<NoteIndexItemModel>
	{
		public NoteIndexItemCollectionModel()
			: base()
		{ }
	}
}
