using System;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    /// <summary>
    /// 拡張機能としての最下層クラス。
    /// <para>プラグイン実装者がこのクラスに対して手を入れることはない。</para>
    /// </summary>
    internal class ExtensionBase: IPlugin
    {
        protected ExtensionBase(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
        {
            Logger = pluginConstructorContext.LoggerFactory.CreateLogger(GetType());
            Plugin = plugin;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        protected ILogger Logger { get; }

        /// <inheritdoc cref="PluginBase"/>
        protected PluginBase Plugin { get; }

        #endregion

        #region IPlugin

        /// <inheritdoc cref="IPluginInformation"/>
        public IPluginInformation PluginInformation => Plugin.PluginInformation;

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized => Plugin.IsInitialized;

        /// <summary>
        /// プラグインのアイコンを取得。
        /// </summary>
        /// <param name="imageLoader">イメージ取得処理。</param>
        /// <param name="iconScale">スケール。</param>
        /// <returns>アイコン。</returns>
        public DependencyObject GetIcon(IImageLoader imageLoader, in IconScale iconScale) => throw new NotSupportedException();

        /// <inheritdoc cref="IPlugin.Initialize(IPluginInitializeContext)"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext) => throw new NotSupportedException();
        /// <inheritdoc cref="IPlugin.Finalize(IPluginFinalizeContext)"/>
        public void Finalize(IPluginFinalizeContext pluginFinalizeContext) => throw new NotSupportedException();

        /// <inheritdoc cref="IPlugin.IsLoaded(PluginKind)"/>
        public bool IsLoaded(PluginKind pluginKind) => throw new NotSupportedException();

        /// <inheritdoc cref="IPlugin.Load(PluginKind, IPluginLoadContext)"/>
        public void Load(PluginKind pluginKind, IPluginLoadContext pluginLoadContext) => throw new NotSupportedException();
        /// <inheritdoc cref="IPlugin.Unload(PluginKind, IPluginUnloadContext)"/>
        public void Unload(PluginKind pluginKind, IPluginUnloadContext pluginUnloadContext) => throw new NotSupportedException();

        #endregion

    }
}
