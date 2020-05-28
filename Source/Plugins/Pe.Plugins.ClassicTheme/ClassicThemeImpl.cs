using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.PluginBase.Abstract;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.ClassicTheme
{
    internal class ClassicThemeImpl: ThemeBase
    {
        public ClassicThemeImpl(IPluginConstructorContext pluginConstructorContext)
            :base(pluginConstructorContext)
        { }

        protected override IReadOnlyCollection<ThemeKind> SupportedKinds => throw new NotImplementedException();

        #region ThemeBase

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public override IGeneralTheme BuildGeneralTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>
        public override ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public override ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public override INoteTheme BuildNoteTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public override ICommandTheme BuildCommandTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public override INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter) => throw new NotImplementedException();

        #endregion
    }
}
