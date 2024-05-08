using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Plugin.Theme
{
    public class ColorPair_G
    {
        #region function

        [Fact]
        public void ConstructorTest()
        {
            var actual = new ColorPair<Color>(Colors.Red, Colors.Lime);
            Assert.Equal(Colors.Red, actual.Foreground);
            Assert.Equal(Colors.Lime, actual.Background);
        }

        #endregion
    }

    public class ColorPairTest
    {
        #region function

        [Fact]
        public void CreateTest()
        {
            var actual = ColorPair.Create("abc", "xyz");
            Assert.Equal("abc", actual.Foreground);
            Assert.Equal("xyz", actual.Background);
        }

        #endregion
    }
}
