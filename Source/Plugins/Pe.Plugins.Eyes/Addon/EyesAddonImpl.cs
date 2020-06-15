using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Eyes.Addon
{
    internal class EyesAddonImpl: AddonBase
    {
        public EyesAddonImpl(IPluginConstructorContext pluginConstructorContext, IPlugin plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region proeprty

        EyesBackground? EyesBackground { get; set; }

        #endregion

        #region AddonBase

        protected override IReadOnlyCollection<AddonKind> SupportedKinds { get; } = new[] {
            AddonKind.Widget,
            AddonKind.Background,
        };

        protected internal override void Load(IPluginLoadContext pluginLoadContext)
        {
        }

        protected internal override void Unload(IPluginUnloadContext pluginUnloadContext)
        {
        }

        public override IWidget BuildWidget(IAddonParameter parameter)
        {
            return new EyesWidget(parameter, PluginInformations);
        }

        public override IBackground BuildBackground(IAddonParameter parameter)
        {
            return EyesBackground ??= new EyesBackground(parameter, PluginInformations);
        }


        #endregion
    }
}
