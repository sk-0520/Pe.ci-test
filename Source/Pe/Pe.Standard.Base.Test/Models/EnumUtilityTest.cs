using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test.Models
{
    [TestClass]
    public class EnumUtilityTest
    {
        #region define

        enum E0
        { }

        enum E1
        {
            A,
        }

        enum E2
        {
            A,
            B,
        }

        enum E3
        {
            A,
            B,
            C,
        }

        #endregion

        #region function

        [TestMethod]
        public void NormalizeTest()
        {
            Assert.AreEqual(E2.A, EnumUtility.Normalize("A", E2.A));
            Assert.AreEqual(E2.B, EnumUtility.Normalize("B", E2.A));
            Assert.AreEqual(E2.A, EnumUtility.Normalize("C", E2.A));
            Assert.AreEqual(E2.B, EnumUtility.Normalize("C", E2.B));
            Assert.AreEqual(E2.A, EnumUtility.Normalize("💩", E2.A));
            Assert.AreEqual(E2.B, EnumUtility.Normalize("💩", E2.B));
        }

        #endregion
    }

}
