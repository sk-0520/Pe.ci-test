namespace ContentTypeTextNet.Pe.PeMain.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Drawing;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using System.Windows.Forms;
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
			var result = language.ReplaceAllWithAppMap(item.Source, GetTemplateMap(), false);
			Debug.WriteLine(result);
			return result;
		}

		public static string ToRtf(TemplateItem item, Language language, Font font, bool evil)
		{
			using(var richTextBox = new RichTextBox()) {
				richTextBox.Text = item.Source;
				richTextBox.Font = font;
				if(!item.ReplaceMode) {
					return richTextBox.Rtf;
				}
				var trf = richTextBox.Rtf;

				var result = language.ReplaceAllWithAppMap(item.Source, GetTemplateMap(), evil);

				Debug.WriteLine(result);

				return trf;
			}
		}
	}
}
