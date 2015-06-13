namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ランチャアイテム。
	/// </summary>
	[DataContract, Serializable]
	public class LauncherItemSettingModel: SettingModelBase
	{
		public LauncherItemSettingModel()
			: base()
		{
			Items = new LauncherItemCollectionModel();
		}

		[DataMember]
		public LauncherItemCollectionModel Items { get; set; }
	}
}
