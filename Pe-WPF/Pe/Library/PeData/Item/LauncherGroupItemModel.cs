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
	public class LauncherGroupItemModel: GuidModelBase, IName
	{
		public LauncherGroupItemModel()
			: base()
		{
			LauncherItems = new ObservableCollection<Guid>();
		}

		#region IName

		/// <summary>
		/// グループ名称。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		/// <summary>
		/// グループ種別。
		/// </summary>
		[DataMember]
		public GroupKind GroupKind { get; set; }

		/// <summary>
		/// ランチャーアイテム。
		/// </summary>
		[DataMember, XmlArrayItem("Item")]
		public ObservableCollection<Guid> LauncherItems { get; set; }
	}
}
