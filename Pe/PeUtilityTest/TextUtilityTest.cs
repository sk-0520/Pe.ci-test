using System;
using System.Collections.Generic;
using NUnit.Framework;
using PeUtility;

namespace PeUtilityTest
{
	[TestFixture]
	public class TextUtilityTest
	{
		[TestCase("a", new[] { "" }, "a")]
		[TestCase("a", new[] { "b" }, "a")]
		[TestCase("a", new[] { "a" }, "a(2)")]
		[TestCase("A", new[] { "A(2)" }, "A")]
		public void ToUniqueDefault(string src, IEnumerable<string> list, string result)
		{
			Assert.IsTrue(TextUtility.ToUniqueDefault(src, list) == result);
		}

		[TestCase("a", "a")]
		[TestCase("*", ".*")]
		[TestCase("?", ".")]
		[TestCase("??", "..")]
		public void RegexPatternToWildcard(string s, string result)
		{
			Assert.IsTrue(TextUtility.RegexPatternToWildcard(s) == result);
		}
	}
}
