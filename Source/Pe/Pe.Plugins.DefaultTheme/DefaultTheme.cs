using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme
{
    public class DefaultTheme : ITheme
    {
        #region variable

        #endregion

        #region ITheme

        public PluginId PluginId { get; } = new PluginId(new Guid("4524FC23-EBB9-4C79-A26B-8F472C05095E"), "default-theme");

        public IPluginInformation IPluginInformation => throw new NotImplementedException();

        public ILauncherGroupTheme GetLauncherGroupTheme()
        {
            throw new NotImplementedException();
        }

        public ILauncherToolbarTheme GetLauncherToolbarTheme()
        {
            throw new NotImplementedException();
        }

        public INoteTheme GetNoteTheme()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
