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
		public void ToUniqueDefaultTest(string test, string src, params string[] list)
		{
			Assert.IsTrue(TextUtility.ToUniqueDefault(src, list) == test);
		}

		[TestCase("a", "a")]
		[TestCase(".*", "*")]
		[TestCase(".", "?")]
		[TestCase("..", "??")]
		public void RegexPatternToWildcardTest(string test, string s)
		{
			Assert.IsTrue(TextUtility.RegexPatternToWildcard(s) == test);
		}

		[TestCase("", "", "<", ">")]
		[TestCase("a", "a", "<", ">")]
		[TestCase("<a", "<a", "<", ">")]
		[TestCase("a>", "a>", "<", ">")]
		[TestCase("<a>", "[a]", "<", ">")]
		[TestCase("<a><b>", "[a][b]", "<", ">")]
		public void ReplaceRangeTest(string src, string result, string head, string tail)
		{
			var conv = TextUtility.ReplaceRange(src, head, tail, s => "[" + s + "]");
			Assert.AreEqual(conv, result);
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
		public void ReplaceRangeFromDictionaryTest(string src, string head, string tail, string result)
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

		[TestCase("a", "A")]
		[TestCase("aa", "AA")]
		[TestCase("az", "Az")]
		[TestCase("Aa", "aA")]
		[TestCase("aAa", "AaA")]
		public void ReplaceFromDictionaryTest(string src, string result)
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
			var conv = TextUtility.ReplaceFromDictionary(src, map);
			Assert.AreEqual(conv, result);
		}

		[TestCase(false, "a")]
		[TestCase(false, "ab")]
		[TestCase(true, "a b")]
		[TestCase(true, " a")]
		[TestCase(true, "a ")]
		[TestCase(true, " a ")]
		[TestCase(false, "あ")]
		[TestCase(false, "☃")]
		public void WhitespaceToQuotationTest(bool hasQ, string s)
		{
			var q = TextUtility.WhitespaceToQuotation(s);
			Assert.IsTrue((q.First() == '"' && q.Last() == '"') == hasQ);
		}

		[TestCase(0, "")]
		[TestCase(1, "a")]
		[TestCase(1, "a\r\n")]
		[TestCase(2, "a\r\nb")]
		[TestCase(2, "a\rb")]
		[TestCase(2, "a\nb")]
		[TestCase(2, " a \r b ")]
		[TestCase(2, " a \n b ")]
		[TestCase(2, " a \r\n b ")]
		public void SplitLinesTest(int result, string s)
		{
			Assert.IsTrue(s.SplitLines().Count() == result);
		}
	}
}
