using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal static class EnumResourceManagerFactory
    {
        #region function

        private static IReadOnlyList<EnumResource> GetKeys()
        {
            var baseName = typeof(Key).FullName + ".";

            return new[] {
                Key.None,
            }.Select(i => new EnumResource((int)i, baseName + i.ToString()))
            .ToList()
            ;
        }

        public static EnumResourceManager Create()
        {
            var enumResourceManager = new EnumResourceManager();

            enumResourceManager
                .Register<Bridge.Models.Data.AppDesktopToolbarPosition>()
                .Register<Bridge.Models.Data.IconBox>()
                .Register<Bridge.Models.Data.CommandItemKind>()
                .Register<Key>()
                //.Register<Key>(GetKeys())
            ;



            return enumResourceManager;
        }

        #endregion
    }
}
