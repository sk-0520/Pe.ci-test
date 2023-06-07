using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    /// <summary>
    /// アドオン用コンテナ。
    /// </summary>
    internal class AddonContainer: PluginContainerBase
    {
        #region variable

        private List<IAddon>? _launcherItemSupportAddons;
        private List<IAddon>? _commandFinderSupportAddons;
        private List<IAddon>? _widgetSupportAddons;
        private List<IAddon>? _backgroundSupportAddons;

        #endregion

        public AddonContainer(PluginContextFactory pluginContextFactory, LauncherItemAddonContextFactory launcherItemAddonContextFactory, BackgroundAddonContextFactory backgroundAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IViewManager viewManager, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IPolicy policy, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginContextFactory = pluginContextFactory;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
            BackgroundAddonContextFactory = backgroundAddonContextFactory;

            UserAgentFactory = userAgentFactory;

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

        private PluginContextFactory PluginContextFactory { get; }
        private LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        private BackgroundAddonContextFactory BackgroundAddonContextFactory { get; }

        private IHttpUserAgentFactory UserAgentFactory { get; }

        private IViewManager ViewManager {get;}
        private IPlatformTheme PlatformTheme { get; }
        private IImageLoader ImageLoader { get; }
        private IMediaConverter MediaConverter { get; }
        private IPolicy Policy { get; }
        private IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// アドオン一覧。
        /// </summary>
        private ISet<IAddon> Addons { get; } = new HashSet<IAddon>();

        private List<IAddon> LauncherItemSupportAddons => this._launcherItemSupportAddons ??= GetSupportAddons(AddonKind.LauncherItem);
        private List<IAddon> CommandFinderSupportAddons => this._commandFinderSupportAddons ??= GetSupportAddons(AddonKind.CommandFinder);
        private List<IAddon> WidgetSupportAddons => this._widgetSupportAddons ??= GetSupportAddons(AddonKind.Widget);
        private List<IAddon> BackgroundSupportAddons => this._backgroundSupportAddons ??= GetSupportAddons(AddonKind.Background);

        private ConcurrentDictionary<LauncherItemId, LauncherItemAddonProxy> LauncherItemAddonProxies { get; } = new System.Collections.Concurrent.ConcurrentDictionary<LauncherItemId, LauncherItemAddonProxy>();

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
                    using(var readerPack = PluginContextFactory.BarrierRead()) {
                        using var loadContext = PluginContextFactory.CreateLoadContext(target.PluginInformation, readerPack);
                        target.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }

                result.Add(target);
            }

            return result;
        }

        public CommandFinderAddonProxy GetCommandFinder()
        {
            return new CommandFinderAddonProxy(CommandFinderSupportAddons, PluginContextFactory, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
        }

        public BackgroundAddonProxy GetBackground()
        {
            return new BackgroundAddonProxy(BackgroundSupportAddons, PluginContextFactory, BackgroundAddonContextFactory, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
        }

        public IReadOnlyList<WidgetAddonProxy> GetWidgets()
        {
            return WidgetSupportAddons
                .Select(i => new WidgetAddonProxy(i, PluginContextFactory, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory))
                .ToList()
            ;
        }

        public IReadOnlyList<PluginId> GetLauncherItemAddonIds()
        {
            return LauncherItemSupportAddons
                .Select(i => i.PluginInformation.PluginIdentifiers.PluginId)
                .ToList()
            ;
        }

        public LauncherItemAddonProxy GetLauncherItemAddon(LauncherItemId launcherItemId, PluginId pluginId)
        {
            return LauncherItemAddonProxies.GetOrAdd(launcherItemId, (launcherItemId, pluginId) => {
                var addon = LauncherItemSupportAddons.FirstOrDefault(i => i.PluginInformation.PluginIdentifiers.PluginId == pluginId);
                if(addon == null) {
                    throw new PluginNotFoundException($"{nameof(pluginId)}: {pluginId}");
                }
                var proxy = new LauncherItemAddonProxy(launcherItemId, addon, PluginContextFactory, LauncherItemAddonContextFactory, UserAgentFactory, ViewManager, PlatformTheme, ImageLoader, MediaConverter, Policy, DispatcherWrapper, LoggerFactory);
                return proxy;
            }, pluginId);
        }

        #endregion

        #region PluginContainerBase

        /// <inheritdoc cref="PluginContainerBase.Plugins"/>
        public override IEnumerable<IPlugin> Plugins => Addons;

        #endregion
    }
}
