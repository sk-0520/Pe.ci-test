using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <summary>
    /// テーマ一覧。。。
    /// <para>一覧が必要かと問われるとなんも言えねぇ。</para>
    /// </summary>
    public class ThemeContainer
    {
        public ThemeContainer(IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// テーマ一覧。
        /// </summary>
        ISet<ITheme> Themes { get; } = new HashSet<ITheme>();

        /// <summary>
        /// 現在使用中テーマ。
        /// </summary>
        public ITheme? CurrentTheme { get; private set; }

        #endregion

        #region function

        private ThemeParameter CreateParameter() => new ThemeParameter(PlatformTheme, DispatcherWrapper, LoggerFactory);

        public void Add(ITheme theme)
        {
            Themes.Add(theme);
        }

        public void SetCurrentTheme(IPluginIdentifiers pluginIdentifiers, PluginContextFactory pluginContextFactory)
        {
            var theme = Themes.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginIdentifiers.PluginId);

            var prev = CurrentTheme;
            CurrentTheme = theme;

            if(prev != null) {
                prev.Unload(PluginKind.Theme);
            }
            var pluginContext = pluginContextFactory.CreateContext(CurrentTheme.PluginInformations.PluginIdentifiers);
            CurrentTheme.Load(PluginKind.Theme, pluginContext);
        }

        public IGeneralTheme GetGeneralTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildGeneralTheme(CreateParameter()));
        }

        public ILauncherGroupTheme GetLauncherGroupTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildLauncherGroupTheme(CreateParameter()));
        }

        public ILauncherToolbarTheme GetLauncherToolbarTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildLauncherToolbarTheme(CreateParameter()));
        }

        public INoteTheme GetNoteTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildNoteTheme(CreateParameter()));
        }

        public ICommandTheme GetCommandTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildCommandTheme(CreateParameter()));
        }

        public INotifyLogTheme GetNotifyTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return DispatcherWrapper.Get(() => CurrentTheme.BuildNotifyLogTheme(CreateParameter()));
        }

        #endregion
    }
}
