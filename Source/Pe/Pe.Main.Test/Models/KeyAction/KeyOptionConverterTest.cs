using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.KeyAction
{
    [TestClass]
    public class KeyOptionConverterTest
    {
        #region define

        class KeyOptionConverter : KeyOptionConverterBase
        {
            public new KeyActionOptionAttribute GetAttribute<TEnum>(TEnum value)
                where TEnum : struct, Enum
            {
                return base.GetAttribute(value);
            }
        }

        enum Option
        {
            A,
            [KeyActionOption(typeof(int), "NAME")]
            B,
        }

        #endregion

        #region function

        [TestMethod]
        public void GetAttributeTest()
        {
            var koc = new KeyOptionConverter();
            Assert.ThrowsException<InvalidOperationException>(() => koc.GetAttribute(Option.A));
            Assert.ThrowsException<InvalidOperationException>(() => koc.GetAttribute((Option)(-1)));
            var actual = koc.GetAttribute(Option.B);
            Assert.AreEqual("NAME", actual.OptionName);
            Assert.AreEqual(typeof(int), actual.ToType);
        }

        #endregion
    }

    [TestClass]
    public class ReplaceOptionConverterTest
    {
        [TestMethod]
        public void ToKeyTest_Exception()
        {
            var roc = new ReplaceOptionConverter();
            Assert.ThrowsException<ArgumentException>(() => roc.ToKey(new Dictionary<string, string>()));
            Assert.ThrowsException<ArgumentException>(() => roc.ToKey(new Dictionary<string, string>() { [nameof(KeyActionReplaceOption.ReplaceKey)] = "💩" }));
        }

        [TestMethod]
        [DataRow(Key.None, "")]
        [DataRow(Key.None, "none")]
        [DataRow(Key.A, "A")]
        public void ToKeyTest_Convert(Key result, string input)
        {
            var roc = new ReplaceOptionConverter();
            var map = new Dictionary<string, string>() {
                [nameof(KeyActionReplaceOption.ReplaceKey)] = input,
            };
            var actual = roc.ToKey(map);
            Assert.AreEqual(result, actual);
        }

    }

    [TestClass]
    public class DisableOptionConverterTest
    {
        [TestMethod]
        [DataRow(true, "true")]
        [DataRow(true, "TRUE")]
        [DataRow(false, "false")]
        [DataRow(false, "FALSE")]
        public void ToForeverTest(bool result, string input)
        {
            var doc = new DisableOptionConverter();
            var map = new Dictionary<string, string>() {
                [nameof(KeyActionDisableOption.Forever)] = input,
            };
            var actual = doc.ToForever(map);
            Assert.AreEqual(result, actual);
        }
    }
}
