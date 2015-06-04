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
	/// 履歴保持アイテム。
	/// </summary>
	[DataContract, Serializable]
	public class HistoryItemModel: ItemModelBase
	{
		public HistoryItemModel() 
			: base()
		{
			UpdateDateTime = CreateDateTime = DateTime.Now;
			UpdateCount = 0;
		}

		/// <summary>
		/// 作成日。
		/// </summary>
		[DataMember, XmlAttribute]
		public DateTime CreateDateTime { get; set; }
		/// <summary>
		/// 更新日。
		/// </summary>
		[DataMember, XmlAttribute]
		public DateTime UpdateDateTime { get; set; }
		/// <summary>
		/// 更新回数。
		/// </summary>
		[DataMember, XmlAttribute]
		public int UpdateCount { get; set; }
	}
}
