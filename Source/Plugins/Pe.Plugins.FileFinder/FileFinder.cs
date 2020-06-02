using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.FileFinder.Addon;

namespace ContentTypeTextNet.Pe.Plugins.FileFinder
{
    public class FileFinder: PluginBase, IAddon
    {
        #region variable

        FileFinderAddonImpl _addon;

        #endregion

        public FileFinder(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._addon = new FileFinderAddonImpl(pluginConstructorContext);
        }

        #region PluginBase

        internal override AddonBase Addon => this._addon;

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion
    }
}
