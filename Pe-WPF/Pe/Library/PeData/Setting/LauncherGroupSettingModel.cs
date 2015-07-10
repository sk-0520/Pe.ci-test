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
	/// ランチャグループ。
	/// </summary>
	[DataContract, Serializable]
	public class LauncherGroupSettingModel: SettingModelBase
	{
		public LauncherGroupSettingModel()
			: base()
		{
			Groups = new LauncherGroupItemCollectionModel();
		}

		[DataMember]
		public LauncherGroupItemCollectionModel Groups { get; set; }
	}
}
