namespace ContentTypeTextNet.Pe.Bridge.Plugin.Addon
{
    /// <summary>
    /// アドオン種別。
    /// </summary>
    public enum AddonKind
    {
        /// <summary>
        /// ランチャーアイテムとして処理可能アドオン。
        /// <para>TODO: 現状の実装だとむずかすぃ。。。</para>
        /// </summary>
        LauncherItem,
        /// <summary>
        /// コマンド入力として処理可能アドオン。
        /// <para><see cref="LauncherItem"/>では登録内容がコマンドに表示される可能性があるが、こちらは完全に独立した何か。</para>
        /// </summary>
        CommandFinder,
        /// <summary>
        /// ウィジェットとして処理可能アドオン。
        /// <para>1 プラグインにつき、 1 ウィンドウを想定。</para>
        /// </summary>
        Widget,
        /// <summary>
        /// 後ろでなんかしてるやつ。
        /// </summary>
        Background,
    }

    /// <summary>
    /// アドオン。
    /// </summary>
    public interface IAddon: IPlugin
    {
        #region function

        /// <summary>
        /// 対象のアドオン種別がサポートされているか。
        /// <para>このプロパティ以下は<see cref="IPlugin.IsInitialized"/>の状態、<see cref="IPlugin.IsLoaded(PluginKind)"/>にかかわらず読み込み可能であること。</para>
        /// </summary>
        /// <param name="addonKind"></param>
        /// <returns></returns>
        bool IsSupported(AddonKind addonKind);

        /// <summary>
        /// ランチャーアイテムアドオンの生成。
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ILauncherItemExtension CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter parameter);

        /// <summary>
        /// コマンド型アドオンの生成。
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        ICommandFinder BuildCommandFinder(IAddonParameter parameter);

        /// <summary>
        /// ウィジェットアドオンの生成。
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IWidget BuildWidget(IAddonParameter parameter);

        /// <summary>
        /// バックグラウンドアドオンの生成。
        /// <para>キャッシュ・都度生成はプラグイン側で制御する。</para>
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        IBackground BuildBackground(IAddonParameter parameter);

        #endregion
    }
}
