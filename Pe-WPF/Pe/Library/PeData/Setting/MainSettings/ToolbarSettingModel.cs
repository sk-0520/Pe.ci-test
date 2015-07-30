namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class ToolbarSettingModel: SettingModelBase
	{
		public ToolbarSettingModel()
		{
			Items = new ToolbarItemCollectionModel();
		}

		[DataMember]
		public ToolbarItemCollectionModel Items { get; set; }
	}
}
