namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class ClipboardUtility
	{
		/// <summary>
		/// コピー処理の親玉。
		/// </summary>
		/// <param name="action">コピー処理。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		static void Copy(Action action, IClipboardWatcher watcher)
		{
			CheckUtility.DebugEnforceNotNull(watcher);

			bool? enabledWatch = null;
			if (!watcher.ClipboardEnabledApplicationCopy) {
				enabledWatch = watcher.ClipboardWatching;
				if (enabledWatch.Value) {
					watcher.ClipboardWatchingChange(false);
				}
			}

			try {
				action();
			} finally {
				if (enabledWatch.HasValue) {
					Debug.Assert(!watcher.ClipboardEnabledApplicationCopy);
					Debug.Assert(watcher != null);

					if (enabledWatch.Value) {
						watcher.ClipboardWatchingChange(true);
					}
				}
			}
		}

		/// <summary>
		/// 文字列をクリップボードへ転写。
		/// </summary>
		/// <param name="text">対象文字列。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyText(string text, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), watcher);
		}

		/// <summary>
		/// RTFをクリップボードへ転写。
		/// </summary>
		/// <param name="rtf">対象RTF。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyRtf(string rtf, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), watcher);
		}

		/// <summary>
		/// HTMLをクリップボードへ転写。
		/// </summary>
		/// <param name="html">対象HTML。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyHtml(string html, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), watcher);
		}

		/// <summary>
		/// 画像をクリップボードへ転写。
		/// </summary>
		/// <param name="image">対象画像。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyImage(BitmapSource image, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetImage(image), watcher);
		}

		/// <summary>
		/// ファイルをクリップボードへ転写。
		/// </summary>
		/// <param name="file">対象ファイル。</param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyFile(IEnumerable<string> file, IClipboardWatcher watcher)
		{
			var sc = TextUtility.ToStringCollection(file);
			Copy(() => Clipboard.SetFileDropList(sc), watcher);
		}

		/// <summary>
		/// 複合データをクリップノードへ転写。
		/// <para>基本的にはvoid CopyClipboardItem(ClipboardItem, ClipboardSetting)を使用する</para>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="watcher">コピー処理をアプリケーション内でどう扱うかの抑制IF。</param>
		public static void CopyDataObject(IDataObject data, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetDataObject(data), watcher);
		}




	}
}
