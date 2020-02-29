using System;
using System.Collections.Generic;
using System.Text;
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
        [DataRow("a..txt", "a.", "txt")]
        [DataRow("a..txt", "a", ".txt")]
        public void AppendExtensionTest(string result, string path, string ext)
        {
            var actual = PathUtility.AppendExtension(path, ext);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", "", "!")]
        [DataRow("", " ", "!")]
        [DataRow("a", "a", "!")]
        [DataRow("a!", "a?", "!")]
        [DataRow("a?", "a?", "?")]
        [DataRow("a@b@c@d", "a?b\\c*d", "@")]
        [DataRow("a<>b<>c<>d", "a?b\\c*d", "<>")]
        public void ToSafeNameTest(string result, string value, string c)
        {
            var actual = PathUtility.ToSafeName(value, v => c);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", " ")]
        [DataRow("a", "a")]
        [DataRow("a_", "a?")]
        [DataRow("a_", "a?")]
        [DataRow("a_b_c_d", "a?b\\c*d")]
        public void ToSafeNameDefaultTest(string result, string value)
        {
            var actual = PathUtility.ToSafeNameDefault(value);
            Assert.AreEqual(actual, result);
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
        public void HasIconTest(bool result, string value)
        {
            var actual = PathUtility.HasIconPath(value);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow(false, @"")]
        [DataRow(false, @"/")]
        [DataRow(false, @"//")]
        [DataRow(false, @"\")]
        [DataRow(false, @"\\")]
        [DataRow(true, @"\\a")]
        public void IsNetworkDrivePathTest(bool result, string value)
        {
            var actual = PathUtility.IsNetworkDrivePath(value);
            Assert.AreEqual(result, actual);
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
        public void GetNetworkDirectoryName(string? result, string value)
        {
            var actual = PathUtility.GetNetworkDirectoryName(value);
            Assert.AreEqual(result, actual);
        }
    }
}
