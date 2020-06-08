using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Plugins.DefaultTheme;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Theme
{
    /// <summary>
    /// テーマ一覧。。。
    /// <para>一覧が必要かと問われるとなんも言えねぇ。</para>
    /// </summary>
    public class ThemeContainer
    {
        #region variable

        DefaultTheme? _defaultTheme;

        #endregion
        public ThemeContainer(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            DatabaseBarrierPack = databaseBarrierPack;
            DatabaseLazyWriterPack = databaseLazyWriterPack;
            DatabaseStatementLoader = databaseStatementLoader;
            EnvironmentParameters = environmentParameters;
            UserAgentManager = userAgentManager;

            PlatformTheme = platformTheme;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        IDatabaseBarrierPack DatabaseBarrierPack { get; }
        IDatabaseLazyWriterPack DatabaseLazyWriterPack { get; }
        IDatabaseStatementLoader DatabaseStatementLoader { get; }
        EnvironmentParameters EnvironmentParameters { get; }
        IUserAgentManager UserAgentManager { get; }

        IPlatformTheme PlatformTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// テーマ一覧。
        /// </summary>
        ISet<ITheme> Themes { get; } = new HashSet<ITheme>();

        DefaultTheme DefaultTheme => this._defaultTheme ??= (DefaultTheme)Themes.First(i => i.PluginInformations.PluginIdentifiers.PluginId == DefaultTheme.Informations.PluginIdentifiers.PluginId);

        /// <summary>
        /// 現在使用中テーマ。
        /// </summary>
        public ITheme? CurrentTheme { get; private set; }

        public bool CurrentThemeIsDefaultTheme { get; private set; }

        #endregion

        #region function

        private ThemeParameter CreateParameter() => new ThemeParameter(PlatformTheme, DispatcherWrapper, LoggerFactory);

        public void Add(ITheme theme)
        {
            Themes.Add(theme);
        }

        public void SetCurrentTheme(Guid themePluginId, PluginContextFactory pluginContextFactory)
        {
            var theme = Themes.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == themePluginId);
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
                    prev.Unload(PluginKind.Theme, pluginContextFactory.CreateContext(CurrentTheme.PluginInformations.PluginIdentifiers, writerPack, false));
                }
            }
            using(var readerPack = DatabaseBarrierPack.WaitWrite()) {
                var pluginContext = pluginContextFactory.CreateContext(CurrentTheme.PluginInformations.PluginIdentifiers, readerPack, true);
                CurrentTheme.Load(PluginKind.Theme, pluginContext);
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
                    var pluginContextFactory = new PluginContextFactory(DatabaseLazyWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager);
                    using(var readerPack = DatabaseBarrierPack.WaitRead()) {
                        DefaultTheme.Load(PluginKind.Theme, pluginContextFactory.CreateContext(DefaultTheme.PluginInformations.PluginIdentifiers, readerPack, true));
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
            return GetTheme(ThemeKind.General, CreateParameter(), CurrentTheme.BuildGeneralTheme, DefaultTheme.BuildGeneralTheme);
        }

        public ILauncherToolbarTheme GetLauncherToolbarTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.LauncherToolbar, CreateParameter(), CurrentTheme.BuildLauncherToolbarTheme, DefaultTheme.BuildLauncherToolbarTheme);
        }

        public INoteTheme GetNoteTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Note, CreateParameter(), CurrentTheme.BuildNoteTheme, DefaultTheme.BuildNoteTheme);
        }

        public ICommandTheme GetCommandTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Command, CreateParameter(), CurrentTheme.BuildCommandTheme, DefaultTheme.BuildCommandTheme);
        }

        public INotifyLogTheme GetNotifyTheme()
        {
            if(CurrentTheme == null) {
                throw new InvalidOperationException();
            }
            return GetTheme(ThemeKind.Notify, CreateParameter(), CurrentTheme.BuildNotifyLogTheme, DefaultTheme.BuildNotifyLogTheme);
        }

        #endregion
    }
}
