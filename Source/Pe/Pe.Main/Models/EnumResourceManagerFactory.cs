using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal static class EnumResourceManagerFactory
    {
        #region function

        public static EnumResourceManager Create()
        {
            var enumResourceManager = new EnumResourceManager();

            var x = enumResourceManager.GetString(Data.LauncherItemKind.File);
            var y = enumResourceManager.GetString(Data.LauncherItemKind.Unknown);

            return enumResourceManager;
        }

        #endregion
    }
}
