using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
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
