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

		class Macro
		{
			public Macro(string name, string rawParam)
			{
				Name = name;
				RawParameter = rawParam;
				if(string.IsNullOrWhiteSpace(rawParam)) {
					ParameterList = new string[0];
				} else {
					ParameterList = rawParam.Split(',').Select(s => s.Trim()).ToArray();
				}
			}

			public string Name { get; set; }
			public string RawParameter { get; set; }
			public IReadOnlyList<string> ParameterList { get; private set; }

			string ExecuteLength()
			{
				return RawParameter.Length.ToString(); ;
			}

			public string Execute()
			{
				var map = new Dictionary<string, Func<string>>() {
					{ "LENGTH", ExecuteLength },
				};
				Func<string> fn;
				if(map.TryGetValue(Name, out fn)) {
					return fn();
				}

				return "#" + Name + "#";
			}
		}

		public static string ConvertFromMacro(string src)
		{
			var reg = new Regex(@"=(?<MACRO>\w+)\((?<PARAMS>.*)?\)");

			var result = reg.Replace(src, (Match m) => {
				var macro = new Macro(
					m.Groups["MACRO"].Value, 
					m.Success ? m.Groups["PARAMS"].Value: string.Empty
				);
				return macro.Execute();
			});

			return result;
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
				var evil = new Tuple<char, char>(AsciiSTX, AsciiETX);

				var rtfBoldHead = @"\b ";
				var rtfBoldTail = @"\b0 ";

				var replacedEvil = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), evil);
				richTextBox.Text = replacedEvil;

				// ちょっちあれな部分を書式設定
				var esc = @"\'";
				var map = new Dictionary<string, string>() {
					{ esc + string.Format("{0:x2}", (int)evil.Item1), rtfBoldHead },
					{ esc + string.Format("{0:x2}", (int)evil.Item2), rtfBoldTail },
				};
				var rtf = richTextBox.Rtf.ReplaceFromDictionary(map);

				return rtf;
			}
		}
	}
}
