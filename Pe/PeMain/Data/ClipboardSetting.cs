using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
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
		Html  = 0x04,
		Image = 0x08,
		File  = 0x10,
	}

	public class ClipboradWeight
	{
		public ClipboardType ClipboardType { get; set; }
		public int Weight { get; set; }
	}

	/// <summary>
	/// クリップボードのデータ。
	/// </summary>
	public class ClipboardItem: NameItem
	{
		public ClipboardItem()
		{
			Timestamp = DateTime.Now;
		}

		public DateTime Timestamp { get; set; }

		public ClipboardType ClipboardTypes { get; set; }
		public string Text { get; set; }
		public string Rtf { get; set; }
		public string Html { get; set; }
		public Image Image { get; set; }
		public IEnumerable<string> Files { get; set; }


		public bool SetClipboardData()
		{
			var isText = Clipboard.ContainsText(TextDataFormat.Text);
			var isRtf = Clipboard.ContainsText(TextDataFormat.Rtf);
			var isHtml = Clipboard.ContainsText(TextDataFormat.Html);
			var isImage = Clipboard.ContainsImage();
			var isFile = Clipboard.ContainsFileDropList();

			ClipboardTypes = ClipboardType.None;
			if(!isText && !isRtf && !isHtml && !isImage && !isFile) {
				return false;
			}

			if(isText) {
				Text = Clipboard.GetText(TextDataFormat.Text);
				ClipboardTypes |= ClipboardType.Text;
			}
			if(isRtf) {
				Rtf = Clipboard.GetText(TextDataFormat.Rtf);
				ClipboardTypes |= ClipboardType.Rtf;
			}
			if(isHtml) {
				Html = Clipboard.GetText(TextDataFormat.Html);
				ClipboardTypes |= ClipboardType.Html;
			}
			if(isImage) {
				Image = Clipboard.GetImage();
				ClipboardTypes |= ClipboardType.Image;
			}
			if(isFile) {
				var files = Clipboard.GetFileDropList().Cast<string>();
				Files = files;
				Text = string.Join(Environment.NewLine, files);
				ClipboardTypes |=  ClipboardType.Text | ClipboardType.File;
			}

			return true;
		}

		public IEnumerable<ClipboardType> GetClipboardTypeList()
		{
			Debug.Assert(ClipboardTypes != ClipboardType.None);

			var list = new[] {
				ClipboardType.Text,
				ClipboardType.Rtf,
				ClipboardType.Html,
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
			if((ClipboardTypes & ClipboardType.Html) == ClipboardType.Html) {
				return ClipboardType.Html;
			}
			if((ClipboardTypes & ClipboardType.Rtf) == ClipboardType.Rtf) {
				return ClipboardType.Rtf;
			}
			if((ClipboardTypes & ClipboardType.File) == ClipboardType.File) {
				return ClipboardType.File;
			}
			if((ClipboardTypes & ClipboardType.Text) == ClipboardType.Text) {
				return ClipboardType.Text;
			}
			if((ClipboardTypes & ClipboardType.Image) == ClipboardType.Image) {
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
			Visible = false;
			Items = new FixedSizedList<ClipboardItem>(Literal.clipboardLimit);
			EnabledApplicationCopy = false;
			Size = new Size(
				Screen.PrimaryScreen.Bounds.Width / 3,
				Screen.PrimaryScreen.Bounds.Height / 3
			);
			var screenArea = Screen.PrimaryScreen.WorkingArea;
			Location = new Point(screenArea.X, screenArea.Height - Size.Height);
			TextFont = new FontSetting(SystemFonts.DialogFont);

			Enabled = true;
		}

		/// <summary>
		/// クリップボードユーティリティを使用するか。
		/// </summary>
		public bool Enabled { get; set; }

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
		/// <summary>
		/// コピーを検知を無視する
		/// </summary>
		[XmlIgnore]
		public bool DisabledCopy { get; set; }
		///// <summary>
		///// 
		///// </summary>
		//[XmlIgnore]
		//public TimeSpan WaitTime { get; set; }
		//[XmlElement("WaitTime", DataType = "duration")]
		//public string _WaitTime
		//{
		//	get { return XmlConvert.ToString(WaitTime); }
		//	set
		//	{
		//		if(!string.IsNullOrWhiteSpace(value)) {
		//			WaitTime = XmlConvert.ToTimeSpan(value);
		//		}
		//	}
		//}
	}
}
