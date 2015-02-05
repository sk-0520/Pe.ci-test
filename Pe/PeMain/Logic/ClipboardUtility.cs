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

	public class ClipboardUtility
	{
		static void Copy(Action action, CommonData commonData)
		{
			//var prevCopy = false;
			if(commonData != null) {
				//prevCopy = commonData.MainSetting.Clipboard.DisabledCopy;
				commonData.MainSetting.Clipboard.DisabledCopy = !commonData.MainSetting.Clipboard.EnabledApplicationCopy;
				//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
			}
			action();
			if(commonData != null) {
				Task.Factory.StartNew(() => {
					Thread.Sleep(commonData.MainSetting.Clipboard.SleepTime);
					commonData.MainSetting.Clipboard.DisabledCopy = !commonData.MainSetting.Clipboard.DisabledCopy;
					//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
				});
			}
		}

		public static void CopyText(string text, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), commonData);
		}

		public static void CopyRtf(string rtf, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), commonData);
		}
		public static void CopyHtml(string html, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(html, TextDataFormat.Html), commonData);
		}
		public static void CopyImage(Image image, CommonData commonData)
		{
			Copy(() => Clipboard.SetImage(image), commonData);
		}

		public static void CopyFile(IEnumerable<string> file, CommonData commonData)
		{
			var sc = new StringCollection();
			sc.AddRange(file.ToArray());
			Copy(() => Clipboard.SetFileDropList(sc), commonData);
		}

		public static void CopyDataObject(IDataObject data, CommonData commonData)
		{
			Copy(() => Clipboard.SetDataObject(data), commonData);
		}

		static string ConvertStringFromRawHtml(RangeItem<int> range, byte[] data)
		{
			if(-1 < range.Start && -1 < range.End && range.Start <= range.End) {
				var raw = data.Skip(range.Start).Take(range.End - range.Start);
				return Encoding.UTF8.GetString(raw.ToArray());
			}

			return null;
		}

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
					logger.Puts(LogType.Warning, ex.Message, new MessageException(key, ex));
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
				clipboardItem.Files = files;
				clipboardItem.Text = string.Join(Environment.NewLine, files);
				clipboardItem.ClipboardTypes |= ClipboardType.Text | ClipboardType.File;
			}

			if(clipboardItem.ClipboardTypes == ClipboardType.None) {
				clipboardItem.Dispose();
				return null;
			}

			return clipboardItem;
		}
	}
}
