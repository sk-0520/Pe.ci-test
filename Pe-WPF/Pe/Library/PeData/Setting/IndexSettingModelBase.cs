namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public abstract class IndexSettingModelBase<TCollectionModel, TItemModel>: SettingModelBase
		where TCollectionModel: IndexItemCollectionModel<TItemModel>, new()
		where TItemModel: IndexItemModelBase
	{
		public IndexSettingModelBase()
			: base()
		{
			Items = new TCollectionModel();
		}

		#region property

		[DataMember]
		public TCollectionModel Items { get; set; }

		#endregion
	}
}
