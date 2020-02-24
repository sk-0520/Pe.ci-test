using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models
{
    public static class ProcessArchitecture
    {
        #region property

        public static string ApplicationArchitecture => GetProcessName(Environment.Is64BitProcess);
        public static string PlatformArchitecture => GetProcessName(Environment.Is64BitOperatingSystem);

        #endregion

        #region function

        private static string GetProcessName(bool is64BitProcess)
        {
            return is64BitProcess ? "x64" : "x86";
        }

        #endregion
    }
}
