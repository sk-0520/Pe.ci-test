namespace ContentTypeTextNet.Pe.PeMain.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Runtime.InteropServices;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows;
	using System.Windows.Interop;
	using System.Windows.Media.Imaging;
	using ContentTypeTextNet.Library.PInvoke.Windows;
	using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms.Utility;
	using ContentTypeTextNet.Library.SharedLibrary.IF;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
	using ContentTypeTextNet.Pe.Library.PeData.Define;
	using ContentTypeTextNet.Pe.Library.PeData.Item;
	using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
	using ContentTypeTextNet.Pe.PeMain.IF;

	public class ClipboardUtility
	{
		#region define
		
		class CaseInsensitiveComparer : IComparer<string>
		{
			public int Compare(string x, string y)
			{
				return string.Compare(x, y, true);
			}
		}

		#endregion

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

		public static void CopyClipboardItem(ClipboardItem clipboardItem, IClipboardWatcher watcher)
		{
			Debug.Assert(clipboardItem.Type != ClipboardType.None);

			var data = new DataObject();
			var typeFuncs = new Dictionary<ClipboardType, Action>() {
				{ ClipboardType.Text, () => data.SetText(clipboardItem.Body.Text, TextDataFormat.UnicodeText) },
				{ ClipboardType.Rtf, () => data.SetText(clipboardItem.Body.Rtf, TextDataFormat.Rtf) },
				{ ClipboardType.Html, () => data.SetText(clipboardItem.Body.Html, TextDataFormat.Html) },
				{ ClipboardType.Image, () => data.SetImage(clipboardItem.Body.Image) },
				{ ClipboardType.File, () => {
					data.SetFileDropList(TextUtility.ToStringCollection(clipboardItem.Body.Files)); 
				}},
			};
			foreach (var type in clipboardItem.GetClipboardTypeList()) {
				typeFuncs[type]();
			}
			CopyDataObject(data, watcher);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <param name="rawHtml"></param>
		/// <param name="encoding"></param>
		/// <returns></returns>
		static string ConvertStringFromRawHtml(RangeModel<int> range, byte[] rawHtml, Encoding encoding)
		{
			if(-1 < range.Head && -1 < range.Tail && range.Head <= range.Tail) {
				var raw = rawHtml.Skip(range.Head).Take(range.Tail - range.Head);
				return Encoding.UTF8.GetString(raw.ToArray());
			}

			return null;
		}

		/// <summary>
		/// <para>UTF-8</para>
		/// </summary>
		/// <param name="range"></param>
		/// <param name="rawHtml"></param>
		/// <returns></returns>
		static string ConvertStringFromDefaultRawHtml(RangeModel<int> range, byte[] rawHtml)
		{
			return ConvertStringFromRawHtml(range, rawHtml, Encoding.UTF8);
		}


		static ClipboardItem CreateClipboardItemFromFramework(ClipboardType enabledTypes, ILogger logger)
		{
			var clipboardItem = new ClipboardItem();

			try {
				var clipboardData = Clipboard.GetDataObject();
				if (clipboardData != null) {
					if (enabledTypes.HasFlag(ClipboardType.Text)) {
						if (clipboardData.GetDataPresent(DataFormats.UnicodeText)) {
							clipboardItem.Body.Text = (string)clipboardData.GetData(DataFormats.UnicodeText);
							clipboardItem.Type |= ClipboardType.Text;
						} else if (clipboardData.GetDataPresent(DataFormats.Text)) {
							clipboardItem.Body.Text = (string)clipboardData.GetData(DataFormats.Text);
							clipboardItem.Type |= ClipboardType.Text;
						}
					}

					if (enabledTypes.HasFlag(ClipboardType.Rtf) && clipboardData.GetDataPresent(DataFormats.Rtf)) {
						clipboardItem.Body.Rtf = (string)clipboardData.GetData(DataFormats.Rtf);
						clipboardItem.Type |= ClipboardType.Rtf;
					}

					if (enabledTypes.HasFlag(ClipboardType.Html) && clipboardData.GetDataPresent(DataFormats.Html)) {
						clipboardItem.Body.Html = (string)clipboardData.GetData(DataFormats.Html);
						clipboardItem.Type |= ClipboardType.Html;
					}

					if (enabledTypes.HasFlag(ClipboardType.Image) && clipboardData.GetDataPresent(DataFormats.Bitmap)) {
						var image = clipboardData.GetData(DataFormats.Bitmap) as BitmapSource;
						if (image != null) {
							var bitmap = BitmapFrame.Create(image);

							clipboardItem.Body.Image = bitmap;
							clipboardItem.Type |= ClipboardType.Image;
						}
					}

					if (enabledTypes.HasFlag(ClipboardType.File) && clipboardData.GetDataPresent(DataFormats.FileDrop)) {
						var files = clipboardData.GetData(DataFormats.FileDrop) as string[];
						if (files != null) {
							var sortedFiles = files.OrderBy(s => s, new CaseInsensitiveComparer());
							clipboardItem.Body.Files.AddRange(sortedFiles);
							clipboardItem.Body.Text = string.Join(Environment.NewLine, sortedFiles);
							clipboardItem.Type |= ClipboardType.Text | ClipboardType.File;
						}
					}
				}
			} catch (COMException ex) {
				logger.Error(ex);
			}

			return clipboardItem;
		}

		/// <summary>
		/// 現在のクリップボードからクリップボードアイテムを生成する。
		/// </summary>
		/// <param name="enabledTypes">取り込み対象とするクリップボード種別。</param>
		/// <returns>生成されたクリップボードアイテム。nullが返ることはない。</returns>
		public static ClipboardItem CreateClipboardItem(ClipboardType enabledTypes, IntPtr hWnd, ILogger logger)
		{
			var clipboardItem = CreateClipboardItemFromFramework(enabledTypes, logger);
			return clipboardItem;
		}

		public static void OutputText(IntPtr hBaseWnd, string outputText, bool usingClipboard, INonProcess nonProcess, IClipboardWatcher clipboardWatcher)
		{
			if (string.IsNullOrEmpty(outputText)) {
				nonProcess.Logger.Information("empty");
				return;
			}

			var windowHandles = new List<IntPtr>();
			var hWnd = hBaseWnd;
			do {
				hWnd = NativeMethods.GetWindow(hWnd, GW.GW_HWNDNEXT);
				windowHandles.Add(hWnd);
			} while (!NativeMethods.IsWindowVisible(hWnd));

			if (hWnd == IntPtr.Zero) {
				nonProcess.Logger.Warning("notfound");
				return;
			}

			NativeMethods.SetForegroundWindow(hWnd);
			if (usingClipboard) {
				// 現在クリップボードを一時退避
				var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.All, hBaseWnd, nonProcess.Logger);
				try {
					ClipboardUtility.CopyText(outputText, clipboardWatcher);
					NativeMethods.SendMessage(hWnd, WM.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
				} finally {
					if (clipboardItem.Type != ClipboardType.None) {
						ClipboardUtility.CopyClipboardItem(clipboardItem, clipboardWatcher);
					}
				}
			} else {
				SendKeysUtility.Send(outputText);
			}
		}
	}
}
