namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Reflection;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public class WindowItemCollectionModel: CollectionModel<WindowItemModel>, IName
	{
		public WindowItemCollectionModel()
			: base()
		{ }

		#region property

		public DateTime DateTime { get; set; }

		#endregion

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion
	}
}
