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
				map[TemplateTextLanguageName.clipboard] = clipboardText;

				var lines = clipboardText.SplitLines().ToList();
				// 改行を削除
				map[TemplateTextLanguageName.clipboardNobreak] = string.Join(string.Empty, lines);
				// 先頭行
				map[TemplateTextLanguageName.clipboardHead] = lines.FirstOrDefault();
				// 最終行
				map[TemplateTextLanguageName.clipboardTail] = lines.LastOrDefault();
			}

			return map;
		}

		/// <summary>
		/// テンプレートアイテムからテンプレートプロセッサ作成。
		/// </summary>
		/// <param name="item">テンプレートアイテム。テンプレートプロセッサが設定される。</param>
		/// <param name="language">使用言語。</param>
		/// <returns>作成されたテンプレートプロセッサ。</returns>
		public static TemplateProcessor MakeTemplateProcessor(TemplateItem item, Language language)
		{
			Debug.Assert(item.ReplaceMode);
			Debug.Assert(item.Program);

			if(item.Processor != null) {
				item.Processor.Language = language;
				item.Processor.TemplateSource = item.Source;

				return item.Processor;
			}

			var processor = new TemplateProcessor() {
				Language = language,
				TemplateSource = item.Source,
			};

			item.Processor = processor;

			return processor;
		}

		public static string ToPlainText(TemplateItem item, Language language)
		{
			if(!item.ReplaceMode) {
				return item.Source;
			}
			if(item.Program) {
				var process = MakeTemplateProcessor(item, language);
				if(process.Compiled) {
					return process.TransformText();
				}
				process.AllProcess();
				if(process.GeneratedErrorList.Any() || process.CompileErrorList.Any()) {
					// エラーあり
					return string.Join(Environment.NewLine, process.GeneratedErrorList.Concat(process.CompileErrorList).Select(e => e.ToString()));
				}
				return process.TransformText();
			} else {
				var replacedText = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), null);
				Debug.WriteLine("replacedText: " + replacedText);
				return replacedText;
			}
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

				if(item.Program) {
					richTextBox.Text = ToPlainText(item, language);
					return richTextBox.Rtf;
				} else {
					var AsciiSTX = '\u0002';
					var AsciiETX = '\u0003';
					var evilReplace = new Tuple<char, char>(AsciiSTX, AsciiETX);

					var rtfBoldHead = @"\b ";
					var rtfBoldTail = @"\b0 ";

					var replacedEvil = language.ReplaceAllAppMap(item.Source, GetTemplateMap(), evilReplace);

					richTextBox.Text = replacedEvil;

					// ちょっちあれな部分を書式設定
					var esc = @"\'";
					var map = new Dictionary<string, string>() {
						{ esc + string.Format("{0:x2}", (int)evilReplace.Item1), rtfBoldHead },
						{ esc + string.Format("{0:x2}", (int)evilReplace.Item2), rtfBoldTail },
					};
					var rtf = richTextBox.Rtf.ReplaceFromDictionary(map);

					return rtf;
				}

			}
		}
	}
}
