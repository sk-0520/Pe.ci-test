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

	/// <summary>
	/// タグを管理。
	/// </summary>
	[DataContract, Serializable]
	public class TagItemModel: PeDataBase, IDeepClone
	{
		public TagItemModel() 
			: base() 
		{
			Items = new List<string>();
		}

		/// <summary>
		/// タグ。
		/// </summary>
		[DataMember, XmlArray("Items"), XmlArrayItem("Item")]
		public List<string> Items { get; set; }

		#region IDeepClone
		
		public IDeepClone DeepClone()
		{
			var result = new TagItemModel();

			result.Items.AddRange(Items);

			return result;
		}

		#endregion
	}
}
