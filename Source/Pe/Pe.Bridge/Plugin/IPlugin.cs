using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;

namespace ContentTypeTextNet.Pe.Bridge.Plugin
{
    /// <summary>
    /// プラグイン種別。
    /// </summary>
    public enum PluginKind
    {
        /// <summary>
        /// アドオン。
        /// </summary>
        Addon,
        /// <summary>
        /// テーマ。
        /// </summary>
        Theme,
    }

    /// <summary>
    /// プラグインの既定インターフェイス。
    /// <para><see cref="Addon.IAddon"/>か<see cref="Theme.ITheme"/>をさらに実装している必要あり。</para>
    /// <para>設定機能を有する場合はさらに<see cref="Preferences.IPreferences"/>を実装する必要あり。</para>
    /// </summary>
    public interface IPlugin
    {
        #region property

        /// <summary>
        /// プラグイン情報。
        /// <para>このプロパティ以下は<see cref="IsInitialized"/>の状態にかかわらず読み込み可能であること。</para>
        /// </summary>
        IPluginInformation PluginInformation { get; }

        /// <summary>
        /// 初期化処理が行われたか。
        /// <para><see cref="Initialize(IPluginInitializeContext)"/>が呼び出された後、プラグイン側で責任をもって真にすること。</para>
        /// </summary>
        bool IsInitialized { get; }

        #endregion

        #region function

        /// <summary>
        /// プラグインの初期化。
        /// <para>この段階ではあんまり小難しいことをしないこと。</para>
        /// </summary>
        /// <param name="pluginInitializeContext"></param>
        void Initialize(IPluginInitializeContext pluginInitializeContext);

        /// <summary>
        /// プラグイン終了。
        /// <para>可能な限りプラグイン開放可能な状態になること。</para>
        /// </summary>
        /// <param name="pluginUninitializeContext"></param>
        void Finalize(IPluginFinalizeContext pluginUninitializeContext);

        /// <summary>
        /// プラグイン機能を使用するための読み込み。
        /// </summary>
        /// <param name="pluginKind">対象機能ごとの読み込み指定。テーマだけ読み込んでアドオンはまだ、みたいな状態。</param>
        /// <param name="pluginLoadContext"></param>
        void Load(PluginKind pluginKind, IPluginLoadContext pluginLoadContext);
        /// <summary>
        /// プラグイン機能の使用を終了。
        /// </summary>
        /// <param name="pluginKind">対象機能。</param>
        /// <param name="pluginUnloadContext"></param>
        void Unload(PluginKind pluginKind, IPluginUnloadContext pluginUnloadContext);
        /// <summary>
        /// プラグイン機能は読み込まれているか。
        /// </summary>
        /// <param name="pluginKind"></param>
        /// <returns></returns>
        bool IsLoaded(PluginKind pluginKind);


        /// <summary>
        /// プラグインを示すアイコンを取得。
        /// <para>この処理は<see cref="IsInitialized"/>の状態にかかわらず実行可能であること。</para>
        /// </summary>
        /// <param name="iconScale"></param>
        /// <returns></returns>
        DependencyObject GetIcon(IImageLoader imageLoader, in IconScale iconScale);

        #endregion
    }
}
