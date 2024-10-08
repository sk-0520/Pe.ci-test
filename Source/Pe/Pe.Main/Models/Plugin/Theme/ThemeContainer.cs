using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using ContentTypeTextNet.Pe.Library.Database;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <summary>
    /// テーマ一覧。。。
    /// </summary>
    /// <remarks>
    /// <para>一覧が必要かと問われるとなんも言えねぇ。</para>
    /// </remarks>
    public class ThemeContainer: PluginContainerBase
    {
        #region variable

        private DefaultTheme? _defaultTheme;

        #endregion
        public ThemeContainer(IDatabaseBarrierPack databaseBarrierPack, IDatabaseDelayWriterPack databaseDelayWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseDelayWriterPack = databaseDelayWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;

            ViewManager = viewManager;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            Policy = policy;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        /// <inheritdoc cref="ILogger"/>
        private ILogger Logger { get; }
        /// <inheritdoc cref="ILoggerFactory"/>
        private ILoggerFactory LoggerFactory { get; }
        private IDatabaseBarrierPack DatabaseBarrierPack { get; }
        private IDatabaseDelayWriterPack DatabaseDelayWriterPack { get; }
        private IDatabaseStatementLoader DatabaseStatementLoader { get; }
        private EnvironmentParameters EnvironmentParameters { get; }
        private IUserAgentManager UserAgentManager { get; }

        private IViewManager ViewManager { get; }
        private IPlatformTheme PlatformTheme { get; }
        private IImageLoader ImageLoader { get; }
        private IMediaConverter MediaConverter { get; }
        private IPolicy Policy { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// テーマ一覧。
        /// </summary>
        private ISet<ITheme> Themes { get; } = new HashSet<ITheme>();

        private DefaultTheme DefaultTheme => this._defaultTheme ??= (DefaultTheme)Themes.First(i => i.PluginInformation.PluginIdentifiers.PluginId == DefaultTheme.Information.PluginIdentifiers.PluginId);

        /// <summary>
        /// 現在使用中テーマ。
        /// </summary>
        public ITheme? CurrentTheme { get; private set; }

        public bool CurrentThemeIsDefaultTheme { get; private set; }

        #endregion

        #region function

        private ThemeParameter CreateParameter(IPlugin addon) => new ThemeParameter(addon.PluginInformation, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);

        public void Add(ITheme theme)
        {
            Themes.Add(theme);
        }

        public void SetCurrentTheme(PluginId themePluginId, PluginContextFactory pluginContextFactory)
        {
            var theme = Themes.FirstOrDefault(i => i.PluginInformation.PluginIdentifiers.PluginId == themePluginId);
            if(theme == null) {
                Logger.LogWarning("指定のテーマ不明のため標準テーマを使用: {0}", themePluginId);
                theme = DefaultTheme;
                CurrentThemeIsDefaultTheme = true;
            } else {
                CurrentThemeIsDefaultTheme = false;
            }

            var prev = CurrentTheme;
            CurrentTheme = theme;

            if(prev != null) {
                using(var writerPack = DatabaseBarrierPack.WaitWrite()) {
                    using var unloadContext = pluginContextFactory.CreateUnloadContext(CurrentTheme.PluginInformation, writerPack);
                    prev.Unload(PluginKind.Theme, unloadContext);
                }
            }
            using(var readerPack = DatabaseBarrierPack.WaitRead()) {
                using var loadContext = pluginContextFactory.CreateLoadContext(CurrentTheme.PluginInformation, readerPack);
                CurrentTheme.Load(PluginKind.Theme, loadContext);
            }
        }

        private IResultTheme GetTheme<IResultTheme, TBuildParameter>(ThemeKind kind, TBuildParameter parameter, Func<TBuildParameter, IResultTheme> buildCurrentTheme, Func<TBuildParameter, IResultTheme> buildDefaultTheme)
        {
            Debug.Assert(CurrentTheme != null);

            Func<TBuildParameter, IResultTheme>? build = null;

            if(!CurrentThemeIsDefaultTheme) {
                if(CurrentTheme.IsSupported(kind)) {
                    build = buildCurrentTheme;
                }
            }

            if(build != null) {
                try {
                    return DispatcherWrapper.Get(() => build(parameter));
                } catch(Exception ex) {
                    Logger.LogWarning(ex, "テーマ使用時にエラー発生のため標準テーマを使用");
                }
            }

            if(!CurrentThemeIsDefaultTheme) {
                if(!DefaultTheme.IsLoaded(PluginKind.Theme)) {
                    Logger.LogInformation("標準テーマ先生準備できておらず。");
                    var pluginContextFactory = new PluginContextFactory(DatabaseBarrierPack, DatabaseDelayWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager, LoggerFactory);
                    using(var readerPack = DatabaseBarrierPack.WaitRead()) {
                        using var loadContext = pluginContextFactory.CreateLoadContext(DefaultTheme.PluginInformation, readerPack);
                        DefaultTheme.Load(PluginKind.Theme, loadContext);
                    }
                }
            }

            return DispatcherWrapper.Get(() => buildDefaultTheme(parameter));
        }

        public IGeneralTheme GetGeneralTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.General, CreateParameter(CurrentTheme), CurrentTheme.BuildGeneralTheme, DefaultTheme.BuildGeneralTheme);
        }

        public ILauncherToolbarTheme GetLauncherToolbarTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.LauncherToolbar, CreateParameter(CurrentTheme), CurrentTheme.BuildLauncherToolbarTheme, DefaultTheme.BuildLauncherToolbarTheme);
        }

        public INoteTheme GetNoteTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Note, CreateParameter(CurrentTheme), CurrentTheme.BuildNoteTheme, DefaultTheme.BuildNoteTheme);
        }

        public ICommandTheme GetCommandTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Command, CreateParameter(CurrentTheme), CurrentTheme.BuildCommandTheme, DefaultTheme.BuildCommandTheme);
        }

        public INotifyLogTheme GetNotifyTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Notify, CreateParameter(CurrentTheme), CurrentTheme.BuildNotifyLogTheme, DefaultTheme.BuildNotifyLogTheme);
        }

        #endregion

        #region PluginContainerBase

        /// <inheritdoc cref="PluginContainerBase.Plugins"/>
        public override IEnumerable<IPlugin> Plugins => Themes;

        #endregion
    }
}
