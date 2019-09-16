using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Database
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
            Assert.AreEqual(test, enumTransfer.ToString(A.TestEnum));
            Assert.AreEqual(test, enumTransfer.ToString(A.testEnum));
            Assert.AreEqual(test, enumTransfer.ToString(A.TEStENUm));
        }

        [TestMethod]
        public void ToTest_B()
        {
            var test = "abc";
            var enumTransfer = new EnumTransfer<B>();
            Assert.AreEqual(test, enumTransfer.ToString(B.TestEnum));
            Assert.AreEqual(test, enumTransfer.ToString(B.testEnum));
            Assert.AreEqual(test, enumTransfer.ToString(B.TEStENUm));
        }

        enum C
        {
            TestMember1,
            TestMember2,
            TestMember3,
        }

        [TestMethod]
        public void FromTest_C()
        {
            var enumTransfer = new EnumTransfer<C>();
            Assert.AreEqual(C.TestMember1, enumTransfer.ToEnum("testmember1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.ToEnum("testmember2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.ToEnum("testmember3"));

            Assert.AreEqual(C.TestMember1, enumTransfer.ToEnum("test-member-1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.ToEnum("test-member-2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.ToEnum("test-member-3"));

            Assert.AreEqual(C.TestMember1, enumTransfer.ToEnum("TestMember1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.ToEnum("TestMember2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.ToEnum("TestMember3"));
        }

        enum D
        {
            [EnumTransfer("test1")]
            TestMember1,
            [EnumTransfer("TEST2")]
            TestMember2,
            [EnumTransfer("3")]
            TestMember3,
        }

        [TestMethod]
        public void FromTest_D()
        {
            var enumTransfer = new EnumTransfer<D>();
            Assert.AreEqual(D.TestMember1, enumTransfer.ToEnum("test1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.ToEnum("TEST2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.ToEnum("3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.ToEnum("testmember1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.ToEnum("testmember2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.ToEnum("testmember3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.ToEnum("test-member-1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.ToEnum("test-member-2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.ToEnum("test-member-3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.ToEnum("TestMember1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.ToEnum("TestMember2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.ToEnum("TestMember3"));
        }

        enum E
        {
            A,
            B = 100,
            [EnumTransfer("Z")]
            C = 10,
            D,
        }
        [TestMethod]
        public void FromTest_E()
        {
            var enumTransfer = new EnumTransfer<E>();
            Assert.AreEqual(E.A, enumTransfer.ToEnum(enumTransfer.ToString(E.A)));
            Assert.AreEqual(E.B, enumTransfer.ToEnum(enumTransfer.ToString(E.B)));
            Assert.AreEqual(E.C, enumTransfer.ToEnum(enumTransfer.ToString(E.C)));
            Assert.AreEqual(E.D, enumTransfer.ToEnum(enumTransfer.ToString(E.D)));
        }


    }
}
