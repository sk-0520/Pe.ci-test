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

            enumResourceManager.Add<Data.LauncherItemKind> ();

            return enumResourceManager;
        }

        #endregion
    }
}
