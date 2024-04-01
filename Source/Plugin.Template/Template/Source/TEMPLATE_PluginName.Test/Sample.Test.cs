using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using Xunit;

namespace TEMPLATE_Namespace.Test
{
    public class SampleTest
    {
        #region function

        [Fact]
        public void SimpleTest()
        {
            Assert.Equal(2, 1 + 1);
        }

        [Theory]
        [InlineData(2, 1)]
        [InlineData(4, 2)]
        [InlineData(6, 3)]
        [InlineData(8, 4)]
        public void ParameterTest(int excepted, int input)
        {
            Assert.Equal(excepted, input + input);
        }

        #endregion
    }
}
