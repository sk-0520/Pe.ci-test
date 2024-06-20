using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme
{
    public class ClassicTheme: PluginBase, ITheme
    {
        #region variable

        private readonly ClassicThemeImpl _theme;

        #endregion

        public ClassicTheme(IPluginConstructorContext pluginConstructorContext)
            : base(pluginConstructorContext)
        {
            this._theme = new ClassicThemeImpl(pluginConstructorContext, this);
        }

        #region PluginBase

        internal override ThemeBase Theme => this._theme;

        protected override void InitializeImpl(IPluginInitializeContext pluginInitializeContext)
        { }
        protected override void FinalizeImpl(IPluginFinalizeContext pluginFinalizeContext)
        { }


        #endregion
    }
}
