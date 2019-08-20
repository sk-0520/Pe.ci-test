using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Model
{
    [TestClass]
    public class PathUtilityTest
    {
        [TestMethod]
        [DataRow("a.txt", "a", "txt")]
        [DataRow("a.txt.txt", "a.txt", "txt")]
        [DataRow("a..txt", "a.", "txt")]
        [DataRow("a..txt", "a", ".txt")]
        public void AppendExtensionTest(string test, string path, string ext)
        {
            Assert.AreEqual(test, PathUtility.AppendExtension(path, ext));
        }

        [TestMethod]
        [DataRow("", "", "!")]
        [DataRow("", " ", "!")]
        [DataRow("a", "a", "!")]
        [DataRow("a!", "a?", "!")]
        [DataRow("a?", "a?", "?")]
        [DataRow("a@b@c@d", "a?b\\c*d", "@")]
        [DataRow("a<>b<>c<>d", "a?b\\c*d", "<>")]
        public void ToSafeNameTest(string test, string value, string c)
        {
            Assert.AreEqual(test, PathUtility.ToSafeName(value, v => c));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("", " ")]
        [DataRow("a", "a")]
        [DataRow("a_", "a?")]
        [DataRow("a_", "a?")]
        [DataRow("a_b_c_d", "a?b\\c*d")]
        public void ToSafeNameDefaultTest(string test, string value)
        {
            Assert.AreEqual(test, PathUtility.ToSafeNameDefault(value));
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
        public void HasIconTest(bool test, string value)
        {
            Assert.AreEqual(test, PathUtility.HasIconPath(value));
        }
    }
}
