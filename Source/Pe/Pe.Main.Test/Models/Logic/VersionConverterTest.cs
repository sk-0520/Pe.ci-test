using System;
using System.Collections.Generic;
using System.Text;
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
        public void ConvertDisplayVersionTest(string result, string separator, int major, int minor, int build, int revisio)
        {
            var version = new Version(major, minor, build, revisio);
            var vc = new VersionConverter();
            var actual = vc.ConvertDisplayVersion(version, separator);
            Assert.AreEqual(result, actual);
        }
        #endregion
    }
}
