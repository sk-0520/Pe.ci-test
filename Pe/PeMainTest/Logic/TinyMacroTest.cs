namespace ContentTypeTextNet.Pe.Test.ApplicationTest.Logic
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
		// CR, LFわっかんねーわ！
		//[TestCase(true, "=trimLines( a \r b )", "a\rb")]
		//[TestCase(true, "=trimLines( a \n b )", "a\nb")]
		[TestCase(true, "=trimLines( a \r\n b )", "a\r\nb")]
		public void Convert_TrimLinesTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}
	}
}
