using System;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test.Models.Plugin
{
    public class PluginUtilityTest
    {
        #region function

        [Theory]
        [InlineData(true, 0, 0, 0, 0)]
        [InlineData(true, 0, 0, 0, 1)]
        //[InlineData(true, 0, 0, 0, -1)]
        [InlineData(false, 0, 0, 1, 0)]
        [InlineData(false, 0, 1, 0, 0)]
        [InlineData(false, 0, 1, 1, 0)]
        [InlineData(false, 1, 0, 0, 0)]
        [InlineData(false, 1, 0, 1, 0)]
        [InlineData(false, 1, 1, 0, 0)]
        [InlineData(false, 1, 1, 1, 0)]
        public void IsUnlimitedVersionTest(bool expected, int major, int minor, int build, int revision)
        {
            var ver = new Version(major, minor, build, revision);
            var actual = PluginUtility.IsUnlimitedVersion(ver);
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
