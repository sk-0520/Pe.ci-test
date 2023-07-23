using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.Eyes.Addon;

namespace ContentTypeTextNet.Pe.Plugins.Reference.Eyes
{
    public class Eyes: PluginBase, IAddon
    {
        #region variable

        private EyesAddonImpl _addon;

        #endregion

        public Eyes(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new EyesAddonImpl(pluginConstructorContext, this);
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
