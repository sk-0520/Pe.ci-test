using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.PluginBase.Abstract
{
    internal abstract class AddonBase: IAddon
    {
        public AddonBase(IPluginConstructorContext pluginConstructorContext)
        {
//            Logger = pluginConstructorContext.LoggerFactory.CreateLogger(GetType());
        }

        #region property

//        protected ILogger Logger { get; }

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<AddonKind> SupportedKinds { get; }

        #endregion

        #region IAddon


        /// <inheritdoc cref="IAddon.IsSupported(AddonKind)"/>
        public bool IsSupported(AddonKind addonKind) => SupportedKinds.Contains(addonKind);

        public virtual ICommandFinder BuildCommandFinder(IAddonParameter parameter) => throw new NotImplementedException();

        #endregion

        #region IPlugin

        public IPluginInformations PluginInformations => throw new NotSupportedException();

        public bool IsInitialized => throw new NotSupportedException();

        public void Initialize(IPluginInitializeContext pluginInitializeContext) => throw new NotSupportedException();

        public bool IsLoaded(PluginKind pluginKind) => throw new NotSupportedException();

        public void Load(PluginKind pluginKind, IPluginContext pluginContext) => throw new NotSupportedException();

        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext) => throw new NotSupportedException();

        public void Unload(PluginKind pluginKind, IPluginContext pluginContext) => throw new NotSupportedException();

        #endregion
    }
}
