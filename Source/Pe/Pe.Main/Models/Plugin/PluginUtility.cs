using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin
{
    public static class PluginUtility
    {
        #region function

        /// <summary>
        /// プラグインバージョンは Pe バージョンに制限されるか。
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public static bool IsUnlimitedVersion(Version version)
        {
            return version.Major == 0 && version.Minor == 0 && version.Build == 0;
        }

        #endregion
    }
}
