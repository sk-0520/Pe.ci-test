using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;

namespace ContentTypeTextNet.Pe.PluginBase
{
    public abstract class PluginBase: IPlugin
    {
        #region property

        IPluginInformations? Informations { get; set; }

        #endregion

        #region function

        protected abstract void InitializeImpl(IPluginInitializeContext pluginInitializeContext);
        protected abstract void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext);

        protected virtual IPluginInformations CreateInformations()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IPlugin


        /// <inheritdoc cref="IPlugin.PluginInformations"/>
        public IPluginInformations PluginInformations => Informations ??= CreateInformations();

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="IPlugin"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext)
        {
            if(IsInitialized) {
                throw new InvalidOperationException(nameof(IsInitialized));
            }

            InitializeImpl(pluginInitializeContext);
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Uninitialize(IPluginUninitializeContext)"/>
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext)
        {
            UninitializeImpl(pluginUninitializeContext);
            // 例外で死んだ場合は再初期化を避けるため補正しない
            IsInitialized = true;
        }

        /// <inheritdoc cref="IPlugin.Load(PluginKind, IPluginContext)"/>
        public abstract void Load(PluginKind pluginKind, IPluginContext pluginContext);
        /// <inheritdoc cref="IPlugin.Unload(PluginKind, IPluginContext)"/>
        public abstract void Unload(PluginKind pluginKind, IPluginContext pluginContext);
        /// <inheritdoc cref="IPlugin.IsLoaded(PluginKind)"/>
        public abstract bool IsLoaded(PluginKind pluginKind);

        #endregion
    }
}
