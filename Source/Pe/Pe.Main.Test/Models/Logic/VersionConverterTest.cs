using System;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class VersionConverterTest
    {
        #region function

        [TestMethod]
        [DataRow("0.00.000", ".", 0, 0, 0, 0)]
        [DataRow("0-00-000", "-", 0, 0, 0, 0)]
        [DataRow("000000", "", 0, 0, 0, 0)]
        [DataRow("0.00.000", ".", 0, 0, 0, 9)]
        [DataRow("1.00.000", ".", 1, 0, 0, 0)]
        [DataRow("1.02.000", ".", 1, 2, 0, 0)]
        [DataRow("1.02.003", ".", 1, 2, 3, 0)]
        [DataRow("11.02.003", ".", 11, 2, 3, 0)]
        [DataRow("11.222.003", ".", 11, 222, 3, 0)]
        [DataRow("11.222.3333", ".", 11, 222, 3333, 0)]
        public void ConvertDisplayVersionTest(string expected, string separator, int major, int minor, int build, int revisio)
        {
            var version = new Version(major, minor, build, revisio);
            var vc = new VersionConverter();
            var actual = vc.ConvertDisplayVersion(version, separator);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("A_1-02-003_B.C", "A", "B", "C")]
        [DataRow("A_1-02-003.C", "A", "", "C")]
        [DataRow("A_1-02-003", "A", "", "")]
        [DataRow("A_1-02-003_B", "A", "B", "")]
        public void ConvertFileNameTest(string expected, string head, string tail, string extension)
        {
            var vc = new VersionConverter();
            var actual = vc.ConvertFileName(head, new Version(1, 2, 3, 4), tail, extension);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertFileNameTest_HeadEmpty()
        {
            var vc = new VersionConverter();
            Assert.ThrowsException<ArgumentException>(() => vc.ConvertFileName(string.Empty, new Version(1, 2, 3, 4), string.Empty, string.Empty));
        }

        #endregion
    }
}
