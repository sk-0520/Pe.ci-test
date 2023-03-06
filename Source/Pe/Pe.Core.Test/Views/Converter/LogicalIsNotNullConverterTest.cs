using System;
using ContentTypeTextNet.Pe.Core.Views.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    [TestClass]
    public class LogicalIsNotNullConverterTest
    {
        #region function

        [TestMethod]
        [DataRow(false, null)]
        [DataRow(true, 1)]
        [DataRow(true, 0)]
        [DataRow(true, 0.0)]
        [DataRow(true, "")]
        [DataRow(true, ' ')]
        public void ConvertTest(bool expected, object value)
        {
            var converter = new LogicalIsNotNullConverter();
            var actual = converter.Convert(value, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ConvertBackTest()
        {
            var converter = new LogicalIsNotNullConverter();
            Assert.ThrowsException<NotSupportedException>(() => converter.ConvertBack(default!, default!, default!, System.Globalization.CultureInfo.CurrentCulture));
        }

        #endregion
    }
}
