using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TEMPLATE_Namespace.Test
{
    [TestClass]
    public class SampleTest
    {
        #region function

        [TestMethod]
        public void SimpleTest()
        {
            Assert.AreEqual(2, 1 + 1);
        }

        [TestMethod]
        [DataRow(2, 1)]
        [DataRow(4, 2)]
        [DataRow(6, 3)]
        [DataRow(8, 4)]
        public void ParameterTest(int excepted, int input)
        {
            Assert.AreEqual(excepted, input + input);
        }

        #endregion
    }
}
