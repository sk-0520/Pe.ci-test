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
	/// ランチャアイテム。
	/// </summary>
	[DataContract, Serializable]
	public class LauncherItemSettingModel: SettingModelBase, IDeepClone
	{
		public LauncherItemSettingModel()
			: base()
		{
			Items = new LauncherItemCollectionModel();
		}

		#region property

		[DataMember]
		public LauncherItemCollectionModel Items { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (LauncherItemSettingModel)target;

			obj.Items.InitializeRange(Items.Select(i => (LauncherItemModel)i.DeepClone()));
		}

		public IDeepClone DeepClone()
		{
			var result = new LauncherItemSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
