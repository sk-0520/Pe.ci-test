using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.PluginBase.Abstract
{
    internal abstract class ThemeBase: ITheme
    {
        public ThemeBase(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<ThemeKind> SupportedKinds { get; }

        #endregion

        #region function

        #endregion

        #region ITheme

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind) => SupportedKinds.Contains(themeKind);
        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public virtual IGeneralTheme BuildGeneralTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>
        public virtual ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public virtual ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public virtual INoteTheme BuildNoteTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public virtual ICommandTheme BuildCommandTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public virtual INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter) => throw new NotImplementedException();

        #endregion

        #region IPlugin

        public IPluginInformations PluginInformations => throw new NotSupportedException();

        public bool IsInitialized => throw new NotSupportedException();

        public void Initialize(IPluginInitializeContext pluginInitializeContext) => throw new NotSupportedException();

        public bool IsLoaded(PluginKind pluginKind) => throw new NotSupportedException();

        public void Load(PluginKind pluginKind, IPluginContext pluginContext) => throw new NotSupportedException();

        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext) => throw new NotSupportedException();

        public void Unload(PluginKind pluginKind, IPluginContext pluginContext) => throw new NotSupportedException();

        #endregion

    }
}
