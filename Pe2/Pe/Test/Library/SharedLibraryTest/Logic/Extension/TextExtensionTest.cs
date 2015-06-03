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
