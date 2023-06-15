using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon
{
    internal class ClockAddonImpl: AddonBase
    {
        public ClockAddonImpl(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region AddonBase

        protected override IReadOnlyCollection<AddonKind> SupportedKinds { get; } = new[] {
            AddonKind.LauncherItem,
            AddonKind.Widget,
        };

        protected internal override void Load(IPluginLoadContext pluginLoadContext)
        {
        }

        protected internal override void Unload(IPluginUnloadContext pluginUnloadContext)
        {
        }

        public override ILauncherItemExtension CreateLauncherItemExtension(ILauncherItemExtensionCreateParameter parameter)
        {
            return new ClockLauncherItem(parameter, PluginInformation, Plugin);
        }

        public override IWidget BuildWidget(IAddonParameter parameter)
        {
            return new ClockWidget(parameter, PluginInformation);
        }


        #endregion
    }
}
