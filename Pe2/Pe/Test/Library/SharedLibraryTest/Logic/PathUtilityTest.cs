namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using NUnit.Framework;

	[TestFixture]
	class PathUtilityTest
	{
		[TestCase("a.txt", "a", "txt")]
		[TestCase("a.txt.txt", "a.txt", "txt")]
		[TestCase("a..txt", "a.", "txt")]
		[TestCase("a..txt", "a", ".txt")]
		public void AppendExtensionTest(string test, string path, string ext)
		{
			Assert.AreEqual(test, PathUtility.AppendExtension(path, ext));
		}

		[TestCase("", "", '!')]
		[TestCase("", " ", '!')]
		[TestCase("a", "a", '!')]
		[TestCase("a!", "a?", '!')]
		[TestCase("a?", "a?", '?')]
		[TestCase("a@b@c@d", "a?b\\c*d", '@')]
		public void ToSafeNameTest(string test, string s, char c)
		{
			Assert.AreEqual(test, PathUtility.ToSafeName(s, v => c));
		}
	}
}
