using System;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    /// <summary>
    /// アドオン基盤。
    /// </summary>
    internal abstract class AddonBase: ExtensionBase, IAddon
    {
        protected AddonBase(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region property

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<AddonKind> SupportedKinds { get; }

        #endregion

        #region function

        /// <summary>
        /// 読み込み 基底処理。
        /// </summary>
        /// <param name="pluginLoadContext"></param>
        protected internal abstract void Load(IPluginLoadContext pluginLoadContext);
        /// <summary>
        /// 解放 基底処理。
        /// </summary>
        /// <param name="pluginUnloadContext"></param>
        protected internal abstract void Unload(IPluginUnloadContext pluginUnloadContext);

        #endregion

        #region IAddon

        /// <inheritdoc cref="IAddon.IsSupported(AddonKind)"/>
        public bool IsSupported(AddonKind addonKind) => SupportedKinds.Contains(addonKind);

        /// <inheritdoc cref="IAddon.CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter)"/>
        public virtual ILauncherItemExtension CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter parameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IAddon.BuildCommandFinder(IAddonParameter)"/>
        public virtual ICommandFinder BuildCommandFinder(IAddonParameter parameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IAddon.BuildWidget(IAddonParameter)"/>
        public virtual IWidget BuildWidget(IAddonParameter parameter) => throw new NotImplementedException();

        /// <inheritdoc cref="IAddon.BuildBackground(IAddonParameter)"/>
        public virtual IBackground BuildBackground(IAddonParameter parameter) => throw new NotImplementedException();

        #endregion
    }
}
