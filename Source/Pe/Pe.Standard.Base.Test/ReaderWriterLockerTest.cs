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
    public class ReaderWriterLockerTest
    {
        [Fact]
        public void ReadLockTest()
        {
            var rwl = new ReaderWriterLocker();
            Parallel.For(0, 1000, n => {
                using(rwl.BeginRead()) {
                    Assert.True(true);
                    //Console.WriteLine(n);
                }
            });
        }

        [Fact]
        public void WriteLockTest()
        {
            var rwl = new ReaderWriterLocker();
            using(rwl.BeginWrite()) {
                try {
                    using(rwl.BeginRead()) {
                        Assert.True(false);
                    }
                } catch(LockRecursionException) {
                    Assert.True(true);
                }
            }
        }
    }

}
