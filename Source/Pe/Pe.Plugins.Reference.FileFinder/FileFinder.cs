using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Addon;
using ContentTypeTextNet.Pe.Plugins.Reference.FileFinder.Preferences;

namespace ContentTypeTextNet.Pe.Plugins.Reference.FileFinder
{
    public class FileFinder: PluginBase, IAddon, IPreferences
    {
        #region variable

        private FileFinderAddonImpl _addon;

        #endregion

        public FileFinder(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new FileFinderAddonImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        protected override IPreferences CreatePreferences() => new FileFinderPreferences(this);

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void FinalizeImpl(IPluginFinalizeContext pluginUninitializeContext)
        { }


        #endregion
    }
}
