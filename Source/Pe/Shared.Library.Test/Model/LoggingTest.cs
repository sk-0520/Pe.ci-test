using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
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
}
