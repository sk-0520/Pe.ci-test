using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Launcher
{
    [TestClass]
    public class LauncherFactoryTest
    {
        #region function

        [TestMethod]
        public void ToCode_Null_Test()
        {
            var lf = new LauncherFactory(new IdFactory(Test.LoggerFactory), Test.LoggerFactory);
            Assert.ThrowsException<ArgumentNullException>(() => lf.ToCode(null!));
        }

        [TestMethod]
        [DataRow("", "")]
        [DataRow("a", "a")]
        [DataRow("a", " a")]
        [DataRow("a", "a ")]
        [DataRow("a", " a ")]
        [DataRow("a", "ａ")]
        [DataRow("0", "０")]
        [DataRow("a_a", "a a")]
        [DataRow("a_a", "a\ra")]
        [DataRow("a.a", "a.a")]
        [DataRow("a_a", "a,a")]
        [DataRow("a[c-7f]a", "a\u007fa")]
        [DataRow("a", "ア")] // 全角から平仮名になる
        [DataRow("a", "ｱ")] // 半角から全角になって平仮名になる
        public void ToCodeTest(string expected, string s)
        {
            var lf = new LauncherFactory(new IdFactory(Test.LoggerFactory), Test.LoggerFactory);
            var actual = lf.ToCode(s);

            Assert.AreEqual(expected, actual, actual);
        }

        #endregion
    }
}
