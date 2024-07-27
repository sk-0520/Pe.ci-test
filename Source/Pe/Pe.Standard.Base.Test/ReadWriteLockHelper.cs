using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Standard.Base;
using Xunit;

namespace ContentTypeTextNet.Pe.Standard.Base.Test
{
    public class ReadWriteLockHelper
    {
        [Fact]
        public void ReadLockTest()
        {
            var test = new Base.ReadWriteLockHelper();
            Parallel.For(0, 1000, n => {
                using(test.BeginRead()) {
                    Assert.True(true);
                    //Console.WriteLine(n);
                }
            });
        }

        [Fact]
        public void WriteLockTest()
        {
            var test = new Base.ReadWriteLockHelper();
            using(test.BeginWrite()) {
                try {
                    using(test.BeginRead()) {
                        Assert.True(false);
                    }
                } catch(LockRecursionException) {
                    Assert.True(true);
                }
            }
        }
    }
}
