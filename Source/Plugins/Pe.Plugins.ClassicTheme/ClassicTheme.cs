using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme
{
    public class ClassicTheme: PluginBase.PluginBase
    {
        public ClassicTheme(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        { }

        #region PluginBase

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }
        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }

        protected override void LoadAddonImpl(IPluginContext pluginContext)
        { }
        protected override void LoadThemeImpl(IPluginContext pluginContext)
        { }

        protected override void UnloadAddonImpl(IPluginContext pluginContext)
        { }
        protected override void UnloadThemeImpl(IPluginContext pluginContext)
        { }

        #endregion
    }
}
