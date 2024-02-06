using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class PathUtilityTest
    {
        [TestMethod]
        [DataRow("a.txt", "a", "txt")]
        [DataRow("a.txt.txt", "a.txt", "txt")]
        [DataRow("a.txt", "a.", "txt")]
        [DataRow("a.txt", "a", ".txt")]
        [DataRow("a.txt", "a.....", "......txt")]
        [DataRow("a.txt.", "a.....", "......txt.")]
        public void AddExtensionTest(string expected, string path, string ext)
        {
            var actual = PathUtility.AddExtension(path, ext);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "", "!")]
        [DataRow("", " ", "!")]
        [DataRow("a", "a", "!")]
#if OS_WINDOWS
        [DataRow("a!", "a?", "!")]
        [DataRow("a!", "a/", "!")]
        [DataRow("a/", "a/", "/")]
        [DataRow("a!", "a\\", "!")]
        [DataRow("a@b@c@d@e", "a?b\\c*d/e", "@")]
        [DataRow("a<>b<>c<>d<>e", "a?b\\c*d/e", "<>")]
#else
        [DataRow("a?", "a?", "!")]
        [DataRow("a!", "a/", "!")]
        [DataRow("a/", "a/", "/")]
        [DataRow("a\\", "a\\", "!")]
        [DataRow("a?b\\c*d@e", "a?b\\c*d/e", "@")]
        [DataRow("a?b\\c*d<>e", "a?b\\c*d/e", "<>")]
