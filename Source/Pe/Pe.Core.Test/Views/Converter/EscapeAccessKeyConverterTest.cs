using ContentTypeTextNet.Pe.Core.Views.Converter;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Views.Converter
{
    [TestClass]
    public class EscapeAccessKeyConverterTest
    {
        #region function

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("__", "_")]
        [DataRow("____", "__")]
        [DataRow("__a__", "_a_")]
        public void ConvertTest(string expected, object value)
        {
            var converter = new EscapeAccessKeyConverter();
            var actual = converter.Convert(value, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(null, null)]
        [DataRow("", "")]
        [DataRow("_", "_")]
        [DataRow("_", "__")]
        [DataRow("__", "____")]
        [DataRow("_a_", "__a__")]
        public void ConvertBackTest(string expected, object value)
        {
            var converter = new EscapeAccessKeyConverter();
            var actual = converter.ConvertBack(value, value?.GetType() ?? typeof(object), null!, System.Globalization.CultureInfo.CurrentCulture);
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
