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
	class LinkExtensionTest
	{
		[TestCase(false, true, 1, 2, 3)]
		[TestCase(true, false, 1, 2, 3)]
		public void IfReveseTest(bool test, bool revese, params object[] array)
		{
			var r = array.IfRevese(revese);
			var rt = Enumerable.SequenceEqual(array, r);
			Assert.True(test == rt);
			Assert.True(Enumerable.SequenceEqual(array, r.IfRevese(revese)));
		}
	}
}
