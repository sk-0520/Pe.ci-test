using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.PeMain.Data
{
	/// <summary>
	/// 認識可能とするクリップボード形式。
	/// </summary>
	[Flags]
	public enum ClipboardType
	{
		Text,
		RichTextFormat,
		Image,
		File,
	}

	/// <summary>
	/// クリップボードのデータ。
	/// </summary>
	public class ClipboardItem: Item
	{
		public ClipboardItem()
		{
			Timestamp = DateTime.Now;
		}

		public DateTime Timestamp { get; set; }
		public IDataObject Data { get; set; }
	}

	[Serializable]
	public class ClipboardSetting: Item
	{
		public ClipboardSetting()
		{
			Items = new ConcurrentFixedSizedQueue<ClipboardItem>(Literal.clipboardLimit);
			EnabledApplicationCopy = false;
		}

		/// <summary>
		/// 標準で使用するデータ形式。
		/// </summary>
		public ClipboardType DefaultClipboardType { get; set; }
		/// <summary>
		/// 本体でのコピー操作でもコピー検知に含めるか。
		/// </summary>
		public bool EnabledApplicationCopy { get; set; }

		/// <summary>
		/// クリップボードデータ
		/// </summary>
		[XmlIgnore]
		public ConcurrentFixedSizedQueue<ClipboardItem> Items { get; set; }
	}
}
