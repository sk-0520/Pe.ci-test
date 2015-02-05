namespace ContentTypeTextNet.Pe.PeMain.Data
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;

	/// <summary>
	/// クリップボードのデータ。
	/// </summary>
	public class ClipboardItem: DisposableNameItem
	{
		public ClipboardItem()
		{
			Timestamp = DateTime.Now;
			ClipboardTypes = ClipboardType.None;
		}

		public DateTime Timestamp { get; set; }

		public ClipboardType ClipboardTypes { get; set; }
		public string Text { get; set; }
		public string Rtf { get; set; }
		public string Html { get; set; }
		public Image Image { get; set; }
		public IEnumerable<string> Files { get; set; }

		#region DisposableNameItem

		protected override void Dispose(bool disposing)
		{
			Image.ToDispose();
			base.Dispose(disposing);
		}

		#endregion

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
