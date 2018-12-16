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
            Assert.AreEqual(C.TestMember1, enumTransfer.From("testmember1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.From("testmember2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.From("testmember3"));

            Assert.AreEqual(C.TestMember1, enumTransfer.From("test-member-1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.From("test-member-2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.From("test-member-3"));

            Assert.AreEqual(C.TestMember1, enumTransfer.From("TestMember1"));
            Assert.AreEqual(C.TestMember2, enumTransfer.From("TestMember2"));
            Assert.AreEqual(C.TestMember3, enumTransfer.From("TestMember3"));
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
            Assert.AreEqual(D.TestMember1, enumTransfer.From("test1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.From("TEST2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.From("3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.From("testmember1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.From("testmember2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.From("testmember3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.From("test-member-1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.From("test-member-2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.From("test-member-3"));

            Assert.AreEqual(D.TestMember1, enumTransfer.From("TestMember1"));
            Assert.AreEqual(D.TestMember2, enumTransfer.From("TestMember2"));
            Assert.AreEqual(D.TestMember3, enumTransfer.From("TestMember3"));
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
            Assert.AreEqual(E.A, enumTransfer.From(enumTransfer.To(E.A)));
            Assert.AreEqual(E.B, enumTransfer.From(enumTransfer.To(E.B)));
            Assert.AreEqual(E.C, enumTransfer.From(enumTransfer.To(E.C)));
            Assert.AreEqual(E.D, enumTransfer.From(enumTransfer.To(E.D)));
        }


    }
}
