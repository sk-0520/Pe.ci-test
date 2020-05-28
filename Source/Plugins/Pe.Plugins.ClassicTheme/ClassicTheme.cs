using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.ClassicTheme.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme
{
    public class ClassicTheme: PluginBase, ITheme
    {
        #region variable

        ClassicThemeImpl _theme;

        #endregion

        public ClassicTheme(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._theme = new ClassicThemeImpl(pluginConstructorContext);
        }

        #region PluginBase

        internal override ThemeBase Theme => this._theme;

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }
        protected override void UninitializeImpl(IPluginUninitializeContext pluginUninitializeContext)
        { }


        #endregion
    }
}
