using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Standard.Base;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    [TestClass]
    public class YurufuwaExtensionsTest
    {
        [TestMethod]
        public void CountingTest()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting().ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.AreEqual(i, counting[i].Number);
            }
        }

        [TestMethod]
        public void Counting_Base_Test()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            var counting = array.Counting(99).ToArray();

            for(var i = 0; i < array.Length; i++) {
                Assert.AreEqual(i + 99, counting[i].Number);
            }
        }
    }

}