#endif
        public void ToSafeNameTest(string expected, string value, string c)
        {
            var actual = PathUtility.ToSafeName(value, v => c);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        [DataRow("", "")]
        [DataRow("", " ")]
        [DataRow("a", "a")]
#if OS_WINDOWS
        [DataRow("a_", "a?")]
        [DataRow("a_b_c_d_e", "a?b\\c*d/e")]
#else
        [DataRow("a?", "a?")]
        [DataRow("a?b\\c*d_e", "a?b\\c*d/e")]
#endif
        public void ToSafeNameDefaultTest(string expected, string value)
        {
            var actual = PathUtility.ToSafeNameDefault(value);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
#if OS_WINDOWS
        [DataRow(false, "exe")]
        [DataRow(false, "dll")]
        [DataRow(true, ".exe")]
        [DataRow(true, ".dll")]
        [DataRow(false, ".ico")]
        [DataRow(true, "a.exe")]
        [DataRow(true, "a.dll")]
        [DataRow(false, "a.ico")]
#else
        [DataRow(false, "exe")]
        [DataRow(false, "dll")]
        [DataRow(false, ".exe")]
        [DataRow(false, ".dll")]
        [DataRow(false, ".ico")]
        [DataRow(false, "a.exe")]
        [DataRow(false, "a.dll")]
        [DataRow(false, "a.ico")]
#endif
        public void HasIconTest(bool expected, string value)
        {
            var actual = PathUtility.HasIconPath(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
#if OS_WINDOWS
        [DataRow(false, @"")]
        [DataRow(false, @"/")]
        [DataRow(false, @"//")]
        [DataRow(false, @"\")]
        [DataRow(false, @"\\")]
        [DataRow(true, @"\\a")]
#else
        [DataRow(false, @"")]
        [DataRow(false, @"/")]
        [DataRow(false, @"//")]
        [DataRow(false, @"\")]
        [DataRow(false, @"\\")]
        [DataRow(false, @"\\a")]
#endif
        public void IsNetworkDirectoryPathTest(bool expected, string value)
        {
            var actual = PathUtility.IsNetworkDirectoryPath(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
#if OS_WINDOWS
        [DataRow(null, @"")]
        [DataRow(null, @"A")]
        [DataRow(null, @"\")]
        [DataRow("a", @"\a")]
        [DataRow("a", @"\\a")]
        [DataRow(null, @"\\a\")]
        [DataRow("b", @"\\a\b")]
        [DataRow("b", @"\\a\\b")]
#else
        [DataRow(null, @"")]
        [DataRow(null, @"A")]
        [DataRow(null, @"\")]
        [DataRow(null, @"\a")]
        [DataRow(null, @"\\a")]
        [DataRow(null, @"\\a\")]
        [DataRow(null, @"\\a\b")]
        [DataRow(null, @"\\a\\b")]
#endif
        public void GetNetworkDirectoryName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkDirectoryName(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
#if OS_WINDOWS
        [DataRow(null, @"")]
        [DataRow(null, @"a")]
        [DataRow("", @"\a")]
        [DataRow(@"\a", @"\a\")]
        [DataRow(@"\a", @"\a\b")]
        [DataRow(@"\a\b", @"\a\b\c")]
#else
        [DataRow(null, @"")]
        [DataRow(null, @"a")]
        [DataRow(null, @"\a")]
        [DataRow(null, @"\a\")]
        [DataRow(null, @"\a\b")]
        [DataRow(null, @"\a\b\c")]
#endif
        public void GetNetworkOwnerName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkOwnerName(value);
            Assert.AreEqual(expected, actual);
        }

#if OS_WINDOWS

        [TestMethod]
        [DataRow(false, null)]
        [DataRow(false, @"")]
        [DataRow(false, @"A")]
        [DataRow(true, @"\A")]
        [DataRow(true, @"\\A")]
        [DataRow(true, @"\\A")]
        [DataRow(true, @"\\A\")]
        [DataRow(false, @"\\A\B")]
        [DataRow(true, @"\\C:")]
        [DataRow(true, @"\\C:\")]
        [DataRow(false, @"\\C:\B")]
        [DataRow(true, @"C:")]
        [DataRow(true, @"C:\")]
        [DataRow(false, @"C:\A")]
        public void IsRootNameTest(bool expected, string? value)
        {
            var actual = PathUtility.IsRootName(value);
            Assert.AreEqual(expected, actual);
        }

#endif

        [TestMethod]
        [DataRow(true, null, null)]
        [DataRow(false, "", null)]
        [DataRow(false, null, "")]
        [DataRow(true, "", "")]
#if OS_WINDOWS
        [DataRow(true, "a", "A")]
        [DataRow(true, "A", "a")]
        [DataRow(true, "A", "A")]
        [DataRow(false, "A", "B")]
        [DataRow(true, "A\\", "A\\")]
        [DataRow(true, "A\\", "a\\")]
        [DataRow(true, "A\\", "a")]
        [DataRow(true, "A", "a\\")]
        [DataRow(true, "A\\", "A/")]
        [DataRow(true, "A/", "A/")]
        [DataRow(true, "A\\", "a/")]
        [DataRow(true, "A\\", "a")]
        [DataRow(true, "A", "a/")]
#endif
        public void IsEqualsTest(bool expected, string a, string b)
        {
            var actual = PathUtility.IsEquals(a, b);
            Assert.AreEqual(expected, actual);
        }

#if OS_WINDOWS

        [TestMethod]
        [DataRow(new[] { "a" }, "a")]
        [DataRow(new[] { "a", "b" }, "a/b")]
        [DataRow(new[] { "a", "b", "c" }, "a/b/c")]
        [DataRow(new[] { "a", "b", "c" }, "/a/b/c")]
        [DataRow(new[] { "a", "b", "c" }, "a/b/c/")]
        [DataRow(new[] { "a", "b", "c" }, "/a/b/c/")]
        [DataRow(new[] { "a", "b", "c" }, "\\a\\b\\c\\")]
        public void SplitTest(string[] expected, string input)
        {
            var actual = PathUtility.Split(input);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(@"", @"", @"")]
        [DataRow(@"a", @"a", @"")]
        [DataRow(@"A", @"", @"A")]
        [DataRow(@"C:\dir", @"C:\", @"dir")]
        [DataRow(@"C:\dir\sub", @"C:\dir", @"sub")]
        [DataRow(@"C:\dir\sub", @"C:\dir\", @"\sub")]
        [DataRow(@"C:\dir\sub\sub2", @"C:\dir\", @"sub/./sub2")]
        [DataRow(@"C:\dir\sub2", @"C:\dir\", @"sub/../sub2")]
        [DataRow(@"C:\dir\sub\sub2", @"C:\dir\", @"sub/./sub2")]
        [DataRow(@"C:\dir\next", @"C:\dir\", @"sub/../../../../next")]
        [DataRow(@"sub2\sub3", @"", @"sub/../../../../sub2/next/../sub3")]
        public void SafeCombineTest(string expected, string directory, string nodes)
        {
            var actual = PathUtility.SafeCombine(directory, nodes);
            Assert.AreEqual(expected, actual);
        }
#endif
    }
}
