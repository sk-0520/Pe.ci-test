using System;
using System.ComponentModel;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model.Database
{
    [TestClass]
    public class EnumTransferTest
    {
        enum A
        {
            TestEnum,
            testEnum,
            TEStENUm,
        }

        enum B
        {
            [EnumTransfer("abc")]
            TestEnum,
            [EnumTransfer("abc")]
            testEnum,
            [EnumTransfer("abc")]
            TEStENUm,
        }

        [TestMethod]
        public void ToTest_A()
        {
            var test = "test-enum";
            var enumTransfer = new EnumTransfer<A>();
            Assert.AreEqual(test, enumTransfer.To(A.TestEnum));
            Assert.AreEqual(test, enumTransfer.To(A.testEnum));
            Assert.AreEqual(test, enumTransfer.To(A.TEStENUm));
        }

        [TestMethod]
        public void ToTest_B()
        {
            var test = "abc";
            var enumTransfer = new EnumTransfer<B>();
            Assert.AreEqual(test, enumTransfer.To(B.TestEnum));
            Assert.AreEqual(test, enumTransfer.To(B.testEnum));
            Assert.AreEqual(test, enumTransfer.To(B.TEStENUm));
        }
    }
}
