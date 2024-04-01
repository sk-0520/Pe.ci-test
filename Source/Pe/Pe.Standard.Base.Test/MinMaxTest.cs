using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class MinMaxTest
    {
        [Theory]
        [InlineData(false, 4, 5, 10)]
        [InlineData(true, 5, 5, 10)]
        [InlineData(true, 8, 5, 10)]
        [InlineData(true, 10, 5, 10)]
        [InlineData(false, 11, 5, 10)]
        public void CompareTest(bool expected, int value, int head, int tail)
        {
            var actual = MinMax.Create(head, tail);
            Assert.Equal(expected, actual.IsIn(value));
        }

        [Theory]
        [InlineData(1, 2, "1,2")]
        [InlineData(-1, -2, "-1,-2")]
        [InlineData(1, 2, " 1 , 2 ")]
        public void ParseTest(int expectedMin, int expectedMax, string s)
        {
            var actual = MinMax.Parse<int>(s);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
        }
    }
}
