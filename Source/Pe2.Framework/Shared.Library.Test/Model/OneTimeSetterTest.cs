using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class OneTimeSetterTest
    {
        [TestMethod]
        public void New_Test()
        {
            var ots_int = new OneTimeSetter<int>();
            Assert.IsFalse(ots_int.IsSetted);
            Assert.AreEqual(ots_int.Value, default(int));
            ots_int.Value = 123;
            Assert.IsTrue(ots_int.IsSetted);
            Assert.AreEqual(123, ots_int.Value);
            Assert.ThrowsException<InvalidOperationException>(() => ots_int.Value = 456);

            var ots_str = new OneTimeSetter<string>();
            Assert.IsFalse(ots_str.IsSetted);
            Assert.AreEqual(ots_str.Value, default(string));
            ots_str.Value = "abc";
            Assert.IsTrue(ots_str.IsSetted);
            Assert.AreEqual("abc", ots_str.Value);
            Assert.ThrowsException<InvalidOperationException>(() => ots_str.Value = "xyz");

        }

        [TestMethod]
        public void New_True_Test()
        {
            var ots_int = new OneTimeSetter<int>(true);
            Assert.IsFalse(ots_int.IsSetted);
            Assert.ThrowsException<InvalidOperationException>(() => ots_int.Value);
            ots_int.Value = 123;
            Assert.IsTrue(ots_int.IsSetted);
            Assert.AreEqual(123, ots_int.Value);
            Assert.ThrowsException<InvalidOperationException>(() => ots_int.Value = 456);

            var ots_str = new OneTimeSetter<string>(true);
            Assert.IsFalse(ots_str.IsSetted);
            Assert.ThrowsException<InvalidOperationException>(() => ots_str.Value);
            ots_str.Value = "abc";
            Assert.IsTrue(ots_str.IsSetted);
            Assert.AreEqual("abc", ots_str.Value);
            Assert.ThrowsException<InvalidOperationException>(() => ots_str.Value = "xyz");

        }
    }
}
