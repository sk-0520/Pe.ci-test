using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Library.Shared.Embedded.Test.Model
{
    [TestClass]
    public class LoggerTest
    {
        class TestLogger2: TestLogger
        {
            protected override void PutCore(LogItem logItem)
            {
                PutAction(logItem);
                base.PutCore(logItem);
            }

            public Action<LogItem> PutAction { get; set; }
        }

        [TestMethod]
        public void PutTest()
        {
            var logger = new TestLogger2();

            logger.PutAction = logItem => {
                Assert.AreEqual(logItem.Kind, LogKind.Debug);
                Assert.AreEqual(logItem.Message, "message");
                Assert.AreEqual(logItem.Caller.MemberName, nameof(PutTest));
            };
            logger.Debug("message");

            logger.PutAction = logItem => {
                Assert.AreEqual(logItem.Kind, LogKind.Error);
                Assert.AreEqual(logItem.Message, "message2");
                Assert.AreEqual(logItem.Caller.MemberName, nameof(PutTest));
            };
            logger.Error("message2");

        }
    }
}
