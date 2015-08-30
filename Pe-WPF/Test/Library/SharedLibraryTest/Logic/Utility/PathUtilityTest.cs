namespace ContentTypeTextNet.Test.Library.SharedLibraryTest.Logic.Utility
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ContentTypeTextNet.Library.SharedLibrary.Logic;
	using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
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

		[TestCase("", "", "!")]
		[TestCase("", " ", "!")]
		[TestCase("a", "a", "!")]
		[TestCase("a!", "a?", "!")]
		[TestCase("a?", "a?", "?")]
		[TestCase("a@b@c@d", "a?b\\c*d", "@")]
		[TestCase("a<>b<>c<>d", "a?b\\c*d", "<>")]
		public void ToSafeNameTest(string test, string value, string c)
		{
			Assert.AreEqual(test, PathUtility.ToSafeName(value, v => c));
		}

		[TestCase("", "")]
		[TestCase("", " ")]
		[TestCase("a", "a")]
		[TestCase("a_", "a?")]
		[TestCase("a_", "a?")]
		[TestCase("a_b_c_d", "a?b\\c*d")]
		public void ToSafeNameDefaultTest(string test, string value)
		{
			Assert.AreEqual(test, PathUtility.ToSafeNameDefault(value));
		}

		[TestCase(false, "exe")]
		[TestCase(false, "dll")]
		[TestCase(true, ".exe")]
		[TestCase(true, ".dll")]
		[TestCase(false, ".ico")]
		[TestCase(true, "a.exe")]
		[TestCase(true, "a.dll")]
		[TestCase(false, "a.ico")]
		public void HasIconTest(bool test, string value)
		{
			Assert.AreEqual(test, PathUtility.IsIconPath(value));
		}
	}
}
