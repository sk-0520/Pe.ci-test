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

	[Serializable]
	[XmlType("Component"), XmlRoot("Component")]
	public class ComponentItemModel: ItemModelBase, IName
	{
		public ComponentItemModel()
		{ }

		#region property

		[DataMember, XmlAttribute]
		public ComponentKind ComponentKind { get; set; }

		[DataMember, XmlAttribute]
		public string Uri { get; set; }

		[DataMember, XmlAttribute]
		public string License { get; set; }

		#endregion

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

	}
}
