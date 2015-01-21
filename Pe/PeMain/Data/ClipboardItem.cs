namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.PeMain.Data;

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


		public bool SetClipboardData(ClipboardType enabledTypes)
		{
			var isPlainText = enabledTypes.HasFlag(ClipboardType.Text) && Clipboard.ContainsText(TextDataFormat.Text);
			var isUnicodeText = enabledTypes.HasFlag(ClipboardType.Text) && Clipboard.ContainsText(TextDataFormat.UnicodeText);
			var isRtf = enabledTypes.HasFlag(ClipboardType.Rtf) && Clipboard.ContainsText(TextDataFormat.Rtf);
			var isHtml = enabledTypes.HasFlag(ClipboardType.Html) && Clipboard.ContainsText(TextDataFormat.Html);
			var isImage = enabledTypes.HasFlag(ClipboardType.Image) && Clipboard.ContainsImage();
			var isFile = enabledTypes.HasFlag(ClipboardType.File) && Clipboard.ContainsFileDropList();

			ClipboardTypes = ClipboardType.None;
			if(!isUnicodeText && !isPlainText && !isRtf && !isHtml && !isImage && !isFile) {
				return false;
			}

			if(isUnicodeText || isPlainText) {
				if(isUnicodeText) {
					Text = Clipboard.GetText(TextDataFormat.UnicodeText);
				} else {
					Text = Clipboard.GetText(TextDataFormat.Text);
				}
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
				ClipboardTypes |= ClipboardType.Text | ClipboardType.File;
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
			var list = new[] {
				ClipboardType.Html,
				ClipboardType.Rtf,
				ClipboardType.File,
				ClipboardType.Text,
				ClipboardType.Image,
			};
			foreach(var type in list) {
				if((ClipboardTypes & type) == type) {
					return type;
				}
			}

			Debug.Assert(false, ClipboardTypes.ToString());
			throw new NotImplementedException();
		}
	}
}
