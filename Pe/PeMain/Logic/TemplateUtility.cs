namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading.Tasks;
	using System.Windows.Forms;
	using ContentTypeTextNet.Pe.Library.Utility;
	using ContentTypeTextNet.Pe.PeMain.Data;

	public static class TemplateUtility
	{
		static IDictionary<string, string> GetTemplateMap()
		{
			var map = new Dictionary<string, string>();

			var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.Text | ClipboardType.File, IntPtr.Zero, new NullLogger());
			if(clipboardItem != null) {
				var clipboardText = clipboardItem.Text;
				// そのまんま
				map[TemplateLanguageName.clipboard] = clipboardText;

				var lines = clipboardText.SplitLines().ToList();
				// 改行を削除
				map[TemplateLanguageName.clipboardNobreak] = string.Join(string.Empty, lines);
				// 先頭行
				map[TemplateLanguageName.clipboardHead] = lines.FirstOrDefault();
				// 最終行
				map[TemplateLanguageName.clipboardTail] = lines.LastOrDefault();
			}

			return map;
		}

		public static string ToPlainText(TemplateItem item, Language language)
		{
			if(!item.ReplaceMode) {
				return item.Source;
			}
			var replacedText = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), null);
			Debug.WriteLine("replacedText: " + replacedText);

			return replacedText;
		}

		public static string ToRtf(TemplateItem item, Language language, FontSetting fontSetting)
		{
			using(var usingFont = new Font(fontSetting.Font.FontFamily, fontSetting.Font.SizeInPoints, default(FontStyle)))
			using(var richTextBox = new RichTextBox()) {
				richTextBox.Font = usingFont;
				
				if(!item.ReplaceMode || string.IsNullOrWhiteSpace(item.Source)) {
					richTextBox.Text = item.Source;
					return richTextBox.Rtf;
				}
				
				var AsciiSTX = '\u0002';
				var AsciiETX = '\u0003';
				var evilReplace = new Tuple<char, char>(AsciiSTX, AsciiETX);

				var AsciiSI = '\u000F';
				var AsciiEO = '\u000E';
				var evilMacro = new Tuple<char, char>(AsciiSI, AsciiEO);

				var rtfBoldHead = @"\b ";
				var rtfBoldTail = @"\b0 ";

				var rtfItalicHead = @"\u ";
				var rtfItalicTail = @"\u0 ";

				var replacedEvil = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), evilReplace);

				richTextBox.Text = replacedEvil;

				// ちょっちあれな部分を書式設定
				var esc = @"\'";
				var map = new Dictionary<string, string>() {
					{ esc + string.Format("{0:x2}", (int)evilReplace.Item1), rtfBoldHead },
					{ esc + string.Format("{0:x2}", (int)evilReplace.Item2), rtfBoldTail },
					{ esc + string.Format("{0:x2}", (int)evilMacro.Item1), rtfItalicHead },
					{ esc + string.Format("{0:x2}", (int)evilMacro.Item2), rtfItalicTail },
				};

				var rtf = richTextBox.Rtf.ReplaceFromDictionary(map);

				return rtf;
			}
		}
	}
}
