using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Html.Addon;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Html
{
    public class Html: PluginBase, IAddon
    {
        #region variable

        private HtmlAddonImpl _addon;

        #endregion

        public Html(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new HtmlAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        //protected override IPreferences CreatePreferences() => new ClockPreferences(this);

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void FinalizeImpl(IPluginFinalizeContext pluginFinalizeContext)
        { }

        #endregion

    }
}
