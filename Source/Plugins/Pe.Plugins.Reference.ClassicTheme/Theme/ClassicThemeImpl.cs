using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Embedded.Abstract;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Plugins.Reference.ClassicTheme.Theme
{
    internal class ClassicThemeImpl: ThemeBase
    {
        public ClassicThemeImpl(IPluginConstructorContext pluginConstructorContext, IPlugin plugin)
            : base(pluginConstructorContext, plugin)
        { }

        #region property

        ResourceDictionary? ResourceDictionary { get; set; }

        #endregion


        #region ThemeBase

        protected override IReadOnlyCollection<ThemeKind> SupportedKinds { get; } = new[] {
            ThemeKind.General,
            ThemeKind.LauncherToolbar,
            ThemeKind.Notify,
            ThemeKind.Command,
        };

        protected internal override void Load(IPluginLoadContext pluginLoadContext)
        {
            ResourceDictionary = new ResourceDictionary();
            var uri = new Uri("pack://application:,,,/Pe.Plugins.ClassicTheme;component/Views/Resources/ClassicThemeResource.xaml", UriKind.Absolute);
            ResourceDictionary.Source = uri;

            Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
        }

        protected internal override void Unload(IPluginUnloadContext pluginUnloadContext)
        {
            if(ResourceDictionary != null) {
                Application.Current.Resources.MergedDictionaries.Add(ResourceDictionary);
            }
        }

        /// <inheritdoc cref="ITheme.BuildGeneralTheme(IThemeParameter)"/>
        public override IGeneralTheme BuildGeneralTheme(IThemeParameter parameter)
        {
            return new ClassicGeneralTheme(parameter);
        }

        /// <inheritdoc cref="ITheme.BuildLauncherToolbarTheme(IThemeParameter)"/>
        public override ILauncherToolbarTheme BuildLauncherToolbarTheme(IThemeParameter parameter)
        {
            return new ClassicLauncherToolbarTheme(parameter);
        }
        /// <inheritdoc cref="ITheme.BuildNoteTheme(IThemeParameter)"/>
        public override INoteTheme BuildNoteTheme(IThemeParameter parameter) => throw new NotImplementedException();
        /// <inheritdoc cref="ITheme.BuildCommandTheme(IThemeParameter)"/>
        public override ICommandTheme BuildCommandTheme(IThemeParameter parameter)
        {
            return new ClassicCommandTheme(parameter);
        }
        /// <inheritdoc cref="ITheme.BuildNotifyLogTheme(IThemeParameter)"/>
        public override INotifyLogTheme BuildNotifyLogTheme(IThemeParameter parameter)
        {
            return new ClassicNotifyLogTheme(parameter);
        }

        #endregion
    }
}
