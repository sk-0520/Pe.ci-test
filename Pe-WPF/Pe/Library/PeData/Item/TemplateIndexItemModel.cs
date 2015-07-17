namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	public class TemplateIndexItemModel: IndexItemModelBase
	{
		public TemplateIndexItemModel()
			: base()
		{ }

		#region property

		/// <summary>
		/// 置換処理を行うか。
		/// </summary>
		[DataMember]
		public bool IsReplace { get; set; }
		/// <summary>
		/// プログラム的置き換え処理を行うか。
		/// </summary>
		[DataMember]
		public bool IsProgrammableReplace { get; set; }

		#endregion
	}
}
