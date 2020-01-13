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

        //[TestMethod]
        public void CreateGetterTest()
        {
            var g = new Get();
            var go = PropertyFactory.CreateOwner(g);
            var gp = PropertyFactory.GetProperty(go, "Property");
            var pgetter = PropertyFactory.CreateGetter<int>(gp);
            var x = pgetter();
        }

        #endregion
    }
}
