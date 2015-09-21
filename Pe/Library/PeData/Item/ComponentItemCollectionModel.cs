namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Library.SharedLibrary.Model;

	/// <summary>
	/// コンポーネント情報統括。
	/// </summary>
	[Serializable, XmlRoot("Components")]
	public class ComponentItemCollectionModel: CollectionModel<ComponentItemModel>
	{ }
}
