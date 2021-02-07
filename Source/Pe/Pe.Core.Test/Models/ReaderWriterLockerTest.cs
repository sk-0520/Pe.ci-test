using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    [TestClass]
    public class ReaderWriterLockerTest
    {
        [TestMethod]
        public void ReadLockTest()
        {
            var rwl = new ReaderWriterLocker();
            Parallel.For(0, 1000, n => {
                using(rwl.BeginRead()) {
                    Assert.IsTrue(true);
                    //Console.WriteLine(n);
                }
            });
        }

        [TestMethod]
        public void WriteLockTest()
        {
            var rwl = new ReaderWriterLocker();
            using(rwl.BeginWrite()) {
                try {
                    using(rwl.BeginRead()) {
                        Assert.IsTrue(false);
                    }
                } catch(LockRecursionException) {
                    Assert.IsTrue(true);
                }
            }
        }
    }
}
