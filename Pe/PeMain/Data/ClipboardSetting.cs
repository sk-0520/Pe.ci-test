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
	public enum ClipboardType
	{
		None,
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
		static readonly IDictionary<string, ClipboardType> _map = new Dictionary<string, ClipboardType>() {
			{ "System.String,UnicodeText", ClipboardType.Text },
			{ "Text", ClipboardType.Text },

			{ "Rich Text Format", ClipboardType.RichTextFormat },

			{ "System.Drawing.Bitmap", ClipboardType.Image },
			{ "Bitmap", ClipboardType.Image },

			//{ "Shell IDList Array", ClipboardType.File },
			{ "FileDrop", ClipboardType.File },
			{ "FileNameW", ClipboardType.File },
			{ "FileName", ClipboardType.File },
		};

		public static ClipboardType ToType(string typeName)
		{
			ClipboardType type;
			if(_map.TryGetValue(typeName, out type)) {
				return type;
			}

			return ClipboardType.None;
		}

		public static IEnumerable<ClipboardType> ToEnabledType(IDataObject data)
		{
			var enabledType = false;
			foreach(var typeName in data.GetFormats()) {
				var type = ToType(typeName);
				if(type != ClipboardType.None) {
					enabledType = true;
					yield return type;
				}
			}
			if(!enabledType) {
				yield return ClipboardType.None;
			}
		}

		public static bool HasEnabledType(IDataObject data)
		{
			return ToEnabledType(data).Any(t => t != ClipboardType.None);
		}

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

		private IEnumerable<ClipboardType> GetClipboardTypeList()
		{
			foreach(var typeName in Data.GetFormats()) {
				yield return ToType(typeName);
			}
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
