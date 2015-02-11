namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Collections.Specialized;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.IF;
	using ContentTypeTextNet.Pe.PeMain.Kind;

	/// <summary>
	/// クリップボード関係の共通処理。
	/// </summary>
	public class ClipboardUtility
	{
		static void Copy(Action action, ClipboardSetting clipboardSetting)
		{
			//var prevCopy = false;
			if(clipboardSetting != null) {
				//prevCopy = commonData.MainSetting.Clipboard.DisabledCopy;
				clipboardSetting.DisabledCopy = !clipboardSetting.EnabledApplicationCopy;
				//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
			}
			action();
			if(clipboardSetting != null) {
				Task.Factory.StartNew(() => {
					Thread.Sleep(clipboardSetting.SleepTime);
					clipboardSetting.DisabledCopy = !clipboardSetting.DisabledCopy;
					//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
				});
			}
		}

		/// <summary>
		/// 文字列をクリップボードへ転写。
		/// </summary>
		/// <param name="text">対象文字列。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyText(string text, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), clipboardSetting);
		}

		/// <summary>
		/// RTFをクリップボードへ転写。
		/// </summary>
		/// <param name="rtf">対象RTF。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyRtf(string rtf, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), clipboardSetting);
		}

		/// <summary>
		/// HTMLをクリップボードへ転写。
		/// </summary>
		/// <param name="html">対象HTML。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyHtml(string html, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), clipboardSetting);
		}

		/// <summary>
		/// 画像をクリップボードへ転写。
		/// </summary>
		/// <param name="image">対象画像。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyImage(Image image, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetImage(image), clipboardSetting);
		}

		/// <summary>
		/// ファイルをクリップボードへ転写。
		/// </summary>
		/// <param name="file">対象ファイル。</param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyFile(IEnumerable<string> file, ClipboardSetting clipboardSetting)
		{
			var sc = new StringCollection();
			sc.AddRange(file.ToArray());
			Copy(() => Clipboard.SetFileDropList(sc), clipboardSetting);
		}

		/// <summary>
		/// 複合データをクリップノードへ転写。
		/// </summary>
		/// <param name="data"></param>
		/// <param name="clipboardSetting">クリップボード設定。</param>
		public static void CopyDataObject(IDataObject data, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetDataObject(data), clipboardSetting);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="range"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		static string ConvertStringFromRawHtml(RangeItem<int> range, byte[] data)
		{
			if(-1 < range.Start && -1 < range.End && range.Start <= range.End) {
				var raw = data.Skip(range.Start).Take(range.End - range.Start);
				return Encoding.UTF8.GetString(raw.ToArray());
			}

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clipboardHtml"></param>
		/// <param name="convertResult"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static bool TryConvertHtmlFromClipbordHtml(string clipboardHtml, out ClipboardHtmlDataItem convertResult, ILogger logger)
		{
			var result = new ClipboardHtmlDataItem();

			//Version:0.9
			//StartHTML:00000213
			//EndHTML:00001173
			//StartFragment:00000247
			//EndFragment:00001137
			//SourceURL:file:///C:/Users/sk/Documents/Programming/SharpDevelop%20Project/Pe/Pe/PeMain/etc/lang/ja-JP.accept.html

			var map = new Dictionary<string, Action<string>>() {
				{ "version", s => result.Version = decimal.Parse(s) },
				{ "starthtml", s => result.Html.Start = int.Parse(s) },
				{ "endhtml", s => result.Html.End = int.Parse(s) },
				{ "startfragment", s => result.Fragment.Start = int.Parse(s) },
				{ "endfragment", s => result.Fragment.End = int.Parse(s) },
				{ "sourceurl", s => result.SourceURL = new Uri(s) },
			};
			var reg = new Regex(@"^\s*(?<KEY>Version|StartHTML|EndHTML|StartFragment|EndFragment|SourceURL)\s*:\s*(?<VALUE>.+?)\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline);
			for(var match = reg.Match(clipboardHtml); match.Success; match = match.NextMatch()) {
				var key = match.Groups["KEY"].Value.ToLower();
				var value = match.Groups["VALUE"].Value;
				try {
					map[key](value);
				} catch(Exception ex) {
					logger.Puts(LogType.Warning, ex.Message, new ExceptionMessage(key, ex));
				}
			}
			//
			//clipboardHtml.
			var rawHtml = Encoding.UTF8.GetBytes(clipboardHtml);
			result.HtmlText = ConvertStringFromRawHtml(result.Html, rawHtml); ;
			result.FragmentText = ConvertStringFromRawHtml(result.Fragment, rawHtml);
			result.SelectionText = ConvertStringFromRawHtml(result.Selection, rawHtml);
			
			convertResult = result;

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="clipboardHtml"></param>
		/// <param name="logger"></param>
		/// <returns></returns>
		public static ClipboardHtmlDataItem ConvertHtmlFromClipbordHtml(string clipboardHtml, ILogger logger)
		{
			ClipboardHtmlDataItem temp;
			TryConvertHtmlFromClipbordHtml(clipboardHtml, out temp, logger);
			return temp;
		}

		/// <summary>
		/// 現在のクリップボードからクリップボードアイテムを生成する。
		/// </summary>
		/// <param name="enabledTypes">取り込み対象とするクリップボード種別。</param>
		/// <returns>生成されたクリップボードアイテム。生成可能な種別がなければnullを返す。</returns>
		public static ClipboardItem CreateClipboardItem(ClipboardType enabledTypes)
		{
			var clipboardItem = new ClipboardItem();

			if(enabledTypes.HasFlag(ClipboardType.Text)) {
				if(Clipboard.ContainsText(TextDataFormat.UnicodeText)) {
					clipboardItem.Text = Clipboard.GetText(TextDataFormat.UnicodeText);
					clipboardItem.ClipboardTypes |= ClipboardType.Text;
				} else if(Clipboard.ContainsText(TextDataFormat.Text)) {
					clipboardItem.Text = Clipboard.GetText(TextDataFormat.Text);
					clipboardItem.ClipboardTypes |= ClipboardType.Text;
				}
			}

			if(enabledTypes.HasFlag(ClipboardType.Rtf) && Clipboard.ContainsText(TextDataFormat.Rtf)) {
				clipboardItem.Rtf = Clipboard.GetText(TextDataFormat.Rtf);
				clipboardItem.ClipboardTypes |= ClipboardType.Rtf;
			}

			if(enabledTypes.HasFlag(ClipboardType.Html) && Clipboard.ContainsText(TextDataFormat.Html)) {
				clipboardItem.Html = Clipboard.GetText(TextDataFormat.Html);
				clipboardItem.ClipboardTypes |= ClipboardType.Html;
			}

			if(enabledTypes.HasFlag(ClipboardType.Image) && Clipboard.ContainsImage()) {
				clipboardItem.Image = Clipboard.GetImage();
				if(clipboardItem.Image != null) {
					clipboardItem.ClipboardTypes |= ClipboardType.Image;
				}
			}

			if(enabledTypes.HasFlag(ClipboardType.File) && Clipboard.ContainsFileDropList()) {
				var files = Clipboard.GetFileDropList().Cast<string>();
				clipboardItem.Files = new List<string>(files);
				clipboardItem.Text = string.Join(Environment.NewLine, files);
				clipboardItem.ClipboardTypes |= ClipboardType.Text | ClipboardType.File;
			}

			if(clipboardItem.ClipboardTypes == ClipboardType.None) {
				clipboardItem.Dispose();
				return null;
			}

			return clipboardItem;
		}

		public static IList<ClipboardItem> FilterClipboardItemList(IReadOnlyList<ClipboardItem> list, ClipboardType types)
		{
			return null;
		}
	}
}
