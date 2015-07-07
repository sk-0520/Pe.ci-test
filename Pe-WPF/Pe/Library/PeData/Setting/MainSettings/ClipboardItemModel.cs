namespace ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Runtime.Serialization;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

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

		/// <summary>
		/// アプリケーション内でのコピー操作も監視対象とするか。
		/// </summary>
		[DataMember]
		public bool EnabledApplicationCopy { get; set; }

		#endregion

	}
}
