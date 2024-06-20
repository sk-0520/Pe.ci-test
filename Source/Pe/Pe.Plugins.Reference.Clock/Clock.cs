using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Addon;
using ContentTypeTextNet.Pe.Plugins.Reference.Clock.Preferences;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Clock
{
    public class Clock: PluginBase, IAddon, IPreferences
    {
        #region variable

        private readonly ClockAddonImpl _addon;

        #endregion

        public Clock(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new ClockAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        protected override IPreferences CreatePreferences() => new ClockPreferences(this);

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void FinalizeImpl(IPluginFinalizeContext pluginFinalizeContext)
        { }


        #endregion
    }
}
