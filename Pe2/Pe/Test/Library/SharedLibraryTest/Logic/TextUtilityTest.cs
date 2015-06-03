namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using NUnit.Framework;

	[TestFixture]
	class TextUtilityTest
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

	}
}
