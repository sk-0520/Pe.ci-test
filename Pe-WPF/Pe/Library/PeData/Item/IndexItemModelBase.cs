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
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	public abstract class IndexItemModelBase: GuidModelBase, IName
	{
		public IndexItemModelBase()
			: base()
		{ }

		#region IName

		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion
	}
}
