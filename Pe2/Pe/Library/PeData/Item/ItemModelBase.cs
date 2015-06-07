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

	/// <summary>
	/// 設定項目データとして使用する基底モデル。
	/// </summary>
	[DataContract, Serializable]
	public abstract class ItemModelBase: PeDataBase, IItemModel
	{
		public ItemModelBase()
			: base()
		{ }
	}
}
