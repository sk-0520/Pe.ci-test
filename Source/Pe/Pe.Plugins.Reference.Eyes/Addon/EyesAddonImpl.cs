using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Eyes.Addon
{
    internal class EyesAddonImpl: AddonBase
    {
        public EyesAddonImpl(IPluginConstructorContext pluginConstructorContext, PluginBase plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region proeprty

        private EyesWidget? EyesWidget { get; set; }
        private EyesBackground? EyesBackground { get; set; }

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
            EyesWidget = new EyesWidget(parameter, PluginInformation);
            if(EyesBackground != null) {
                EyesWidget.Attach(EyesBackground);
            }
            return EyesWidget;
        }

        public override IBackground BuildBackground(IAddonParameter parameter)
        {
            EyesBackground ??= new EyesBackground(parameter, PluginInformation);
            if(EyesWidget != null) {
                EyesWidget.Attach(EyesBackground);
            }
            return EyesBackground;
        }


        #endregion
    }
}
