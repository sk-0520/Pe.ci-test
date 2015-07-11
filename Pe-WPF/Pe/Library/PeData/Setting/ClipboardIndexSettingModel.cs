namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[DataContract, Serializable]
	public class ClipboardIndexSettingModel: IndexSettingModelBase<ClipboardItemCollectionModel, ClipboardItemModel>
	{
		public ClipboardIndexSettingModel()
			:base()
		{ }
	}
}
