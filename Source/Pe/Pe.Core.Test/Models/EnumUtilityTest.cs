using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
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
