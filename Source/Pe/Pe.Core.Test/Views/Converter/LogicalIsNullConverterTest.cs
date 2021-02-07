using System;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    [TestClass]
    public class LogicalIsNullConverterTest
    {
        #region function

        [TestMethod]
        [DataRow(true, null)]
        [DataRow(false, 1)]
        [DataRow(false, 0)]
        [DataRow(false, 0.0)]
        [DataRow(false, "")]
        [DataRow(false, ' ')]
        public void ConvertTest(bool expected, object value)
        {
            var converter = new LogicalIsNullConverter();
            var actual = converter.Convert(value, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var converter = new LogicalIsNullConverter();
            Assert.ThrowsException<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
