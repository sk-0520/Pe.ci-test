using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Compatibility.Forms
{
    public class DrawingUtilityTest
    {
        #region function

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2.9, 2.9)]
        public void Convert_SW_Size_Test(int expectedWidth, int expectedHeight, double width, double height)
        {
            var input = new System.Windows.Size(width, height);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        [Theory]
        [InlineData(0, 0, 0, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(2, 2, 2, 2)]
        public void Convert_SD_Size_Test(double expectedWidth, double expectedHeight, int width, int height)
        {
            var input = new System.Windows.Size(width, height);
            var actual = DrawingUtility.Convert(input);
            Assert.Equal(expectedWidth, actual.Width);
            Assert.Equal(expectedHeight, actual.Height);
        }

        #endregion
    }
}
