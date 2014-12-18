using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		None  = 0,
		Text  = 0x01,
		Rtf   = 0x02,
		Image = 0x04,
		File  = 0x08,
	}

	public class ClipboradWeight
	{
		public ClipboardType ClipboardType { get; set; }
		public int Weight { get; set; }
	}

	/// <summary>
	/// クリップボードのデータ。
	/// </summary>
	public class ClipboardItem: Item
	{
		/*
		static readonly IDictionary<string, ClipboardType> _map = new Dictionary<string, ClipboardType>() {
			{ "System.String,UnicodeText", ClipboardType.Text },
			{ "Text", ClipboardType.Text },

			{ "Rich Text Format", ClipboardType.RichTextFormat },

			{ "System.Drawing.Bitmap", ClipboardType.Image },
			{ "Bitmap", ClipboardType.Image },
			{ "Format17", ClipboardType.Image },
			{ "DeviceIndependentBitmap", ClipboardType.Image },
			

			//{ "Shell IDList Array", ClipboardType.File },
			{ "FileDrop", ClipboardType.File },
			{ "FileNameW", ClipboardType.File },
			{ "FileName", ClipboardType.File },
		};

		protected static ClipboardType ToType(string typeName)
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
		*/

		public ClipboardItem()
		{
			Timestamp = DateTime.Now;
		}

		public DateTime Timestamp { get; set; }

		public ClipboardType ClipboardTypes { get; set; }
		public string Text { get; set; }
		public string Rtf { get; set; }
		public Image Image { get; set; }
		public IEnumerable<string> Files { get; set; }

		public bool SetClipboardData()
		{
			var isText = Clipboard.ContainsText(TextDataFormat.Text);
			var isRtf = Clipboard.ContainsText(TextDataFormat.Rtf);
			var isImage = Clipboard.ContainsImage();
			var isFile = Clipboard.ContainsFileDropList();

			ClipboardTypes = ClipboardType.None;
			if(!isText && !isRtf && !isImage && !isFile) {
				return false;
			}
			Debug.WriteLine("t = {0} r = {1} i = {2} f = {3}", isText, isRtf, isImage, isFile);

			if(isText) {
				Text = Clipboard.GetText(TextDataFormat.Text);
				ClipboardTypes |= ClipboardType.Text;
			}
			if(isRtf) {
				Rtf = Clipboard.GetText(TextDataFormat.Rtf);
				ClipboardTypes |= ClipboardType.Rtf;
			}
			if(isImage) {
				Image = Clipboard.GetImage();
				ClipboardTypes |= ClipboardType.Image;
			}
			if(isFile) {
				Files = Clipboard.GetFileDropList().Cast<string>();
				ClipboardTypes |= ClipboardType.File;
			}
			Debug.WriteLine("ClipboardTypes = {0}", ClipboardTypes);

			return true;
		}

		public IEnumerable<ClipboardType> GetClipboardTypeList()
		{
			Debug.Assert(ClipboardTypes != ClipboardType.None);

			var list = new[] {
				ClipboardType.Text,
				ClipboardType.Rtf,
				ClipboardType.Image,
				ClipboardType.File,
			};
			foreach(var type in list) {
				if((ClipboardTypes & type) == type) {
					yield return type;
				}
			}
		}

		public ClipboardType GetSingleClipboardType()
		{
			Debug.WriteLine("> {0}", ClipboardTypes);
			if((ClipboardTypes & ClipboardType.Rtf) == ClipboardType.Rtf) {
				Debug.WriteLine(">> {0}", ClipboardTypes);
				return ClipboardType.Rtf;
			}
			if((ClipboardTypes & ClipboardType.File) == ClipboardType.File) {
				Debug.WriteLine(">> {0}", ClipboardTypes);
				return ClipboardType.File;
			}
			if((ClipboardTypes & ClipboardType.Text) == ClipboardType.Text) {
				Debug.WriteLine(">> {0}", ClipboardTypes);
				return ClipboardType.Text;
			}
			if((ClipboardTypes & ClipboardType.Image) == ClipboardType.Image) {
				Debug.WriteLine(">> {0}", ClipboardTypes);
				return ClipboardType.Image;
			}

			Debug.Assert(false, ClipboardTypes.ToString());
			throw new NotImplementedException();
		}
	}

	[Serializable]
	public class ClipboardSetting: Item
	{
		public ClipboardSetting()
		{
			Items = new FixedSizedList<ClipboardItem>(Literal.clipboardLimit);
			EnabledApplicationCopy = false;
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 3,
				Screen.PrimaryScreen.Bounds.Height / 3
			);
			var screenArea = Screen.PrimaryScreen.WorkingArea;
			Location = new Point(screenArea.X, screenArea.Height - Size.Height);
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
		public bool TopMost { get; set; }
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
