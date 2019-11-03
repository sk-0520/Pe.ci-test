using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContentTypeTextNet.Pe.Main.Test
{
    [TestClass]
    public class Test
    {
        #region property

        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();

        #endregion

        #region function

        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
            var logger = LoggerFactory.CreateLogger(nameof(Test));
            logger.LogInformation("start test");
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            var logger = LoggerFactory.CreateLogger(nameof(Test));
            logger.LogInformation("end test");
        }
        #endregion
    }
}
