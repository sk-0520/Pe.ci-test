using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ContentTypeTextNet.Pe.PeMain.Data;

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

	}
}
