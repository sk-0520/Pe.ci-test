using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Logic
{
    [TestClass]
    public class CronItemSettingFactoryTest
    {
        #region function

        [TestMethod]
        [DataRow("")]
        [DataRow("a")]
        [DataRow("a b")]
        [DataRow("a b c")]
        [DataRow("a b c d")]
        [DataRow("a b c d e")]
        [DataRow("0 1")]
        [DataRow("0 1 2 3 4 5")]
        [DataRow("@")]
        [DataRow("@reboot")]
        [DataRow("@yearly")]
        [DataRow("@annually")]
        [DataRow("@monthly")]
        [DataRow("@weekly")]
        public void Parse_Exception_Test(string value)
        {
            var cisf = new CronItemSettingFactory();
            try {
                cisf.Parse(value);
                Assert.Fail();
            } catch(Exception) {
                Assert.IsTrue(true);
            }

        }

        #endregion
    }

    [TestClass]
    public class CronSchedulerTest
    {
        #region function

        #endregion
    }

}
