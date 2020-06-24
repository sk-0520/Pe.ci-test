using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    internal abstract class AddonBase: ExtensionBase, IAddon
    {
        public AddonBase(IPluginConstructorContext pluginConstructorContext, IPlugin plugin)
            :base(pluginConstructorContext, plugin)
        { }

        #region property

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<AddonKind> SupportedKinds { get; }

        #endregion

        #region function

        protected internal abstract void Load(IPluginLoadContext pluginLoadContext);
        protected internal abstract void Unload(IPluginUnloadContext pluginUnloadContext);

        #endregion

        #region IAddon


        /// <inheritdoc cref="IAddon.IsSupported(AddonKind)"/>
        public bool IsSupported(AddonKind addonKind) => SupportedKinds.Contains(addonKind);

        /// <inheritdoc cref="IAddon.BuildCommandFinder(IAddonParameter)"/>
        public virtual ICommandFinder BuildCommandFinder(IAddonParameter parameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IAddon.BuildWidget(IAddonParameter)"/>
        public virtual IWidget BuildWidget(IAddonParameter parameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IAddon.BuildBackground(IAddonParameter)"/>
        public virtual IBackground BuildBackground(IAddonParameter parameter) => throw new NotImplementedException();

        #endregion
    }
}
