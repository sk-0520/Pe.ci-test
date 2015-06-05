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


		[TestCase(new[] { 1, 2, 3 }, new[] { 1, 2, 3 }, true)]
		[TestCase(new[] { 1, 2, 3 }, new[] { 3, 2, 1 }, true)]
		[TestCase(new[] { 1, 2, 3 }, new[] { 2, 3, 1 }, true)]
		[TestCase(new[] { 3, 2, 1 }, new[] { 3, 2, 1 }, false)]
		[TestCase(new[] { 3, 2, 1 }, new[] { 1, 2, 3 }, false)]
		[TestCase(new[] { 3, 2, 1 }, new[] { 2, 3, 1 }, false)]
		public void IfOrderByAscTest(int[] result, int[] array, bool orderByAsc)
		{
			var r = array.IfOrderByAsc(k => k, orderByAsc);
			Assert.True(Enumerable.SequenceEqual(result, r));
		}
	}
}
