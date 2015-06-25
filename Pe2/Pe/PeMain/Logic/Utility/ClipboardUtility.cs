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
		/// <param name="appCopy">コピー処理をクリップボード監視に引き渡すか。</param>
		/// <param name="watcher">コピー処理をクリップボード監視に渡さない場合の抑制IF。</param>
		static void Copy(Action action, bool appCopy, IClipboardWatcher watcher)
		{
			bool? enabledWatch = null;
			if (!appCopy) {
				if (watcher == null) {
					throw new ArgumentNullException("watcher");
				}
				enabledWatch = watcher.ClipboardWatching;
				if (enabledWatch.Value) {
					watcher.ClipboardWatchingChange(false);
				}
			}

			try {
				action();
			} finally {
				if (enabledWatch.HasValue) {
					Debug.Assert(!appCopy);
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
		/// <param name="enabledApplicationCopy">コピー操作を通知するか。</param>
		/// <param name="watcher">抑制IF。</param>
		public static void CopyText(string text, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), enabledApplicationCopy, watcher);
		}

		/// <summary>
		/// RTFをクリップボードへ転写。
		/// </summary>
		/// <param name="rtf">対象RTF。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyRtf(string rtf, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), enabledApplicationCopy, watcher);
		}

		/// <summary>
		/// HTMLをクリップボードへ転写。
		/// </summary>
		/// <param name="html">対象HTML。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyHtml(string html, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), enabledApplicationCopy, watcher);
		}

		/// <summary>
		/// 画像をクリップボードへ転写。
		/// </summary>
		/// <param name="image">対象画像。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyImage(BitmapSource image, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetImage(image), enabledApplicationCopy, watcher);
		}

		/// <summary>
		/// ファイルをクリップボードへ転写。
		/// </summary>
		/// <param name="file">対象ファイル。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyFile(IEnumerable<string> file, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			var sc = TextUtility.ToStringCollection(file);
			Copy(() => Clipboard.SetFileDropList(sc), enabledApplicationCopy, watcher);
		}

		/// <summary>
		/// 複合データをクリップノードへ転写。
		/// <para>基本的にはvoid CopyClipboardItem(ClipboardItem, ClipboardSetting)を使用する</para>
		/// </summary>
		/// <param name="data"></param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyDataObject(IDataObject data, bool enabledApplicationCopy, IClipboardWatcher watcher)
		{
			Copy(() => Clipboard.SetDataObject(data), enabledApplicationCopy, watcher);
		}




	}
}
