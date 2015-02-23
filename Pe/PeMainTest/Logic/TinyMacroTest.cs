﻿namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using ContentTypeTextNet.Pe.PeMain.Logic;
	using NUnit.Framework;

	[TestFixture]
	public class TinyMacroTest
	{
		[TestCase(true, "", "")]
		[TestCase(true, "a", "a")]
		[TestCase(true, "=", "=")]
		[TestCase(true, "=(", "=(")]
		[TestCase(true, "=(a", "=(a")]
		[TestCase(true, "=(a)", "=(a)")]
		[TestCase(true, "=?(a)", "=?(a)")]
		public void Convert_PlainTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=LENGTH(a)", "1")]
		[TestCase(true, "=LENGTH(a)=LENGTH(B)", "11")]
		[TestCase(true, "=LENGTH()", "0")]
		[TestCase(true, "=LENGTH(abc)", "3")]
		[TestCase(true, "=LENGTH(=LENGTH(A))", "1")]
		[TestCase(true, "=LENGTH(=LENGTH(abcdefghij))", "2")]
		[TestCase(true, "=LENGTH(=LENGTH())", "1")]
		[TestCase(true, "-=LENGTH(=LENGTH(=LENGTH(=LENGTH(=LENGTH()))))+", "-1+")]
		public void Convert_LengthTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=TRIM(a)", "a")]
		[TestCase(true, "=TRIM( a)", "a")]
		[TestCase(true, "=TRIM( a )", "a")]
		[TestCase(true, "=TRIM( a b )", "a b")]
		[TestCase(true, "=TRIM( a \r b )", "a \r b")]
		[TestCase(true, "=TRIM( a \n b )", "a \n b")]
		[TestCase(true, "=TRIM( a \r\n b )", "a \r\n b")]
		public void Convert_TrimTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}
	}
}
