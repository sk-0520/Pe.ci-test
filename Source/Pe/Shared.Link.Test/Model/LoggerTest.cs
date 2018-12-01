using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shared.Link.Test.Model
{
    [TestClass]
    public class LoggerTest
    {
        class TestLogger2: TestLogger
        {
            protected override void PutCore(LogKind kind, string message, string detail, Caller caller)
            {
                PutAction(kind, message, detail, caller);
                base.PutCore(kind, message, detail, caller);
            }

            public PutDelegate PutAction { get; set; }
        }

        [TestMethod]
        public void PutTest()
        {
            var logger = new TestLogger2();

            logger.PutAction = new PutDelegate((LogKind kind, string message, string detail, Caller caller) => {
                Assert.AreEqual(kind, LogKind.Debug);
                Assert.AreEqual(message, "message");
                Assert.AreEqual(caller.memberName, nameof(PutTest));
            });
            logger.Debug("message");

            logger.PutAction = new PutDelegate((LogKind kind, string message, string detail, Caller caller) => {
                Assert.AreEqual(kind, LogKind.Error);
                Assert.AreEqual(message, "message2");
                Assert.AreEqual(caller.memberName, nameof(PutTest));
            });
            logger.Error("message2");

        }
    }
}
