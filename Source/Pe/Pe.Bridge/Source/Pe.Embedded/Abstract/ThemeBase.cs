using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Embedded.Abstract
{
    internal abstract class ThemeBase: ITheme
    {
        public ThemeBase(IPluginConstructorContext pluginConstructorContext)
        {
            Logger = pluginConstructorContext.LoggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }

        /// <summary>
        /// サポートするテーマ機能を一括定義。
        /// </summary>
        protected abstract IReadOnlyCollection<ThemeKind> SupportedKinds { get; }

        #endregion

        #region function

        protected internal abstract void Load(IPluginLoadContext pluginLoadContext);
        protected internal abstract void Unload(IPluginUnloadContext pluginUnloadContext);

        #endregion

        #region ITheme

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind) => SupportedKinds.Contains(themeKind);
        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public virtual IGeneralTheme BuildGeneralTheme(IThemeParameter parameter) => throw new NotImplementedException();
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
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext) => throw new NotSupportedException();

        public bool IsLoaded(PluginKind pluginKind) => throw new NotSupportedException();

        public void Load(PluginKind pluginKind, IPluginLoadContext pluginLoadContext) => throw new NotSupportedException();

        public void Unload(PluginKind pluginKind, IPluginUnloadContext pluginUnloadContext) => throw new NotSupportedException();

        #endregion

    }

    internal abstract class ThemeDetailBase
    {
        protected ThemeDetailBase(IThemeParameter parameter)
        {
            PlatformTheme = parameter.PlatformTheme;
            DispatcherWrapper = parameter.DispatcherWrapper;
            Logger = parameter.LoggerFactory.CreateLogger(GetType());
        }

        #region property

        protected ILogger Logger { get; }
        protected IPlatformTheme PlatformTheme { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region function

        protected double GetHorizontal(Thickness thickness) => thickness.Left + thickness.Right;

        protected double GetVertical(Thickness thickness) => thickness.Top + thickness.Bottom;

        protected TValue GetResourceValue<TValue>(string className, string methodName)
        {
            var key = className + '.' + methodName;
            return (TValue)Application.Current.Resources[key];

        }

        #endregion
    }
}
