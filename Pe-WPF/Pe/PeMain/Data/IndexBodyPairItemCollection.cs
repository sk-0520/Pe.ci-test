namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;

	public class IndexBodyPairItemCollection<TIndexBody> : FixedSizeCollectionModel<IndexBodyPairItem<TIndexBody>>
		where TIndexBody: IndexBodyItemModelBase
	{
		public IndexBodyPairItemCollection(int limitSize)
			: base(limitSize)
		{ }
		/// <summary>
		/// 指定IDのデータを取得。
		/// </summary>
		/// <param name="id"></param>
		/// <returns>なければnull。</returns>
		public TIndexBody GetFromId(Guid id)
		{
			var result = this.FirstOrDefault(pair => pair.Id == id);
			if (result != null) {
				return result.Body;
			}

			return null;
		}
	}
}
