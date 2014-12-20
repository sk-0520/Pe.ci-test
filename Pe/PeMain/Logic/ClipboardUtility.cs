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

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	public class ClipboardUtility
	{
		static void Copy(Action action, CommonData commonData)
		{
			var prevCopy = false;
			if(commonData != null) {
				prevCopy = commonData.MainSetting.Clipboard.DisabledCopy;
				commonData.MainSetting.Clipboard.DisabledCopy = !commonData.MainSetting.Clipboard.EnabledApplicationCopy;
				//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
			}
			action();
			Task.Factory.StartNew(() => {
				Thread.Sleep(Literal.clipboardSleepTime);
				if(commonData != null) {
					commonData.MainSetting.Clipboard.DisabledCopy = prevCopy;
					//Debug.WriteLine(commonData.MainSetting.Clipboard.DisabledCopy);
				}
			});
		}

		public static void CopyText(string text, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), commonData);
		}

		public static void CopyRtf(string rtf, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), commonData);
		}
		public static void CopyHtml(string rtf, CommonData commonData)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Html), commonData);
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

		public static bool TryConvertHtmlFromClipbordHtml(string clipboardHtml, out string convertResult)
		{
			Func<string, string, int> getIndex = (pattern, line) => {
				var reg = new Regex(pattern, RegexOptions.IgnoreCase);
				var match = reg.Match(line);
				if(match.Success && match.Groups.Count == 2) {
					var data = match.Groups[1].Value;
					var result = -1;
					if(int.TryParse(data, out result)) {
						return result;
					}
				}

				return -1;
			};

			var lines = clipboardHtml.SplitLines();
			var head = -1;
			var tail = -1;

			foreach(var line in lines) {
				if(head != -1 && tail != -1) {
					break;
				}
				if(head == -1) {
					head = getIndex("StartHTML:([0-9]+)", line);
				}
				if(tail == -1) {
					tail = getIndex("EndHTML:([0-9]+)", line);
				}
			}
			if(head != -1 && tail != -1) {
				convertResult = new string(clipboardHtml.Take(tail).Skip(head).ToArray());
				return true;
			} else {
				convertResult = string.Empty;
				return false;
			}
		}

		public static string ConvertHtmlFromClipbordHtml(string clipboardHtml)
		{
			string temp;
			TryConvertHtmlFromClipbordHtml(clipboardHtml, out temp);
			return temp;
		}

	}
}
