using System.Collections.Generic;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class SizeConverterTest
    {
        #region function

        [TestMethod]
        [DataRow("0 byte", 0, "{0} {1}", new[] { "byte" })]
        [DataRow("0.00 byte", 0, "{0:0.00} {1}", new[] { "byte" })]
        [DataRow("1023.00 byte", 1023, "{0:0.00} {1}", new[] { "byte", "KB" })]
        [DataRow("1.00 KB", 1024, "{0:0.00} {1}", new[] { "byte", "KB" })]
        public void ConvertHumanReadableByteTest(string expected, long byteSize, string sizeFormat, IReadOnlyList<string> units)
        {
            var sc = new SizeConverter();
            var actual = sc.ConvertHumanReadableByte(byteSize, sizeFormat, units);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
