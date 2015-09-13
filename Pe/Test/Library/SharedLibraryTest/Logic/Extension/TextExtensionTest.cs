namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Extension
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using NUnit.Framework;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;

	[TestFixture]
	class TextExtensionTest
	{
		[TestCase("", "", "<", ">")]
		[TestCase("a", "a", "<", ">")]
		[TestCase("<a", "<a", "<", ">")]
		[TestCase("a>", "a>", "<", ">")]
		[TestCase("<a>", "[a]", "<", ">")]
		[TestCase("<a><b>", "[a][b]", "<", ">")]
		public void ReplaceRangeTest(string src, string result, string head, string tail)
		{
			var conv = src.ReplaceRange(head, tail, s => "[" + s + "]");
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
			Assert.IsTrue(src.ReplaceRangeFromDictionary(head, tail, map) == result);
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
			var conv = src.ReplaceFromDictionary(map);
			Assert.AreEqual(conv, result);
		}
		
		[TestCase(0, null)]
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
