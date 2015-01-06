using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using ContentTypeTextNet.Pe.Library.Utility;

namespace ContentTypeTextNet.Pe.Test.UtilityTest
{
	[TestFixture]
	public class TextUtilityTest
	{
		[TestCase("a", "a", "")]
		[TestCase("a", "a", "b")]
		[TestCase("a(2)", "a", "a")]
		[TestCase("A", "A", "A(2)")]
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
