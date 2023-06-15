using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Html.Addon
{
    internal class HtmlAddonImpl: AddonBase
    {
        public HtmlAddonImpl(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region AddonBase

        protected override IReadOnlyCollection<AddonKind> SupportedKinds { get; } = new[] {
            AddonKind.Widget,
        };

        protected internal override void Load(IPluginLoadContext pluginLoadContext)
        { }

        protected internal override void Unload(IPluginUnloadContext pluginUnloadContext)
        { }

        public override IWidget BuildWidget(IAddonParameter parameter)
        {
            return new HtmlWidget(parameter, PluginInformation);
        }

        #endregion
    }
}
