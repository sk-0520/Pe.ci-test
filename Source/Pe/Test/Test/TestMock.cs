using System;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace ContentTypeTextNet.Pe.Test
{
    public class MockLog
    {
        public MockLog(Mock<ILoggerFactory> loggerFactory, Mock<ILogger> logger)
        {
            LoggerFactory = loggerFactory;
            Logger = logger;
        }

        #region property

        public Mock<ILoggerFactory> LoggerFactory { get; }
        public Mock<ILogger> Logger { get; }

        #endregion

        #region function

        public void VerifyLoggerFactoryNone()
        {
            LoggerFactory.Verify(a => a.CreateLogger(It.IsAny<string>()), Times.Exactly(0));
        }

        public void VerifyLoggerFactory(Times times)
        {
            LoggerFactory.Verify(a => a.CreateLogger(It.IsAny<string>()), times);
        }

        public void VerifyLoggerNone(LogLevel logLevel = LogLevel.None)
        {
            Logger.Verify(
                a => a.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Exactly(0)
            );
        }

        public void VerifyLoggerMessage(LogLevel logLevel, string message, Times times)
        {
            //var lv = logLevel == LogLevel.None ? It.IsAny<LogLevel>() : It.Is<LogLevel>(a => a == logLevel);
            Logger.Verify(
                a => a.Log(
                    //logLevel == LogLevel.None ? It.IsAny<LogLevel>() : It.Is<LogLevel>(a => a == logLevel),
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((a, _) => a.ToString() == message),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                times
            );
        }

        public void VerifyLoggerMessage(string message, Times times)
        {
            VerifyLoggerMessage(LogLevel.None, message, times);
        }

        #endregion
    }

    public static class TestMock
    {
        public static MockLog CreateLog()
        {
            var mockLogger = new Mock<ILogger>();

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory
                .Setup(a => a.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockLogger.Object)
            ;

            return new MockLog(mockLoggerFactory, mockLogger);
        }
    }
}
