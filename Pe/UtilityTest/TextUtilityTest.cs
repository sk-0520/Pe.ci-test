namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	using System;
	using System.Linq;
	using System.Collections.Generic;
	using NUnit.Framework;
	using ContentTypeTextNet.Pe.Library.Utility;

	[TestFixture]
	public class TextUtilityTest
	{
		[TestCase("a", "a", "")]
		[TestCase("a", "a", "b")]
		[TestCase("a(2)", "a", "a")]
		[TestCase("A", "A", "A(2)")]
		[TestCase("a(3)", "a", "a(5)", "a(2)", "a(4)", "a")]
		public void ToUniqueDefault(string result, string src, params string[] list)
		{
			Assert.IsTrue(TextUtility.ToUniqueDefault(src, list) == result);
		}

		[TestCase("a", "a")]
		[TestCase(".*", "*")]
		[TestCase(".", "?")]
		[TestCase("..", "??")]
		public void RegexPatternToWildcard(string result, string s)
		{
			Assert.IsTrue(TextUtility.RegexPatternToWildcard(s) == result);
		}

		[TestCase("a", "<", ">", "a")]
		[TestCase("<a>", "<", ">", "A")]
		[TestCase("<aa>", "<", ">", "<aa>")]
		[TestCase("<a><b>", "<", ">", "AB")]
		[TestCase("<a<a>><b>", "<", ">", "<a<a>>B")]
		[TestCase("a", "@[", "]", "a")]
		[TestCase("@[a]", "@[", "]", "A")]
		[TestCase("@[aa]", "@[", "]", "@[aa]")]
		[TestCase("@[a]@[b]", "@[", "]", "AB")]
		[TestCase("@[a@[a]]@[b]", "@[", "]", "@[a@[a]]B")]
		public void ReplaceRangeFromDictionary(string src, string head, string tail, string result)
		{
			var map = new Dictionary<string, string>() {
				{ "A", "a" },
				{ "B", "b" },
				{ "C", "c" },
				{ "D", "d" },
				{ "E", "e" },
				{ "a", "A" },
				{ "b", "B" },
				{ "c", "C" },
				{ "d", "D" },
				{ "e", "E" },
			};
			Assert.IsTrue(TextUtility.ReplaceRangeFromDictionary(src, head, tail, map) == result);
		}

		[TestCase(false, "a")]
		[TestCase(false, "ab")]
		[TestCase(true, "a b")]
		[TestCase(true, " a")]
		[TestCase(true, "a ")]
		[TestCase(true, " a ")]
		[TestCase(false, "あ")]
		[TestCase(false, "☃")]
		public void WhitespaceToQuotation(bool hasQ, string s)
		{
			var q = TextUtility.WhitespaceToQuotation(s);
			Assert.IsTrue((q.First() == '"' && q.Last() == '"') == hasQ);
		}

	}
}
