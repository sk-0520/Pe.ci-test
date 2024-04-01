using System.Runtime.CompilerServices;
using ContentTypeTextNet.Pe.Standard.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace ContentTypeTextNet.Pe.Main.Test
{
    public class Test
    {
        #region property

        /// <summary>
        /// テスト用ロガー。
        /// </summary>
        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();

        /// <summary>
        /// テスト用DIコンテナ。
        /// </summary>
        public static IDiContainer DiContainer { get; set; } = null!;

        #endregion

        #region function

        [ModuleInitializer]
        public static void AssemblyInitialize()
        {
            var logger = LoggerFactory.CreateLogger(nameof(Test));
            logger.LogInformation("START Pe.Main.Test");

            var testDiContainer = new TestDiContainer();
            var diContainer = testDiContainer.CreateDiContainer(LoggerFactory);
            var testDatabase = new TestDatabase();
            testDatabase.Initialize(diContainer);

            DiContainer = diContainer;
        }

        #endregion
    }
}
