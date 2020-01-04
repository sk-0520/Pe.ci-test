using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{

    public readonly struct ColorPair<T>
    {
        public ColorPair(T foreground, T background)
        {
            Foreground = foreground;
            Background = background;
        }

        #region property

        public T Foreground { get; }
        public T Background { get; }

        #endregion
    }

    public static class ColorPair
    {
        #region function
        public static ColorPair<T> Create<T>(T foreground, T background)
        {
            return new ColorPair<T>(foreground, background);
        }

        #endregion
    }


    public enum ViewState
    {
        Active,
        Inactive,
        Disable
    }

    public interface ITheme : IPlugin
    {
        #region function

        IGeneralTheme BuildGeneralTheme(IThemeParameter parameter);
        ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter);
        ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter);
        INoteTheme BuildNoteTheme(IThemeParameter parameter);
        ICommandTheme BuildCommandTheme(IThemeParameter parameter);

        #endregion
    }
}
