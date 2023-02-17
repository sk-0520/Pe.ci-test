using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
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
