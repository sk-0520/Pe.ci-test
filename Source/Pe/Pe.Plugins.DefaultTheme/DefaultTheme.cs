using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme
{
    public class DefaultTheme: ITheme
    {
        #region define

        public static readonly PluginInformations Informations = new PluginInformations(
            new PluginIdentifiers(new Guid("4524FC23-EBB9-4C79-A26B-8F472C05095E"), "default-theme"),
            new PluginVersions(Assembly.GetExecutingAssembly()!.GetName()!.Version!, new Version(0, 0, 0), new Version(0, 0, 0)),
            new PluginAuthors(new Author("sk"), PluginLicense.DoWhatTheF_ckYouWantToPublicLicense2),
            new PluginCategory(PluginCategories.Design)
        );

        #endregion

        #region variable

        #endregion

        public DefaultTheme(ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        ILogger Logger { get; }

        ResourceDictionary? ResourceDictionary { get; set; }

        bool IsThemeLoaded { get; set; }

        #endregion

        #region ITheme

        /// <inheritdoc cref="IPlugin.PluginInformations"/>
        public IPluginInformations PluginInformations => Informations;

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="IPlugin.Initialize(IPluginInitializeContext)"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext)
        {
            IsInitialized = true;
        }
        /// <inheritdoc cref="IPlugin.Uninitialize(IPluginUninitializeContext)"/>
        public void Uninitialize(IPluginUninitializeContext pluginUninitializeContext)
        {
            IsInitialized = false;
        }

        /// <inheritdoc cref="IPlugin.Load(PluginKind, IPluginContext)"/>
        public void Load(PluginKind pluginKind, IPluginContext pluginContext)
        {
            if(pluginKind != PluginKind.Theme) {
                throw new NotSupportedException();
            }
            if(!IsThemeLoaded) {
                ResourceDictionary = new ResourceDictionary();
                var uri = new Uri("pack://application:,,,/Pe.Plugins.DefaultTheme;component/Views/Resources/DefaultThemeResource.xaml", UriKind.Absolute);
                ResourceDictionary.Source = uri;

                Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);

                IsThemeLoaded = true;
            }
        }

        /// <inheritdoc cref="IPlugin.Unload(PluginKind)"/>
        public void Unload(PluginKind pluginKind, IPluginContext pluginContext)
        {
            if(IsThemeLoaded) {
                if(ResourceDictionary != null) {
                    Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
                }
                IsThemeLoaded = false;
            }
        }

        /// <inheritdoc cref="IPlugin.IsLoaded(PluginKind)"/>
        public bool IsLoaded(PluginKind pluginKind)
        {
            if(pluginKind == PluginKind.Theme) {
                return IsThemeLoaded;
            }

            return false;
        }

        /// <inheritdoc cref="ITheme.IsSupported(ThemeKind)"/>
        public bool IsSupported(ThemeKind themeKind) => true;

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return new DefaultGeneralTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return new DefaultLauncherToolbarTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return new DefaultNoteTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return new DefaultCommandTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return new DefaultNotifyLogTheme(parameter);
        }

        #endregion
    }
}
