using System.Windows.Input;

namespace ContentTypeTextNet.Pe.Main.Models
{
    internal static class EnumResourceManagerFactory
    {
        #region function

        public static EnumResourceManager Create()
        {
            var enumResourceManager = new EnumResourceManager();

            enumResourceManager
                .Register<Bridge.Models.Data.AppDesktopToolbarPosition>()
                .Register<Bridge.Models.Data.NoteCaptionPosition>()
                .Register<Bridge.Models.Data.ShowMode>()
                .Register<Bridge.Models.Data.IconBox>()
                .Register<Bridge.Models.Data.CommandItemKind>()
                .Register<Key>()
                .Register<ModifierKeys>()
            //.Register<Key>(GetKeys())
            ;



            return enumResourceManager;
        }

        #endregion
    }
}
