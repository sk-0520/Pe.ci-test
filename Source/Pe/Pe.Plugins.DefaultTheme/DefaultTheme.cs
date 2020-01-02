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
        #endregion

        #region ITheme

        public PluginId PluginId => Id;

        public IPluginInformation IPluginInformation => throw new NotImplementedException();

        public bool IsInitialized { get; private set; }

        public void Initialize()
        {
            ResourceDictionary = new ResourceDictionary();
            var uri = new Uri("pack://application:,,,/Pe.Plugins.DefaultTheme;component/Views/Resources/ThemeResource.xaml", UriKind.Absolute);
            ResourceDictionary.Source = uri;

            Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
            IsInitialized = true;
        }
        public void Uninitialize()
        {
            if(ResourceDictionary != null) {
                Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
            }

            IsInitialized = false;
        }

        public IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return new GeneralTheme(parameter);
        }

        public ILauncherGroupTheme BuildLauncherGroupTheme(IThemeParameter parameter)
        {
            return new LauncherGroupTheme(parameter);
        }

        public ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return new LauncherToolbarTheme(parameter);
        }

        public INoteTheme BuildNoteTheme(IThemeParameter parameter)
        {
            return new NoteTheme(parameter);
        }

        public ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return new CommandTheme(parameter);
        }

        #endregion
    }
}
