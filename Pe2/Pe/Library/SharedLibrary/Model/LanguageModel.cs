namespace ContentTypeTextNet.Library.SharedLibrary.Model
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
	/// 言語設定。
	/// </summary>
	public class LanguageModel : ModelBase, IName
	{
		#region property

		/// <summary>
		/// 言語コード。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Code { get; set; }

		#region IName

		/// <summary>
		/// 言語名。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion

		/// <summary>
		/// 共通定義部。
		/// </summary>
		[DataMember, XmlArray, XmlArrayItem("Item")]
		public IEnumerable<LanguageItemModel> Define { get; set; }
		/// <summary>
		/// 言語データ。
		/// </summary>
		[DataMember, XmlArray, XmlArrayItem("Item")]
		public IEnumerable<LanguageItemModel> Words { get; set; }

		#endregion

	}
}
