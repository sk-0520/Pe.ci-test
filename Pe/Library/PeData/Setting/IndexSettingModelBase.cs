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

	public abstract class IndexSettingModelBase<TCollectionModel, TItemModel>: SettingModelBase, IDeepClone
		where TCollectionModel: IndexItemCollectionModel<TItemModel>, new()
		where TItemModel: IndexItemModelBase, IDeepClone
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

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (IndexSettingModelBase<TCollectionModel, TItemModel>)target;

			obj.Items.InitializeRange(Items.Select(i => (TItemModel)i.DeepClone()));
		}

		public abstract IDeepClone DeepClone();

		#endregion
	}
}
