using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    public interface IReadOnlyColorPair<out T>
    {
        #region property

        T Foreground { get; }
        T Background { get; }

        #endregion
    }

    public interface ITheme : IPlugin
    {
        #region function

        IFontTheme GetFontTheme();
        ILauncherGroupTheme GetLauncherGroupTheme();
        ILauncherToolbarTheme GetLauncherToolbarTheme();
        INoteTheme GetNoteTheme();

        #endregion
    }
}
