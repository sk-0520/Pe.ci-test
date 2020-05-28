using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.PluginBase.Abstract;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme
{
    public class ClassicTheme: PluginBase.Abstract.PluginBase, ITheme
    {
        #region variable

        ClassicThemeImpl _theme;

        #endregion

        public ClassicTheme(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            this._theme = new ClassicThemeImpl(loggerFactory);
        }

        #region PluginBase

        internal override ThemeBase Theme => this._theme;

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
