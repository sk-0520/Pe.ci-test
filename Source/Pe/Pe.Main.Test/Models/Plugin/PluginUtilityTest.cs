using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Plugin
{
    [TestClass]
    public class PluginUtilityTest
    {
        #region function

        [TestMethod]
        [DataRow(true, 0, 0, 0, 0)]
        [DataRow(true, 0, 0, 0, 1)]
        //[DataRow(true, 0, 0, 0, -1)]
        [DataRow(false, 0, 0, 1, 0)]
        [DataRow(false, 0, 1, 0, 0)]
        [DataRow(false, 0, 1, 1, 0)]
        [DataRow(false, 1, 0, 0, 0)]
        [DataRow(false, 1, 0, 1, 0)]
        [DataRow(false, 1, 1, 0, 0)]
        [DataRow(false, 1, 1, 1, 0)]
        public void IsUnlimitedVersionTest(bool expected, int major, int minor, int build, int revision)
        {
            var ver = new Version(major, minor, build, revision);
            var actual = PluginUtility.IsUnlimitedVersion(ver);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
