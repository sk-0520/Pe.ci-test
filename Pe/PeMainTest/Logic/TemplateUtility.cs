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
		public void ToPlainText_Text(bool test, string src, string result, string langName)
		{
			var lang = CreateLanguage(langName);
			var item = new TemplateItem() {
				Source = src,
			};
			var convertedText = TemplateUtility.ToPlainText(item, lang);
			Assert.IsTrue((convertedText == result) == test);
		}

		[TestCase(true, "a", "a", "")]
		[TestCase(true, "abc", "abc", "")]
		[TestCase(true, "@", "@", "")]
		[TestCase(true, "@[APPLICATION]", "Pe", "")]
		[TestCase(true, "@[A", "@[A", "")]
		[TestCase(true, "@[A]", "@[A]", "")]
		[TestCase(true, "@[]", "@[]", "")]
		public void ToPlainText_Replace(bool test, string src, string result, string langName)
		{
			var lang = CreateLanguage(langName);
			var item = new TemplateItem() {
				Source = src,
				ReplaceMode = true,
			};
			var convertedText = TemplateUtility.ToPlainText(item, lang);
			Assert.IsTrue((convertedText == result) == test);
		}

	}
}
