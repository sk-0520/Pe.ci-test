using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using Xunit;

namespace ContentTypeTextNet.Pe.Bridge.Test.Models.Data
{
    public class NoteCaptionPositionExtensionsTest
    {
        #region function

        [Theory]
        [InlineData(true, NoteCaptionPosition.Top)]
        [InlineData(true, NoteCaptionPosition.Bottom)]
        [InlineData(false, NoteCaptionPosition.Left)]
        [InlineData(false, NoteCaptionPosition.Right)]
        public void IsVerticalTest(bool expected, NoteCaptionPosition input)
        {
            var actual = input.IsVertical();
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData(false, NoteCaptionPosition.Top)]
        [InlineData(false, NoteCaptionPosition.Bottom)]
        [InlineData(true, NoteCaptionPosition.Left)]
        [InlineData(true, NoteCaptionPosition.Right)]
        public void IsHorizontalTest(bool expected, NoteCaptionPosition input)
        {
            var actual = input.IsHorizontal();
            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
