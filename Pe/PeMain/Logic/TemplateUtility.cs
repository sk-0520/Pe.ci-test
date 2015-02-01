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
			return new Dictionary<string, string>() {
			};
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
			using(var richTextBox = new RichTextBox()) {
				richTextBox.Font = fontSetting.Font;
				
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
