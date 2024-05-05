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

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,1,2")]
        public void Parse_throw_Test(string s)
        {
            Assert.Throws<ArgumentException>(() => MinMax.Parse<int>(s));
        }

        [Theory]
        [InlineData(1, 2, "1,2")]
        [InlineData(-1, -2, "-1,-2")]
        [InlineData(1, 2, " 1 , 2 ")]
        public void TryParse_success_Test(int expectedMin, int expectedMax, string s)
        {
            var result = MinMax.TryParse<int>(s, out var actual);
            Assert.True(result);
            Assert.Equal(expectedMin, actual.Minimum);
            Assert.Equal(expectedMax, actual.Maximum);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0")]
        [InlineData("0,1,2")]
        public void TryParse_failure_Test(string s)
        {
            var result = MinMax.TryParse<int>(s, out var actual);
            Assert.False(result);
            Assert.Equal(0, actual.Minimum);
            Assert.Equal(0, actual.Maximum);
        }
    }
}
