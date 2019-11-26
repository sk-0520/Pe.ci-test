using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class HeadAndTailTest
    {
        [TestMethod]
        [DataRow(false, 4, 5, 10)]
        [DataRow(true, 5, 5, 10)]
        [DataRow(true, 8, 5, 10)]
        [DataRow(true, 10, 5, 10)]
        [DataRow(false, 11, 5, 10)]
        public void CompareTest(bool result, int value, int head, int tail)
        {
            var actual = HeadAndTail.Create(head, tail);
            Assert.AreEqual(result, actual.IsIn(value));
        }
    }
}
