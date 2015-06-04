namespace ContentTypeTextNet.Pe.Library.PeModel.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;

	/// <summary>
	/// タグを管理。
	/// </summary>
	[DataContract, Serializable]
	public class TagsItemModel: PeModelBase
	{
		public TagsItemModel() 
			: base() 
		{
			Items = new List<string>();
		}

		/// <summary>
		/// タグ。
		/// </summary>
		[DataMember, XmlArray("Items"), XmlArrayItem("Item")]
		IList<string> Items { get; set; }
	}
}
