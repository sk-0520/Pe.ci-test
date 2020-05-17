using System;
using System.Collections.Generic;
using System.Text;

namespace ContentTypeTextNet.Pe.Bridge.Plugin.Theme
{
    /// <summary>
    /// テーマ種別。
    /// </summary>
    public enum ThemeKind
    {
        /// <summary>
        /// 基本。
        /// </summary>
        General,
        /// <summary>
        /// ランチャーグループ。
        /// </summary>
        LauncherGroup,
        /// <summary>
        /// ランチャーツールバー。
        /// </summary>
        LauncherToolbar,
        /// <summary>
        /// ノート。
        /// </summary>
        Note,
        /// <summary>
        /// コマンド。
        /// </summary>
        Command,
        /// <summary>
        /// 通知UI。
        /// </summary>
        Notify,
    }

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

        /// <summary>
        /// 対象のテーマ種別がサポートされているか。
        /// </summary>
        /// <param name="themeKind"></param>
        /// <returns></returns>
        bool IsSupported(ThemeKind themeKind);


        IGeneralTheme BuildGeneralTheme(IThemeParameter parameter);
        ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter);
        ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter);
        INoteTheme BuildNoteTheme(IThemeParameter parameter);
        ICommandTheme BuildCommandTheme(IThemeParameter parameter);

        INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter);

        #endregion
    }
}
