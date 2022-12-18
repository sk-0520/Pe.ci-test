using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
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
        [DataRow("a!", "a?", "!")]
        [DataRow("a?", "a?", "?")]
        [DataRow("a@b@c@d", "a?b\\c*d", "@")]
        [DataRow("a<>b<>c<>d", "a?b\\c*d", "<>")]
        public void ToSafeNameTest(string expected, string value, string c)
        {
            var actual = PathUtility.ToSafeName(value, v => c);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", " ")]
        [DataRow("a", "a")]
        [DataRow("a_", "a?")]
        [DataRow("a_", "a?")]
        [DataRow("a_b_c_d", "a?b\\c*d")]
        public void ToSafeNameDefaultTest(string expected, string value)
        {
            var actual = PathUtility.ToSafeNameDefault(value);
            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        [DataRow(false, "exe")]
        [DataRow(false, "dll")]
        [DataRow(true, ".exe")]
        [DataRow(true, ".dll")]
        [DataRow(false, ".ico")]
        [DataRow(true, "a.exe")]
        [DataRow(true, "a.dll")]
        [DataRow(false, "a.ico")]
        public void HasIconTest(bool expected, string value)
        {
            var actual = PathUtility.HasIconPath(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(false, @"")]
        [DataRow(false, @"/")]
        [DataRow(false, @"//")]
        [DataRow(false, @"\")]
        [DataRow(false, @"\\")]
        [DataRow(true, @"\\a")]
        public void IsNetworkDirectoryPathTest(bool expected, string value)
        {
            var actual = PathUtility.IsNetworkDirectoryPath(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(null, @"")]
        [DataRow(null, @"A")]
        [DataRow(null, @"\")]
        [DataRow("a", @"\a")]
        [DataRow("a", @"\\a")]
        [DataRow(null, @"\\a\")]
        [DataRow("b", @"\\a\b")]
        [DataRow("b", @"\\a\\b")]
        public void GetNetworkDirectoryName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkDirectoryName(value);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(null, @"")]
        [DataRow(null, @"a")]
        [DataRow("", @"\a")]
        [DataRow(@"\a", @"\a\")]
        [DataRow(@"\a", @"\a\b")]
        [DataRow(@"\a\b", @"\a\b\c")]
        public void GetNetworkOwnerName(string? expected, string value)
        {
            var actual = PathUtility.GetNetworkOwnerName(value);
            Assert.AreEqual(expected, actual);
        }

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

        [TestMethod]
        [DataRow(true, null, null)]
        [DataRow(false, "", null)]
        [DataRow(false, null, "")]
        [DataRow(true, "", "")]
        [DataRow(true, "a", "A")]
        [DataRow(true, "A", "a")]
        [DataRow(true, "A", "A")]
        [DataRow(false, "A", "B")]
        [DataRow(true, "A\\", "A\\")]
        [DataRow(true, "A\\", "a\\")]
        [DataRow(true, "A\\", "a")]
        [DataRow(true, "A", "a\\")]
        [DataRow(true, "A\\", "A/")]
        [DataRow(true, "A\\", "a/")]
        [DataRow(true, "A\\", "a")]
        [DataRow(true, "A", "a/")]
        public void IsEqualsTest(bool expected, string a, string b)
        {
            var actual = PathUtility.IsEquals(a, b);
            Assert.AreEqual(expected, actual);
        }

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
    }
}
