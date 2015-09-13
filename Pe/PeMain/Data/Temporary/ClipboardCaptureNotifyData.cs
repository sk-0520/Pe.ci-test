namespace ContentTypeTextNet.Pe.PeMain.Data.Temporary
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.Library.PeData.Item;

	public class ClipboardCaptureNotifyData:NotifyDataBase
	{
		/// <summary>
		/// 重複した場合に格納されるアイテム。
		/// </summary>
		public ClipboardIndexItemModel DuplicationItem { get; set; }
		/// <summary>
		/// 制限フィルタ処理により空。
		/// </summary>
		public bool EmptyFromFiltered { get; set; }
	}
}
