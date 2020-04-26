using System;
using System.Collections.Generic;
using System.Text;

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
    /// </summary>
    public interface IPlugin
    {
        #region property

        PluginId PluginId { get; }

        IPluginInformations PluginInformations { get; }

        bool IsInitialized { get; }

        #endregion

        #region function

        /// <summary>
        /// プラグインの初期化。
        /// <para>この段階ではあんまり小難しいことをしないこと。</para>
        /// </summary>
        void Initialize(IPluginInitializeContext pluginContext);

        void Uninitialize();

        void Load(PluginKind pluginKind, IPluginContext pluginContext);
        void Unload(PluginKind pluginKind);
        bool IsLoaded(PluginKind pluginKind);

        #endregion
    }
}
