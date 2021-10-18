using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Bridge.Plugin.Preferences;
using ContentTypeTextNet.Pe.Embedded.Abstract;

namespace TEMPLATE_Namespace
{
    public class TEMPLATE_PluginName: PluginBase// IAddon, ITheme, IPreferences
    {
        #region variable
        #endregion

        public TEMPLATE_PluginName(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            //
        }

        #region PluginBase

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }

        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion

    }
}
