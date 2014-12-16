using System;
using System.Collections.Generic;
using System.Drawing;
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

		public override string ToString()
		{
			return Timestamp.ToString();
		}
	}

	[Serializable]
	public class ClipboardSetting: Item
	{
		public ClipboardSetting()
		{
			Items = new FixedSizedList<ClipboardItem>(Literal.clipboardLimit);
			EnabledApplicationCopy = false;
			TextFont = new FontSetting(SystemFonts.DialogFont);
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
		/// 表示状態。
		/// </summary>
		public bool Visible { get; set; }
		/// <summary>
		/// サイズ。
		/// </summary>
		public Size Size { get; set; }
		/// <summary>
		/// 位置。
		/// </summary>
		public Point Location { get; set; }
		/// <summary>
		/// 最前面表示。
		/// </summary>
		public bool Topmost { get; set; }
		/// <summary>
		/// テキストデータのフォント
		/// </summary>
		public FontSetting TextFont { get; set; }
		/// <summary>
		/// クリップボードデータ
		/// </summary>
		[XmlIgnore]
		public FixedSizedList<ClipboardItem> Items { get; set; }
	}
}
