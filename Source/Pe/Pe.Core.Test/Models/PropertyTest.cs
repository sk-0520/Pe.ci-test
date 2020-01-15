using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class PropertyFactoryTest
    {
        #region define

        public class Get { int Property { get; } = 1; }
        public class GetSet { int Property { get; set; } }

        #endregion

        #region functino

        [TestMethod]
        public void CreateGetterTest()
        {
            var gi = new Get();
            var go = PropertyFactory.CreateOwner(gi);
            var pgetter = PropertyFactory.CreateGetter<Get,int>(go, "Property");
            var gi1 = pgetter(gi);
            Assert.AreEqual(1, gi1);
        }

        [TestMethod]
        public void CreateSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyFactory.CreateOwner(gsi);
            var pgetter = PropertyFactory.CreateGetter<GetSet, int>(gso, "Property");
            var psetter = PropertyFactory.CreateSetter<GetSet, int>(gso, "Property");
            psetter(gsi, 10);
            var gi1 = pgetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateObjectGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyFactory.CreateOwner(gsi);
            var pgetter = PropertyFactory.CreateGetter<GetSet, object>(gso, "Property");
            var psetter = PropertyFactory.CreateSetter<GetSet, object>(gso, "Property");
            psetter(gsi, 10);
            var gi1 = pgetter(gsi);
            Assert.AreEqual(10, gi1);
        }

        [TestMethod]
        public void CreateDelegateGetterSetterTest()
        {
            var gsi = new GetSet();
            var gso = PropertyFactory.CreateOwner(gsi);
            var pgetter = PropertyFactory.CreateGetter(gso, "Property");
            var psetter = PropertyFactory.CreateSetter(gso, "Property");
            psetter.DynamicInvoke(gsi, 10);
            var gi1 = pgetter.DynamicInvoke(gsi);
            Assert.AreEqual(10, gi1);
        }

        #endregion
    }
}
