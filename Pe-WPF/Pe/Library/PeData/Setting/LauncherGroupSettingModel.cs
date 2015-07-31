namespace ContentTypeTextNet.Pe.Library.PeData.Setting
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	/// <summary>
	/// ランチャグループ。
	/// </summary>
	[DataContract, Serializable]
	public class LauncherGroupSettingModel: SettingModelBase, IDeepClone
	{
		public LauncherGroupSettingModel()
			: base()
		{
			Groups = new LauncherGroupItemCollectionModel();
		}

		#region property

		[DataMember]
		public LauncherGroupItemCollectionModel Groups { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (LauncherGroupSettingModel)target;

			obj.Groups.AddRange(Groups.Select(i => (LauncherGroupItemModel)i.DeepClone()));
		}

		public IDeepClone DeepClone()
		{
			var result = new LauncherGroupSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
