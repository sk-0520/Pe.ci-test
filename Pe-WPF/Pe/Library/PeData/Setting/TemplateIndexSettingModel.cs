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
	public class TemplateIndexSettingModel: IndexSettingModelBase<TemplateItemCollectionModel, TemplateItemModel>
	{
		public TemplateIndexSettingModel()
			: base()
		{ }
	}
}
