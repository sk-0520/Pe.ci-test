using System;
using System.Collections.Generic;
using NUnit.Framework;
using PeUtility;

namespace PeUtilityTest
{
	[TestFixture]
	public class TextUtilityTest
	{
		[TestCase("a", "a", new[] { "" })]
		[TestCase("a", "a", new[] { "b" })]
		[TestCase("a(2)", "a", new[] { "a" })]
		[TestCase("A", "A", new[] { "A(2)" })]
		public void ToUniqueDefault(string result, string src, IEnumerable<string> list)
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
	}
}
