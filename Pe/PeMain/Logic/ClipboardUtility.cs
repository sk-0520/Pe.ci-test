using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.Library.Utility;
using System.Diagnostics;
using System.Threading;

using ObjectDumper;
using ContentTypeTextNet.Pe.PeMain.IF;

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
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
					logger.Puts(LogType.Warning, ex.Message, ex);
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
	}
}
