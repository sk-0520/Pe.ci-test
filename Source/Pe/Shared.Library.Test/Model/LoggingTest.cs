using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Library.Test.Model
{
    [TestClass]
    public class StockLoggerTest
    {
        [TestMethod]
        public void PutTest()
        {
            var logger = new StockLogger();

            logger.Debug("1");
            Assert.AreEqual("1", logger.Items.Last().Message);

            logger.Information("2");
            Assert.AreEqual("2", logger.Items.Last().Message);

            var child = logger.CreateLogger("A").CreateLogger("B");
            child.Warning("3");

            Assert.AreEqual("3", logger.Items.Last().Message);
        }
    }

    [TestClass]
    public class ActionAsyncLoggerTest
    {
        [TestMethod]
        public void PutItemsTest()
        {
            var test = new TestLogger();

            var counter = 0;
            var max = 9999;// 9999;
            var logger = new ActionAsyncLogger(l => {
                Assert.IsTrue(l.Any());
                counter += l.Count;
                foreach(var item in l) {
                    test.Trace(item.Message);
                }
            });

            var result = Parallel.For(0, max, i => logger.Debug(i.ToString()));

            Assert.IsTrue(result.IsCompleted);
            while(!logger.IsCompleted) {
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
            Assert.AreEqual(counter, max);
        }
    }
}
