namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	[DataContract, Serializable]
	public abstract class IndexBodyItemModelBase : ItemModelBase
	{
		public IndexBodyItemModelBase()
			: base()
		{
			History = new HistoryItemModel();
		}

		#region property

		[IgnoreDataMember, XmlIgnore]
		public abstract IndexKind IndexKind { get; }

		[DataMember]
		public HistoryItemModel History { get; set; }

		#endregion
	}
}
