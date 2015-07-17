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
	/// 履歴保持アイテム。
	/// </summary>
	[Serializable]
	public class HistoryItemModel: ItemModelBase, IDeepClone
	{
		public HistoryItemModel() 
			: base()
		{
			UpdateTimestamp = CreateTimestamp = DateTime.Now;
			UpdateCount = 0;
		}

		#region property

		/// <summary>
		/// 作成日。
		/// </summary>
		[DataMember, XmlAttribute]
		public DateTime CreateTimestamp { get; set; }
		/// <summary>
		/// 更新日。
		/// </summary>
		[DataMember, XmlAttribute]
		public DateTime UpdateTimestamp { get; set; }
		/// <summary>
		/// 更新回数。
		/// </summary>
		[DataMember, XmlAttribute]
		public int UpdateCount { get; set; }

		#endregion

		#region IDeepClone

		public virtual IDeepClone DeepClone()
		{
			var result = new HistoryItemModel() {
				CreateTimestamp = this.CreateTimestamp,
				UpdateTimestamp = this.UpdateTimestamp,
				UpdateCount = this.UpdateCount
			};

			return result;
		}

		#endregion

		#region function

		public virtual void Update()
		{
			UpdateCount += 1;
			UpdateTimestamp = DateTime.Now;
		}

		#endregion
	}
}
