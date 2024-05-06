using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace ContentTypeTextNet.Pe.Core.Test.Models
{
    public class NullDoubleProgressTest
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

            var test = new NullDoubleProgress(mockLoggerFactory.Object);
            mockLoggerFactory.Verify(a => a.CreateLogger(It.IsAny<string>()), Times.Exactly(1));
            mockLogger.Verify(a => a.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(0));

            test.Report(0.5);
            mockLogger.Verify(a => a.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.Is<It.IsAnyType>((a, _) => a.ToString() == "0.5"), It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Exactly(1));
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
