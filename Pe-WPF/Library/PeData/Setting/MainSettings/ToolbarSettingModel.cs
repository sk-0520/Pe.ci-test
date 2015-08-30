namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	[Serializable]
	public class ToolbarSettingModel: SettingModelBase, IDeepClone
	{
		public ToolbarSettingModel()
		{
			Items = new ToolbarItemCollectionModel();
		}

		[DataMember]
		public ToolbarItemCollectionModel Items { get; set; }

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (ToolbarSettingModel)target;

			obj.Items.InitializeRange(Items.Select(i => (ToolbarItemModel)i.DeepClone()));
		}

		public IDeepClone DeepClone()
		{
			var result = new ToolbarSettingModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
