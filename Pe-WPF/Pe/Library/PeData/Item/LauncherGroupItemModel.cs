namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Model;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[Serializable]
	public class LauncherGroupItemModel: GuidModelBase, IName, IDeepClone
	{
		public LauncherGroupItemModel()
			: base()
		{
			LauncherItems = new CollectionModel<Guid>();
		}

		#region property

		/// <summary>
		/// グループ種別。
		/// </summary>
		[DataMember]
		public GroupKind GroupKind { get; set; }

		/// <summary>
		/// ランチャーアイテム。
		/// </summary>
		[DataMember, XmlArrayItem("Item")]
		public CollectionModel<Guid> LauncherItems { get; set; }

		#endregion

		#region IName

		/// <summary>
		/// グループ名称。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		#region IDeepClone

		public void DeepCloneTo(IDeepClone target)
		{
			var obj = (LauncherGroupItemModel)target;

			obj.GroupKind = GroupKind;
			obj.LauncherItems.AddRange(LauncherItems);
		}

		public IDeepClone DeepClone()
		{
			var result = new LauncherGroupItemModel();

			DeepCloneTo(result);

			return result;
		}

		#endregion
	}
}
