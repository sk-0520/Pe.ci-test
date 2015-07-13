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
			UpdateDateTime = CreateDateTime = DateTime.Now;
			UpdateCount = 0;
		}

		#region property

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

		#endregion

		#region IDeepClone

		public virtual IDeepClone DeepClone()
		{
			var result = new HistoryItemModel() {
				CreateDateTime = this.CreateDateTime,
				UpdateDateTime = this.UpdateDateTime,
				UpdateCount = this.UpdateCount
			};

			return result;
		}

		#endregion

		#region function

		public virtual void Update()
		{
			UpdateCount += 1;
			UpdateDateTime = DateTime.Now;
		}

		#endregion
	}
}
