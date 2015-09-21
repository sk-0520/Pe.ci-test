namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	/// <summary>
	/// コンポーネント情報。
	/// </summary>
	[Serializable]
	[XmlType("Component"), XmlRoot("Component")]
	public class ComponentItemModel: ItemModelBase, IName
	{
		public ComponentItemModel()
		{ }

		#region property

		/// <summary>
		/// コンポーネント種別。
		/// </summary>
		[DataMember, XmlAttribute]
		public ComponentKind Kind { get; set; }
		/// <summary>
		/// URI。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Uri { get; set; }
		/// <summary>
		/// ライセンス。
		/// </summary>
		[DataMember, XmlAttribute]
		public string License { get; set; }

		#endregion

		#region IName

		/// <summary>
		/// コンポーネント名。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

	}
}
