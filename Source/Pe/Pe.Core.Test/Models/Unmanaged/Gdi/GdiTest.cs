using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged;
using ContentTypeTextNet.Pe.Core.Models.Unmanaged.Gdi;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models.Unmanaged.Gdi
{
    public class GdiTest
    {
        #region define

        private class TestClass: GdiBase
        {
            public TestClass()
                : base(new nint(1))
            { }

            protected override BitmapSource MakeBitmapSourceCore()
            {
                throw new NotImplementedException();
            }

            protected override bool ReleaseHandle()
            {
                return true;
            }
        }

        #endregion

        #region function

        [Fact]
        public void Test()
        {
            var test = new TestClass();
            Assert.False(test.CanMakeImageSource);

            Assert.Equal(new nint(1), test.ResourceHandle);
            Assert.False(test.IsInvalid);
            Assert.Throws<NotSupportedException>(() => test.MakeBitmapSource());
            test.Dispose();
            Assert.True(test.IsInvalid);
            Assert.Equal(nint.Zero, test.ResourceHandle);
        }

        #endregion
    }
}
