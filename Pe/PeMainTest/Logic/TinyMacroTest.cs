namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
{
	using System;
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

		[TestCase("=trim(    =len(123)   )", "3")]
		public void Convert_Multi(string src, string result)
		{
			Assert.IsTrue(TinyMacro.Convert(src) == result);
		}
			 

		[TestCase(true, "=len(a)", "1")]
		[TestCase(true, "=len(a)=length(B)", "11")]
		[TestCase(true, "=len()", "0")]
		[TestCase(true, "=len(abc)", "3")]
		[TestCase(true, "=len(=len(A))", "1")]
		[TestCase(true, "=len(=len(abcdefghij))", "2")]
		[TestCase(true, "=len(=len())", "1")]
		[TestCase(true, "-=len(=len(=len(=len(=len()))))+", "-1+")]
		public void Convert_LengthTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=trim(a)", "a")]
		[TestCase(true, "=trim( a)", "a")]
		[TestCase(true, "=trim( a )", "a")]
		[TestCase(true, "=trim( a b )", "a b")]
		[TestCase(true, "=trim( a \r b )", "a \r b")]
		[TestCase(true, "=trim( a \n b )", "a \n b")]
		[TestCase(true, "=trim( a \r\n b )", "a \r\n b")]
		public void Convert_TrimTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=trimLines(a)", "a")]
		[TestCase(true, "=trimLines( a)", "a")]
		[TestCase(true, "=trimLines( a )", "a")]
		[TestCase(true, "=trimLines( a b )", "a b")]
		// Environment.NewLine
		[TestCase(true, "=trimLines( a \r b )", "a\r\nb")]
		[TestCase(true, "=trimLines( a \n b )", "a\r\nb")]
		[TestCase(true, "=trimLines( a \r\n b )", "a\r\nb")]
		public void Convert_TrimLinesTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=line(a, 1)", "a")]
		[TestCase(true, "=line(a\r\nb\r\nc, 1)", "a")]
		[TestCase(true, "=line(a\r\nb\r\nc, 2)", "b")]
		[TestCase(true, "=line(a\r\nb\r\nc, 3)", "c")]
		[TestCase(false, "=line(a\r\nb\r\nc, 4)", "")]
		public void Convert_LineTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "=env(temp)", "temp")]
		[TestCase(true, "=env(%temp)", "%temp")]
		//[TestCase(true, "=env(%SystemRoot%)", @"C:\Windows")]
		public void Convert_Environment(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}
	}
}
