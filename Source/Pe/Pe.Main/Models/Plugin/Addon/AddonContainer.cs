using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models;
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

        List<IAddon>? _launcherItemSupportAddons;
        List<IAddon>? _commandFinderSupportAddons;
        List<IAddon>? _widgetSupportAddons;
        List<IAddon>? _backgroundSupportAddons;


        #endregion
        public AddonContainer(PluginContextFactory pluginContextFactory, LauncherItemAddonContextFactory launcherItemAddonContextFactory, BackgroundAddonContextFactory backgroundAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IMediaConverter mediaConverter, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginContextFactory = pluginContextFactory;
            LauncherItemAddonContextFactory = launcherItemAddonContextFactory;
            BackgroundAddonContextFactory = backgroundAddonContextFactory;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            MediaConverter = mediaConverter;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        PluginContextFactory PluginContextFactory { get; }
        LauncherItemAddonContextFactory LauncherItemAddonContextFactory { get; }
        BackgroundAddonContextFactory BackgroundAddonContextFactory { get; }

        IHttpUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
        IMediaConverter MediaConverter { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        /// <summary>
        /// アドオン一覧。
        /// </summary>
        ISet<IAddon> Addons { get; } = new HashSet<IAddon>();

        List<IAddon> LauncherItemSupportAddons => this._launcherItemSupportAddons ??= GetSupportAddons(AddonKind.LauncherItem);
        List<IAddon> CommandFinderSupportAddons => this._commandFinderSupportAddons ??= GetSupportAddons(AddonKind.CommandFinder);
        List<IAddon> WidgetSupportAddons => this._widgetSupportAddons ??= GetSupportAddons(AddonKind.Widget);
        List<IAddon> BackgroundSupportAddons => this._backgroundSupportAddons ??= GetSupportAddons(AddonKind.Background);

        ConcurrentDictionary<Guid, LauncherItemAddonProxy> LauncherItemAddonProxies { get; } = new System.Collections.Concurrent.ConcurrentDictionary<Guid, LauncherItemAddonProxy>();

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
                        using var loadContext = PluginContextFactory.CreateLoadContex(target.PluginInformations, readerPack);
                        target.Load(Bridge.Plugin.PluginKind.Addon, loadContext);
                    }
                }

                result.Add(target);
            }

            return result;
        }

        public CommandFinderAddonProxy GetCommandFinder()
        {
            return new CommandFinderAddonProxy(CommandFinderSupportAddons, PluginContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
        }

        public BackgroundAddonProxy GetBackground()
        {
            return new BackgroundAddonProxy(BackgroundSupportAddons, PluginContextFactory, BackgroundAddonContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
        }

        public IReadOnlyList<WidgetAddonProxy> GetWidgets()
        {
            return WidgetSupportAddons
                .Select(i => new WidgetAddonProxy(i, PluginContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory))
                .ToList()
            ;
        }

        public IReadOnlyList<Guid> GetLauncherItemAddonIds()
        {
            return LauncherItemSupportAddons
                .Select(i => i.PluginInformations.PluginIdentifiers.PluginId)
                .ToList()
            ;
        }

        public LauncherItemAddonProxy GetLauncherItemAddon(Guid launcherItemId, Guid pluginId)
        {
            return LauncherItemAddonProxies.GetOrAdd(launcherItemId, (launcherItemId, pluginId) => {
                var addon = LauncherItemSupportAddons.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
                if(addon == null) {
                    throw new PluginNotFoundException($"{nameof(pluginId)}: {pluginId}");
                }
                var proxy = new LauncherItemAddonProxy(launcherItemId, addon, PluginContextFactory, LauncherItemAddonContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, MediaConverter, DispatcherWrapper, LoggerFactory);
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
