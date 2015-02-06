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

			var clipboardItem = ClipboardUtility.CreateClipboardItem(ClipboardType.Text | ClipboardType.File);
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
			var result = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), null);
			Debug.WriteLine(result);
			return result;
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
				
				var STX = '\u0002';
				var ETX = '\u0003';
				var evil = new Tuple<char, char>(STX, ETX);

				var replacedEvil = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), evil);
				richTextBox.Text = replacedEvil;

				// ちょっちあれな部分を書式設定
				var esc = @"\'";
				var map = new Dictionary<string, string>() {
					{ esc + string.Format("{0:x2}", (int)evil.Item1), @"\b " },
					{ esc + string.Format("{0:x2}", (int)evil.Item2), @"\b0 " },
				};
				var rtf = richTextBox.Rtf.ReplaceFromDictionary(map);

				return rtf;
			}
		}
	}
}
