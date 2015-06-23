namespace ContentTypeTextNet.Pe.Library.PeData.Item
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;

	[Serializable]
	public class ClipboardItemModel: ItemModelBase
	{
		public ClipboardItemModel()
			: base()
		{ }

		#region property

		/// <summary>
		/// クリップボード監視の変更を検知するか。
		/// </summary>
		[DataMember]
		public bool Enabled { get; set; }

		#endregion

	}
}
