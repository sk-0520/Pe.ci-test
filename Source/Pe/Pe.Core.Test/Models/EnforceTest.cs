using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class EnforceTest
    {
        [TestMethod]
        public void ThrowIf_Target_Test()
        {
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIf<NullReferenceException>(false));
            Enforce.ThrowIf<NullReferenceException>(true);
            Assert.IsTrue(true);

            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIf<NotImplementedException>(false));
            Enforce.ThrowIf<NotImplementedException>(true);
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThrowIf_Default_Test()
        {
            try {
                Enforce.ThrowIf(1 == 0);
                Assert.Fail();
            } catch(EnforceException ex) {
                Assert.AreEqual("1 == 0", ex.Message);
            }

            try {
                var a = 1;
                var b = 0;
                Enforce.ThrowIf(a == b);
                Assert.Fail();
            } catch(EnforceException ex) {
                Assert.AreEqual("a == b", ex.Message);
            }
        }

        [TestMethod]
        public void ThrowIfNull_Target_Test()
        {
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNull<object, NullReferenceException>(default));
            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNull<object, NotImplementedException>(default));
        }

        [TestMethod]
        public void ThrowIfNull_Default_Test()
        {
            Assert.ThrowsException<EnforceException>(() => Enforce.ThrowIfNull(default(object)));
        }

        [TestMethod]
        public void ThrowIfNullOrEmpty_Target_Test()
        {
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNullOrEmpty<NullReferenceException>(default));
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNullOrEmpty<NullReferenceException>(""));
            Enforce.ThrowIfNullOrEmpty<NullReferenceException>(" ");
            Enforce.ThrowIfNullOrEmpty<NullReferenceException>("a");
            Assert.IsTrue(true);

            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNullOrEmpty<NotImplementedException>(default));
            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNullOrEmpty<NotImplementedException>(""));
            Enforce.ThrowIfNullOrEmpty<NotImplementedException>(" ");
            Enforce.ThrowIfNullOrEmpty<NotImplementedException>("a");
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void ThrowIfNullOrWhiteSpace_Target_Test()
        {
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(default));
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(""));
            Assert.ThrowsException<NullReferenceException>(() => Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>(" "));
            Enforce.ThrowIfNullOrWhiteSpace<NullReferenceException>("a");
            Assert.IsTrue(true);

            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(default));
            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(""));
            Assert.ThrowsException<NotImplementedException>(() => Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>(" "));
            Enforce.ThrowIfNullOrWhiteSpace<NotImplementedException>("a");
            Assert.IsTrue(true);
        }
    }
}
