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

	[DataContract, Serializable]
	public class ClipboardIndexSettingModel: IndexSettingModelBase<ClipboardIndexItemCollectionModel, ClipboardIndexItemModel>
	{
		public ClipboardIndexSettingModel()
			:base()
		{ }

		#region IndexSettingModelBase

		public override IDeepClone DeepClone()
		{
			var result = new ClipboardIndexSettingModel();

			DeepCloneTo(result);

			return result;
		}
		

		#endregion
	}
}
