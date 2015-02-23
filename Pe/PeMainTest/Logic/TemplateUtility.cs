namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Pe.PeMain.Data;
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using NUnit.Framework;

	[TestFixture]
	public class TemplateUtilityTest
	{
		Language CreateLanguage(string name)
		{
			return new Language();
		}

		[TestCase(true, "a", "a", "")]
		[TestCase(true, "abc", "abc", "")]
		[TestCase(true, "@", "@", "")]
		[TestCase(true, "@[APP]", "@[APP]", "")]
		[TestCase(true, "@[A", "@[A", "")]
		[TestCase(true, "@[A]", "@[A]", "")]
		[TestCase(true, "@[]", "@[]", "")]
		public void ToPlainText_Text(bool result, string templateText, string plainText, string langName)
		{
			var lang = CreateLanguage(langName);
			var item = new TemplateItem() {
				Source = templateText,
			};
			var convertedText = TemplateUtility.ToPlainText(item, lang);
			Assert.IsTrue((convertedText == plainText) == result);
		}

		[TestCase(true, "a", "a", "")]
		[TestCase(true, "abc", "abc", "")]
		[TestCase(true, "@", "@", "")]
		[TestCase(true, "@[APPLICATION]", "Pe", "")]
		[TestCase(true, "@[A", "@[A", "")]
		[TestCase(true, "@[A]", "@[A]", "")]
		[TestCase(true, "@[]", "@[]", "")]
		public void ToPlainText_Replace(bool result, string templateText, string plainText, string langName)
		{
			var lang = CreateLanguage(langName);
			var item = new TemplateItem() {
				Source = templateText,
				ReplaceMode = true,
			};
			var convertedText = TemplateUtility.ToPlainText(item, lang);
			Assert.IsTrue((convertedText == plainText) == result);
		}

	}
}
