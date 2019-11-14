using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.KeyMapping;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyMapping
{
    [TestClass]
    public class KeyMappingConverterTest
    {
        #region function

        [TestMethod]
        [DataRow(Key.A, "a")]
        [DataRow(Key.None, "")]
        [DataRow(Key.None, "none")]
        [DataRow(Key.Enter, "enter")]
        [DataRow(Key.Enter, "return")]
        [DataRow(Key.Return, "return")]
        public void ConvertFromStringTest(Key result, string value)
        {
            var kmc = new KeyMappingConverter();
            var actual = kmc.ConvertFromString(value);
            Assert.AreEqual(result, actual);
        }

        [TestMethod]
        [DataRow("none", Key.None)]
        [DataRow("return", Key.Enter)]
        [DataRow("return", Key.Return)]
        public void ConvertFromKeyTest(string result, Key value)
        {
            var kmc = new KeyMappingConverter();
            var actual = kmc.ConvertFromKey(value);
            Assert.AreEqual(result, actual);
        }
        #endregion
    }
}
