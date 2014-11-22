using System;
using System.Collections.Generic;
using NUnit.Framework;
using PeUtility;

namespace PeUtilityTest
{
	[TestFixture]
	public class TextUtilityTest
	{
		[TestCase("a", new [] { "a" }, null, "a(2)")]
		public void ToUnique(string src, IEnumerable<string> list, Func<string, int, string> dg, string result)
		{
			Assert.IsTrue(TextUtility.ToUnique(src, list, dg) == result);
		}
	}
}
