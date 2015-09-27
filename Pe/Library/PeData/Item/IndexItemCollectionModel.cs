namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// インデックスデータ統括。
	/// </summary>
	/// <typeparam name="TIndexModel"></typeparam>
	public class IndexItemCollectionModel<TIndexModel>: GuidCollectionBase<TIndexModel>
		where TIndexModel: IndexItemModelBase
	{
		public IndexItemCollectionModel()
			: base()
		{ }
	}
}
