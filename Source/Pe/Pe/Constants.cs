using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace ContentTypeTextNet.Pe.Main
{
    public static partial class Constants
    {
        #region property

        public static string ProjectName { get; } = "Pe2";

        public static string ApplicationRootDirectoryPath { get; } = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        public static string EtcDirectoryPath { get; } = Path.Combine(ApplicationRootDirectoryPath, "etc");
        public static string LoggingConfigFilePath { get; } = Path.Combine(EtcDirectoryPath, "nlog.config");

        #endregion
    }
}
