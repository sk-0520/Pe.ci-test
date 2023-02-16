using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test.Models
{
    public class Get { int Property { get; } = 1; }
    public class GetSet { int Property { get; set; } }

    [TestClass]
    public class PropertyFactoryTest
    {
        #region function

        [TestMethod]
        public void CreateGetterTest()
        {
            var gi = new Get();
            var go = PropertyExpressionFactory.CreateOwner(gi);
            var pgetter = PropertyExpressionFactory.CreateGetter<Get, int>(go, "Property");
            var gi1 = pgetter(gi);
            Assert.AreEqual(1, gi1);
        }

        [TestMethod]
        public void CreateSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var pgetter = PropertyExpressionFactory.CreateGetter<GetSet, int>(gso, "Property");
            var psetter = PropertyExpressionFactory.CreateSetter<GetSet, int>(gso, "Property");
            psetter(gsi, 10);
            var gi1 = pgetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateObjectGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var pgetter = PropertyExpressionFactory.CreateGetter<GetSet, object>(gso, "Property");
            var psetter = PropertyExpressionFactory.CreateSetter<GetSet, object>(gso, "Property");
            psetter(gsi, 10);
            var gi1 = pgetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateDelegateGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var pgetter = PropertyExpressionFactory.CreateGetter(gso, "Property");
            var psetter = PropertyExpressionFactory.CreateSetter(gso, "Property");
            psetter.DynamicInvoke(gsi, 10);
            var gi1 = pgetter.DynamicInvoke(gsi);
            Assert.AreEqual(10, gi1);
        }

        #endregion
    }

    [TestClass]
    public class PropertyAccesser
    {
        #region function

        [TestMethod]
        public void GetterTest()
        {
            var gi = new Get();
            var gp = new PropertyAccesser<Get, int>(gi, "Property");
            var gi1 = gp.Get(gi);
            Assert.AreEqual(1, gi1);
        }

        [TestMethod]
        public void SetterTest()
        {
            {
                var gi = new Get();
                var gp = new PropertyAccesser<Get, int>(gi, "Property");
                Assert.IsFalse(gp.PropertyInfo.CanWrite);
                Assert.ThrowsException<NotSupportedException>(() => gp.Set(gi, 100));
            }

            {
                var gsi = new GetSet();
                var gsp = new PropertyAccesser<GetSet, int>(gsi, "Property");
                Assert.IsTrue(gsp.PropertyInfo.CanWrite);
                gsp.Set(gsi, 100);
                var gsi1 = gsp.Get(gsi);
                Assert.AreEqual(100, gsi1);
            }

        }

        #endregion
    }

}
