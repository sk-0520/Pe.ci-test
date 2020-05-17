using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public void GetMembers_Exception_Test()
        {
            Assert.ThrowsException<ArgumentException>(() => EnumUtility.GetMembers<E0>(typeof(E1)));
            EnumUtility.GetMembers<E1>(typeof(E1));
        }

        [TestMethod]
        public void GetMembersTest()
        {
            var e0 = EnumUtility.GetMembers<E0>();
            foreach(var e in e0) {
                CollectionAssert.Contains(Enum.GetValues(typeof(E0)), e);
            }
            Assert.IsFalse(e0.Any());

            var e1 = EnumUtility.GetMembers<E1>();
            foreach(var e in e1) {
                CollectionAssert.Contains(Enum.GetValues(typeof(E1)), e);
            }

            var e2 = EnumUtility.GetMembers<E2>();
            foreach(var e in e2) {
                CollectionAssert.Contains(Enum.GetValues(typeof(E2)), e);
            }

            var e3 = EnumUtility.GetMembers<E3>();
            foreach(var e in e3) {
                CollectionAssert.Contains(Enum.GetValues(typeof(E3)), e);
            }
        }

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

        [TestMethod]
        public void ParseTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => EnumUtility.Parse<E2>(null!, false));
            Assert.ThrowsException<ArgumentException>(() => EnumUtility.Parse<E2>("", false));
            Assert.ThrowsException<ArgumentException>(() => EnumUtility.Parse<E2>("C", false));
            Assert.ThrowsException<ArgumentException>(() => EnumUtility.Parse<E2>("a", false));
            Assert.ThrowsException<ArgumentException>(() => EnumUtility.Parse<E2>("b", false));
            Assert.AreEqual(E2.A, EnumUtility.Parse<E2>("A", false));
            Assert.AreEqual(E2.B, EnumUtility.Parse<E2>("B", false));
            Assert.AreEqual(E2.A, EnumUtility.Parse<E2>("a", true));
            Assert.AreEqual(E2.B, EnumUtility.Parse<E2>("b", true));
        }

        [TestMethod]
        public void TryParseTest()
        {
            Assert.IsTrue(EnumUtility.TryParse<E2>("A", out var result1, false));
            Assert.AreEqual(E2.A, result1);

            Assert.IsTrue(EnumUtility.TryParse<E2>("a", out var result2, true));
            Assert.AreEqual(E2.A, result2);

            Assert.IsFalse(EnumUtility.TryParse<E2>("a", out var result3, false));
            Assert.AreEqual(default, result3);

            Assert.IsTrue(EnumUtility.TryParse<E2>("B", out var result4, false));
            Assert.AreEqual(E2.B, result4);

            Assert.IsTrue(EnumUtility.TryParse<E2>("b", out var result5, true));
            Assert.AreEqual(E2.B, result5);

            Assert.IsFalse(EnumUtility.TryParse<E2>("b", out var result6, false));
            Assert.AreEqual(default, result6);
        }

        #endregion
    }
}
