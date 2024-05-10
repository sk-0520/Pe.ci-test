using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Test;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

// こいつの評価基準はモックをありゃこりゃするやつなんですよ

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class NullDoubleProgressTest
    {
        #region function

        [Fact]
        public void ReportTest()
        {
            var mockLog = TestMock.CreateLog();

            mockLog.VerifyLoggerFactoryNone();
            mockLog.VerifyLoggerNone();

            var test = new NullDoubleProgress(mockLog.LoggerFactory.Object);
            mockLog.VerifyLoggerFactory(Times.Once());
            mockLog.VerifyLoggerNone();

            test.Report(0.5);
            mockLog.VerifyLoggerMessage("0.5", Times.Exactly(1));
            //mockLog.VerifyLoggerMessage(LogLevel.Trace, "0.5", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Debug, "0.5", Times.Exactly(1));
            //mockLog.VerifyLoggerMessage(LogLevel.Information, "0.5", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Warning, "0.5", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Error, "0.5", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Critical, "0.5", Times.Exactly(0));

            test.Report(0.75);
            mockLog.VerifyLoggerMessage("0.75", Times.Exactly(1));
            //mockLog.VerifyLoggerMessage(LogLevel.Trace, "0.75", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Debug, "0.75", Times.Exactly(2));
            //mockLog.VerifyLoggerMessage(LogLevel.Information, "0.75", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Warning, "0.75", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Error, "0.75", Times.Exactly(0));
            //mockLog.VerifyLoggerMessage(LogLevel.Critical, "0.75", Times.Exactly(0));

            test.Report(0.75);
            mockLog.VerifyLoggerMessage("0.75", Times.Exactly(2));
        }

        #endregion
    }

    public class NullStringProgressTest
    {
        #region function

        [Fact]
        public void ReportTest()
        {
            var mockLogger = new Mock<ILogger>();

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory
                .Setup(a => a.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockLogger.Object)
            ;

            mockLoggerFactory.Verify(a => a.CreateLogger(It.IsAny<string>()), Times.Exactly(0));
            mockLogger.Verify(a => a.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(0));

            var test = new NullStringProgress(mockLoggerFactory.Object);
            mockLoggerFactory.Verify(a => a.CreateLogger(It.IsAny<string>()), Times.Exactly(1));
            mockLogger.Verify(a => a.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(0));

            test.Report("abc");
            mockLogger.Verify(a => a.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.Is<It.IsAnyType>((a, _) => a.ToString() == "abc"), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(1));
        }

        #endregion
    }

}
