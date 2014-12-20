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

namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	public class ClipboardUtility
	{
		static void Copy(Action action, ClipboardSetting clipboardSetting)
		{
			var precCopy = false;
			if(clipboardSetting != null) {
				precCopy = clipboardSetting.EnabledApplicationCopy;
				clipboardSetting.DisabledCopy = !clipboardSetting.EnabledApplicationCopy;
			}
			action();
			if(clipboardSetting != null) {
				clipboardSetting.DisabledCopy = precCopy;
			}
		}

		public static void CopyText(string text, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(text, TextDataFormat.UnicodeText), clipboardSetting);
		}

		public static void CopyRtf(string rtf, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Rtf), clipboardSetting);
		}
		public static void CopyHtml(string rtf, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetText(rtf, TextDataFormat.Html), clipboardSetting);
		}
		public static void CopyImage(Image image, ClipboardSetting clipboardSetting)
		{
			Copy(() => Clipboard.SetImage(image), clipboardSetting);
		}

		public static void CopyFile(IEnumerable<string> file, ClipboardSetting clipboardSetting)
		{
			var sc = new StringCollection();
			sc.AddRange(file.ToArray());
			Copy(() => Clipboard.SetFileDropList(sc), clipboardSetting);
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
