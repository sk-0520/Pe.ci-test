using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Interactivity;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// アドオン用コンテナ。
    /// </summary>
    public class AddonContainer
    {
        #region variable

        List<IAddon>? _launcherItemSupportAddons;
        List<IAddon>? _commandFinderSupportAddons;
        List<IAddon>? _widgetSupportAddons;
        List<IAddon>? _backgroundSupportAddons;


        #endregion
        public AddonContainer(IDatabaseBarrierPack databaseBarrierPack, IDatabaseLazyWriterPack databaseLazyWriterPack, IDatabaseStatementLoader databaseStatementLoader, EnvironmentParameters environmentParameters, IUserAgentManager userAgentManager, IPlatformTheme platformTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
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
        /// アドオン一覧。
        /// </summary>
        ISet<IAddon> Addons { get; } = new HashSet<IAddon>();

        List<IAddon> LauncherItemSupportAddons => this._launcherItemSupportAddons ??= GetSupportAddons(AddonKind.LauncherItem);
        List<IAddon> CommandFinderSupportAddons => this._commandFinderSupportAddons ??= GetSupportAddons(AddonKind.CommandFinder);
        List<IAddon> WidgetSupportAddons => this._widgetSupportAddons ??= GetSupportAddons(AddonKind.Widget);
        List<IAddon> BackgroundSupportAddons => this._backgroundSupportAddons ??= GetSupportAddons(AddonKind.Background);

        #endregion

        #region function

        public void Add(IAddon addon)
        {
            Addons.Add(addon);
        }

        private List<IAddon> GetSupportAddons(AddonKind kind)
        {
            var result = new List<IAddon>();

            var targets = Addons.Where(i => i.IsSupported(kind));

            foreach(var target in targets) {
                if(!target.IsLoaded(Bridge.Plugin.PluginKind.Addon)) {
                    var pluginContextFactory = new PluginContextFactory(DatabaseBarrierPack, DatabaseLazyWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager, LoggerFactory);
                    using(var readerPack = DatabaseBarrierPack.WaitRead()) {
                        var loadContext = pluginContextFactory.CreateLoadContex(target.PluginInformations, readerPack);
                        target.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }

                result.Add(target);
            }

            return result;
        }

        public CommandFinderAddonWrapper GetCommandFinder()
        {
            return new CommandFinderAddonWrapper(CommandFinderSupportAddons, DatabaseBarrierPack, DatabaseLazyWriterPack, DatabaseStatementLoader, EnvironmentParameters, UserAgentManager, PlatformTheme, DispatcherWrapper, LoggerFactory);
        }

        #endregion
    }
}
