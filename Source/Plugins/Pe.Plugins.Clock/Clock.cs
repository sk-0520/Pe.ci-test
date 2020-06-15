using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Clock.Addon;

namespace ContentTypeTextNet.Pe.Plugins.Clock
{
    public class Clock: PluginBase, IAddon
    {
        #region variable

        ClockAddonImpl _addon;

        #endregion

        public Clock(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new ClockAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        //protected override IPreferences CreatePreferences() => new FileFinderPreferences();

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion

    }
}
