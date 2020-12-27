using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class MediaUtilityTest
    {
        [TestMethod]
        [DataRow(0x00, 0x00, 0x00, 0x00, 0x00, 0xff, 0xff, 0xff)]
        [DataRow(0x00, 0xff, 0xff, 0xff, 0x00, 0x00, 0x00, 0x00)]
        [DataRow(0x00, 0x10, 0xff, 0xff, 0x00, 0xef, 0x00, 0x00)]
        public void GetNegativeColorTest(int expectedA, int expectedR, int expectedG, int expectedB, int argA, int argR, int argG, int argB)
        {
            var expected = Color.FromArgb((byte)expectedA, (byte)expectedR, (byte)expectedG, (byte)expectedB);
            var arg = Color.FromArgb((byte)argA, (byte)argR, (byte)argG, (byte)argB);

            var result = MediaUtility.GetNegativeColor(arg);

            Assert.AreEqual(expected, result);
        }
    }
}
