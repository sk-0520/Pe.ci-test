using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    [TestClass]
    public class NoteCaptionPositionExtensionsTest
    {
        #region function

        [TestMethod]
        [DataRow(true, NoteCaptionPosition.Top)]
        [DataRow(true, NoteCaptionPosition.Bottom)]
        [DataRow(false, NoteCaptionPosition.Left)]
        [DataRow(false, NoteCaptionPosition.Right)]
        public void IsVerticalTest(bool expected, NoteCaptionPosition input)
        {
            var actual = input.IsVertical();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(false, NoteCaptionPosition.Top)]
        [DataRow(false, NoteCaptionPosition.Bottom)]
        [DataRow(true, NoteCaptionPosition.Left)]
        [DataRow(true, NoteCaptionPosition.Right)]
        public void IsHorizontalTest(bool expected, NoteCaptionPosition input)
        {
            var actual = input.IsHorizontal();
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
