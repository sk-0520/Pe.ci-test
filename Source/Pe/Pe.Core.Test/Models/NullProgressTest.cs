using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
            var mockLog = MockLog.Create();

            mockLog.VerifyFactoryNever();
            mockLog.VerifyLogNever();

            var test = new NullDoubleProgress(mockLog.Factory.Object);
            mockLog.VerifyFactory(Times.Once());
            mockLog.VerifyLogNever();

            test.Report(0.5);
            mockLog.VerifyMessage(LogLevel.Trace, "0.5", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Debug, "0.5", Times.Exactly(1));
            mockLog.VerifyMessage(LogLevel.Information, "0.5", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Warning, "0.5", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Error, "0.5", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Critical, "0.5", Times.Exactly(0));

            test.Report(0.75);
            mockLog.VerifyMessage(LogLevel.Trace, "0.75", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Debug, "0.75", Times.Exactly(1));
            mockLog.VerifyMessage(LogLevel.Information, "0.75", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Warning, "0.75", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Error, "0.75", Times.Exactly(0));
            mockLog.VerifyMessage(LogLevel.Critical, "0.75", Times.Exactly(0));

            test.Report(0.75);
            mockLog.VerifyMessage(LogLevel.Debug, "0.75", Times.Exactly(2));
            mockLog.VerifyMessageStartsWith(LogLevel.Debug, "0.7", Times.Exactly(2));
            mockLog.VerifyMessageEndsWith(LogLevel.Debug, ".75", Times.Exactly(2));
            mockLog.VerifyMessageContains(LogLevel.Debug, "7", Times.Exactly(2));
            mockLog.VerifyMessageRegex(LogLevel.Debug, new Regex("\\d\\.\\d{2}"), Times.Exactly(2));
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
