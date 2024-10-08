using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContentTypeTextNet.Pe.Library.Base.Test
{
    public class IFlushableExtensions
    {
        #region define

        class X: DisposerBase, IFlushable
        {
            #region IFlushable

            public void Flush()
            { }

            #endregion
        }

        #endregion

        #region function

        [Fact]
        public void SafeFlush_null_Test()
        {
            var test = default(X)!;
            var actual = test.SafeFlush();
            Assert.False(actual);
        }

        [Fact]
        public void SafeFlush_disposed_Test()
        {
            var test = new X();
            test.Dispose();
            var actual = test.SafeFlush();
            Assert.False(actual);
        }

        [Fact]
        public void SafeFlushTest()
        {
            var test = new X();
            var actual = test.SafeFlush();
            Assert.True(actual);
        }
        #endregion
    }
}
