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

        /// <summary>
        /// 前景。
        /// </summary>
        public T Foreground { get; }
        /// <summary>
        /// 背景。
        /// </summary>
        public T Background { get; }

        #endregion
    }

    /// <summary>
    /// <see cref="ColorPair{T}"/>ヘルパー。
    /// </summary>
    public static class ColorPair
    {
        #region function

        /// <summary>
        /// <see cref="ColorPair{T}"/>生成処理。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="foreground">前景。</param>
        /// <param name="background">背景。</param>
        /// <returns></returns>
        public static ColorPair<T> Create<T>(T foreground, T background)
        {
            return new ColorPair<T>(foreground, background);
        }

        #endregion
    }

    /// <summary>
    /// ビュー状態。
    /// </summary>
    public enum ViewState
    {
        /// <summary>
        /// アクティブ。
        /// </summary>
        Active,
        /// <summary>
        /// 非アクティブ。
        /// </summary>
        Inactive,
        /// <summary>
        /// 無効。
        /// </summary>
        Disable
    }

    /// <summary>
    /// プラグインテーマ。
    /// </summary>
    public interface ITheme: IPlugin
    {
        #region function

        /// <summary>
        /// 対象のテーマ種別がサポートされているか。
        /// </summary>
        /// <param name="themeKind"></param>
        /// <returns></returns>
        bool IsSupported(ThemeKind themeKind);


        /// <summary>
        /// 基本テーマ生成。
        /// </summary>
        /// <remarks>
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IGeneralTheme BuildGeneralTheme(IThemeParameter parameter);
        /// <summary>
        /// ランチャーツールバーテーマ生成。
        /// </summary>
        /// <remarks>
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter);
        /// <summary>
        /// ノートテーマ生成。
        /// </summary>
        /// <remarks>
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        INoteTheme BuildNoteTheme(IThemeParameter parameter);
        /// <summary>
        /// コマンド入力テーマ生成。
        /// </summary>
        /// <remarks>
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ICommandTheme BuildCommandTheme(IThemeParameter parameter);

        /// <summary>
        /// 通知ログテーマ生成。
        /// </summary>
        /// <remarks>
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </remarks>
        /// <param name="parameter"></param>
        /// <returns></returns>
        INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter);

        #endregion
    }
}
