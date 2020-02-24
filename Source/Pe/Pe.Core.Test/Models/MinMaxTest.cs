using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class MinMaxTest
    {
        [TestMethod]
        [DataRow(false, 4, 5, 10)]
        [DataRow(true, 5, 5, 10)]
        [DataRow(true, 8, 5, 10)]
        [DataRow(true, 10, 5, 10)]
        [DataRow(false, 11, 5, 10)]
        public void CompareTest(bool result, int value, int head, int tail)
        {
            var actual = MinMax.Create(head, tail);
            Assert.AreEqual(result, actual.IsIn(value));
        }

        [TestMethod]
        [DataRow(1, 2, "1,2")]
        [DataRow(-1, -2, "-1,-2")]
        [DataRow(1, 2, " 1 , 2 ")]
        public void ParseTest(int resultMin, int resultMax, string s)
        {
            var actual = MinMax.Parse<int>(s);
            Assert.AreEqual(resultMin, actual.Minimum);
            Assert.AreEqual(resultMax, actual.Maximum);
        }
    }
}
