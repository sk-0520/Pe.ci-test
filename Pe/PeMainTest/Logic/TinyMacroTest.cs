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
		[TestCase(true, "<%=", "<%=")]
		[TestCase(true, "<%=(", "<%=(")]
		[TestCase(true, "<%=(a", "<%=(a")]
		[TestCase(true, "<%=(a)%>", "=(a)")]
		[TestCase(true, "<%=?(a)%>", "=?(a)")]
		public void Convert_PlainTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase("A<%=trim(    =len(123)   )%>", "A3")]
		[TestCase("<%=trim(    =len(123)   )%>", "3")]
		[TestCase("<%=trim(    =len(123)   ) %>  <%=len(abc)%>", "3  3")]
		[TestCase("<%=mid(=right(=left(abcdefg, 4), 3), 2, 1)%>", "c")]
		[TestCase("<%=rept(=right(=left(abcdefg, 4), 3), 2)%>", "bcdbcd")]
		public void Convert_Multi(string src, string result)
		{
			Assert.IsTrue(TinyMacro.Convert(src) == result);
		}
			 

		[TestCase(true, "<%=len(a)%>", "1")]
		[TestCase(true, "<%=len(a)=length(B)%>", "11")]
		[TestCase(true, "<%=len()%>", "0")]
		[TestCase(true, "<%=len(abc)%>", "3")]
		[TestCase(true, "<%=len(=len(A))%>", "1")]
		[TestCase(true, "<%=len(=len(abcdefghij))%>", "2")]
		[TestCase(true, "<%=len(=len())%>", "1")]
		[TestCase(true, "-<%=len(=len(=len(=len(=len()))))%>+", "-1+")]
		[TestCase(true, "<%=len(あ)%>", "1")]
		[TestCase(true, "<%=len(が)%>", "1")]
		[TestCase(true, "<%=len(ｱ)%>", "1")]
		[TestCase(true, "<%=len(ｱｱ)%>", "2")]
		//[TestCase(true, "<%=len(0)=len(0)%>", "11")]
		public void Convert_LengthTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=trim(a)%>", "a")]
		[TestCase(true, "<%=trim( a)%>", "a")]
		[TestCase(true, "<%=trim( a )%>", "a")]
		[TestCase(true, "<%=trim( a b )%>", "a b")]
		[TestCase(true, "<%=trim( a \r b )%>", "a \r b")]
		[TestCase(true, "<%=trim( a \n b )%>", "a \n b")]
		[TestCase(true, "<%=trim( a \r\n b )%>", "a \r\n b")]
		public void Convert_TrimTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=trimLines(a)%>", "a")]
		[TestCase(true, "<%=trimLines( a)%>", "a")]
		[TestCase(true, "<%=trimLines( a )%>", "a")]
		[TestCase(true, "<%=trimLines( a b )%>", "a b")]
		// Environment.NewLine
		[TestCase(true, "<%=trimLines( a \r b )%>", "a\r\nb")]
		[TestCase(true, "<%=trimLines( a \n b )%>", "a\r\nb")]
		[TestCase(true, "<%=trimLines( a \r\n b )%>", "a\r\nb")]
		public void Convert_TrimLinesTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=line(a, 1)%>", "a")]
		[TestCase(true, "<%=line(a\r\nb\r\nc, 1)%>", "a")]
		[TestCase(true, "<%=line(a\r\nb\r\nc, 2)%>", "b")]
		[TestCase(true, "<%=line(a\r\nb\r\nc, 3)%>", "c")]
		[TestCase(false, "<%=line(a\r\nb\r\nc, 4)%>", "err")]
		public void Convert_LineTest(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=env(temp)%>", "temp")]
		[TestCase(true, "<%=env(%temp)%>", "%temp")]
		//[TestCase(true, "<%=env(%SystemRoot%)%>", @"C:\Windows")]
		public void Convert_Environment(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=left(abc)%>", "a")]
		[TestCase(true, "<%=left(abc,1)%>", "a")]
		[TestCase(true, "<%=left(abc,2)%>", "ab")]
		[TestCase(true, "<%=left(abc,3)%>", "abc")]
		[TestCase(true, "<%=left(abc,0)%>", "")]
		[TestCase(false, "<%=left(abc,-1)%>", "err")]
		[TestCase(false, "<%=left(abc,a)%>", "err")]
		[TestCase(true, "<%=left(=left(abcde,3),2)%>", "ab")]
		[TestCase(true, "<%=left(あいう)%>", "あ")]
		[TestCase(true, "<%=left(あいう,1)%>", "あ")]
		[TestCase(true, "<%=left(あいう,2)%>", "あい")]
		[TestCase(true, "<%=left(あいう,3)%>", "あいう")]
		[TestCase(true, "<%=left(あいう,0)%>", "")]
		[TestCase(false, "<%=left(あいう,-1)%>", "err")]
		[TestCase(true, "<%=left(=left(あいうえお,3),2)%>", "あい")]
		public void Convert_Left(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(true, "<%=right(abc)%>", "c")]
		[TestCase(true, "<%=right(abc,1)%>", "c")]
		[TestCase(true, "<%=right(abc,2)%>", "bc")]
		[TestCase(true, "<%=right(abc,3)%>", "abc")]
		[TestCase(true, "<%=right(abc,0)%>", "")]
		[TestCase(false, "<%=right(abc,-1)%>", "err")]
		[TestCase(false, "<%=right(abc,a)%>", "err")]
		[TestCase(true, "<%=right(=right(abcde,3),2)%>", "de")]
		[TestCase(true, "<%=right(あいう)%>", "う")]
		[TestCase(true, "<%=right(あいう,1)%>", "う")]
		[TestCase(true, "<%=right(あいう,2)%>", "いう")]
		[TestCase(true, "<%=right(あいう,3)%>", "あいう")]
		[TestCase(true, "<%=right(あいう,0)%>", "")]
		[TestCase(false, "<%=right(あいう,-1)%>", "err")]
		[TestCase(true, "<%=right(=right(あいうえお,3),2)%>", "えお")]
		public void Convert_Right(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(false, "<%=mid(abc)%>", "err")]
		[TestCase(true, "<%=mid(abc,2)%>", "")]
		[TestCase(true, "<%=mid(abc,2,1)%>", "b")]
		[TestCase(true, "<%=mid(abc,2,2)%>", "bc")]
		[TestCase(true, "<%=mid(abc,2,3)%>", "bc")]
		[TestCase(true, "<%=mid(abc,3,0)%>", "")]
		[TestCase(true, "<%=mid(abc,3,1)%>", "c")]
		[TestCase(true, "<%=mid(abc,3,2)%>", "c")]
		[TestCase(true, "<%=mid(abc,4,2)%>", "")]
		[TestCase(false, "<%=mid(abc,a,2)%>", "err")]
		public void Convert_Substring(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}

		[TestCase(false, "<%=rept(a)%>", "err")]
		[TestCase(false, "<%=rept(a,)%>", "err")]
		[TestCase(false, "<%=rept(a,b)%>", "err")]
		[TestCase(true, "<%=rept(a,0)%>", "")]
		[TestCase(true, "<%=rept(a,1)%>", "a")]
		[TestCase(true, "<%=rept(a,2)%>", "aa")]
		[TestCase(true, "<%=rept(aa,2)%>", "aaaa")]
		[TestCase(true, "<%=rept(あ,0)%>", "")]
		[TestCase(true, "<%=rept(あ,1)%>", "あ")]
		[TestCase(true, "<%=rept(あ,2)%>", "ああ")]
		public void Convert_Repeat(bool test, string src, string result)
		{
			Assert.IsTrue((TinyMacro.Convert(src) == result) == test);
		}
	}
}
