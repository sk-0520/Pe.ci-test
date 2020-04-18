using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme.Theme;

namespace ContentTypeTextNet.Pe.Plugins.DefaultTheme
{
    public class DefaultTheme : ITheme
    {
        #region define

        public static readonly PluginId Id = new PluginId(new Guid("4524FC23-EBB9-4C79-A26B-8F472C05095E"), "default-theme");

        #endregion

        #region variable

        #endregion

        #region property

        ResourceDictionary? ResourceDictionary { get; set; }

        bool IsThemeLoaded { get; set; }

        #endregion

        #region ITheme

        /// <inheritdoc cref="IPlugin.PluginId"/>
        public PluginId PluginId => Id;

        /// <inheritdoc cref="IPlugin.PluginInformations"/>
        public IPluginInformations PluginInformations => throw new NotImplementedException();

        /// <inheritdoc cref="IPlugin.IsInitialized"/>
        public bool IsInitialized { get; private set; }

        /// <inheritdoc cref="IPlugin.Initialize(IPluginInitializeContext)"/>
        public void Initialize(IPluginInitializeContext pluginInitializeContext)
        {
            IsInitialized = true;
        }
        /// <inheritdoc cref="IPlugin.Uninitialize"/>
        public void Uninitialize()
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
                var uri = new Uri("pack://application:,,,/Pe.Plugins.DefaultTheme;component/Views/Resources/ThemeResource.xaml", UriKind.Absolute);
                ResourceDictionary.Source = uri;

                Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);

                IsThemeLoaded = true;
            }
        }

        /// <inheritdoc cref="IPlugin.Unload(PluginKind)"/>
        public void Unload(PluginKind pluginKind)
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

        /// <inheritdoc cref="ITheme.IsSupport(ThemeKind)"/>
        public bool IsSupport(ThemeKind themeKind) => true;

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return new GeneralTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildLauncherGroupTheme(IThemeParameter)"/>
        public ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter)
        {
            return new LauncherGroupTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return new LauncherToolbarTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return new NoteTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return new CommandTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return new NotifyLogTheme(parameter);
        }

        #endregion
    }
}
