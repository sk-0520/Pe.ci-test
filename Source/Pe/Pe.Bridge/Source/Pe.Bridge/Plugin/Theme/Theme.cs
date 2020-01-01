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

    //TODO: 構造体で一本化したい
    public class ColorPair<T> : IReadOnlyColorPair<T>
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

        ILauncherGroupTheme GetLauncherGroupTheme();
        ILauncherToolbarTheme GetLauncherToolbarTheme();
        INoteTheme GetNoteTheme();

        #endregion
    }
}
