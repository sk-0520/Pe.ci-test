using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
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
            var propertyGetter = PropertyExpressionFactory.CreateGetter<Get, int>(go, "Property");
            var gi1 = propertyGetter(gi);
            Assert.AreEqual(1, gi1);
        }

        [TestMethod]
        public void CreateSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter<GetSet, int>(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter<GetSet, int>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateObjectGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter<GetSet, object>(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter<GetSet, object>(gso, "Property");
            propertySetter(gsi, 10);
            var gi1 = propertyGetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateDelegateGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyExpressionFactory.CreateOwner(gsi);
            var propertyGetter = PropertyExpressionFactory.CreateGetter(gso, "Property");
            var propertySetter = PropertyExpressionFactory.CreateSetter(gso, "Property");
            propertySetter.DynamicInvoke(gsi, 10);
            var gi1 = propertyGetter.DynamicInvoke(gsi);
            Assert.AreEqual(10, gi1);
        }

        #endregion
    }

    [TestClass]
    public class PropertyAccessor
    {
        #region function

        [TestMethod]
        public void GetterTest()
        {
            var gi = new Get();
            var gp = new PropertyAccessor<Get, int>(gi, "Property");
            var gi1 = gp.Get(gi);
            Assert.AreEqual(1, gi1);
        }

        [TestMethod]
        public void SetterTest()
        {
            {
                var gi = new Get();
                var gp = new PropertyAccessor<Get, int>(gi, "Property");
                Assert.IsFalse(gp.PropertyInfo.CanWrite);
                Assert.ThrowsException<NotSupportedException>(() => gp.Set(gi, 100));
            }

            {
                var gsi = new GetSet();
                var gsp = new PropertyAccessor<GetSet, int>(gsi, "Property");
                Assert.IsTrue(gsp.PropertyInfo.CanWrite);
                gsp.Set(gsi, 100);
                var gsi1 = gsp.Get(gsi);
                Assert.AreEqual(100, gsi1);
            }

        }

        #endregion
    }
}
