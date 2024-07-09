using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Moq;

namespace ContentTypeTextNet.Pe.Test
{
    /// <summary>
    /// ログ系のモック。
    /// </summary>
    public class MockLog
    {
        public MockLog(Mock<ILoggerFactory> loggerFactory, Mock<ILogger> logger)
        {
            Factory = loggerFactory;
            Logger = logger;
        }

        #region property

        public Mock<ILoggerFactory> Factory { get; }
        public Mock<ILogger> Logger { get; }

        #endregion

        #region function

        /// <summary>
        /// 生成。
        /// </summary>
        /// <remarks>原則これを使用する。</remarks>
        /// <returns></returns>
        public static MockLog Create()
        {
            var mockLogger = new Mock<ILogger>();

            var mockLoggerFactory = new Mock<ILoggerFactory>();
            mockLoggerFactory
                .Setup(a => a.CreateLogger(It.IsAny<string>()))
                .Returns(() => mockLogger.Object)
            ;

            return new MockLog(mockLoggerFactory, mockLogger);
        }

        /// <summary>
        /// <see cref="Factory"/> は呼び出されなかった。
        /// </summary>
        public void VerifyFactoryNever()
        {
            Factory.Verify(a => a.CreateLogger(It.IsAny<string>()), Times.Never());
        }

        /// <summary>
        /// <see cref="Factory"/> が呼び出された。
        /// </summary>
        /// <param name="times"></param>
        public void VerifyFactory(Times times)
        {
            Factory.Verify(a => a.CreateLogger(It.IsAny<string>()), times);
        }

        /// <summary>
        /// <see cref="Logger"/>の<see cref="ILogger.Log"/> は呼び出されなかった。
        /// </summary>
        /// <param name="logLevel">対象ログレベル。<see cref="LogLevel.None"/>は全てを指す。</param>
        public void VerifyLogNever(LogLevel logLevel = LogLevel.None)
        {
            Logger.Verify(
                a => a.Log(
                    It.Is<LogLevel>(a => logLevel == LogLevel.None ? true : a == logLevel),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Never()
            );
        }

        /// <summary>
        /// <see cref="Logger"/>の<see cref="ILogger.Log"/> のメッセージを検証。
        /// </summary>
        /// <param name="logLevel">対象ログレベル。<see cref="LogLevel.None"/>は全てを指す。</param>
        /// <param name="predicate">判定処理。</param>
        /// <param name="times"></param>
        public void VerifyMessagePredicate(LogLevel logLevel, Predicate<string> predicate, Times times)
        {
            Logger.Verify(
                a => a.Log(
                    It.Is<LogLevel>(a => logLevel == LogLevel.None ? true : a == logLevel),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((a, _) => predicate(a.ToString()!)),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()
                ),
                times
            );
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">完全一致となるメッセージ。</param>
        public void VerifyMessage(LogLevel logLevel, string message, Times times)
        {
            VerifyMessagePredicate(logLevel, a => message == a, times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">前方一致となるメッセージ。</param>
        public void VerifyMessageStartsWith(LogLevel logLevel, string message, Times times)
        {
            VerifyMessagePredicate(logLevel, a => a.StartsWith(message), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">後方一致となるメッセージ。</param>
        public void VerifyMessageEndsWith(LogLevel logLevel, string message, Times times)
        {
            VerifyMessagePredicate(logLevel, a => a.EndsWith(message), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="message">部分一致となるメッセージ。</param>
        public void VerifyMessageContains(LogLevel logLevel, string message, Times times)
        {
            VerifyMessagePredicate(logLevel, a => a.Contains(message, StringComparison.Ordinal), times);
        }

        /// <inheritdoc cref="VerifyMessagePredicate" />
        /// <param name="regex">メッセージに対する一致パターン。</param>
        public void VerifyMessageRegex(LogLevel logLevel, Regex regex, Times times)
        {
            VerifyMessagePredicate(logLevel, a => regex.IsMatch(a), times);
        }

        #endregion
    }
}
