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
	using ContentTypeTextNet.Pe.Library.PeData.IF;

	/// <summary>
	/// インデックスデータのボディ部。
	/// </summary>
	[DataContract, Serializable]
	public abstract class IndexBodyItemModelBase: ItemModelBase, IDeepClone
	{
		public IndexBodyItemModelBase()
			: base()
		{
			History = new HistoryItemModel();
		}

		#region property

		/// <summary>
		/// インデックス種別。
		/// </summary>
		[IgnoreDataMember, XmlIgnore]
		public abstract IndexKind IndexKind { get; }

		/// <summary>
		/// 履歴。
		/// </summary>
		[DataMember]
		public HistoryItemModel History { get; set; }

		#endregion

		#region IDeepClone

		public virtual void DeepCloneTo(IDeepClone target)
		{
			var obj = (IndexBodyItemModelBase)target;

			History.DeepCloneTo(obj.History);
		}

		public abstract IDeepClone DeepClone();

		#endregion
	}
}
