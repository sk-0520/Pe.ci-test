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
	/// 言語データの最小データ。
	/// </summary>
	[DataContract, Serializable]
	public class LanguageItemModel: ModelBase, IName
	{
		#region property

		/// <summary>
		/// 表示用文字列。
		/// </summary>
		[DataMember, XmlAttribute]
		public string Word { get; set; }

		#endregion

		#region IName

		/// <summary>
		/// キーとして使用される、
		/// </summary>
		[DataMember, XmlAttribute]
		public string Name { get; set; }

		#endregion
	}
}
