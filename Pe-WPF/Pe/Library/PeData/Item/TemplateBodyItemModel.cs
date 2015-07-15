namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Define;

	[DataContract, Serializable]
	public class TemplateBodyItemModel : IndexBodyItemModelBase
	{
		/// <summary>
		/// 対象文字列。
		/// </summary>
		[DataMember]
		public string Source { get; set; }

		#region IndexBodyItemModelBase

		public override IndexKind IndexKind { get { return IndexKind.Template; } }

		#endregion
	}
}
