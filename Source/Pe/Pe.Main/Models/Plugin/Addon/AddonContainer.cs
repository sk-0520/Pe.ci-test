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
    internal class AddonContainer: PluginContainerBase
    {
        #region variable

        List<IAddon>? _launcherItemSupportAddons;
        List<IAddon>? _commandFinderSupportAddons;
        List<IAddon>? _widgetSupportAddons;
        List<IAddon>? _backgroundSupportAddons;


        #endregion
        public AddonContainer(PluginContextFactory pluginContextFactory, BackgroundAddonContextFactory backgroundAddonContextFactory, IHttpUserAgentFactory userAgentFactory, IPlatformTheme platformTheme, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            PluginContextFactory = pluginContextFactory;
            BackgroundAddonContextFactory = backgroundAddonContextFactory;

            UserAgentFactory = userAgentFactory;
            PlatformTheme = platformTheme;
            ImageLoader = imageLoader;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }

        PluginContextFactory PluginContextFactory { get; }
        BackgroundAddonContextFactory BackgroundAddonContextFactory { get; }

        IHttpUserAgentFactory UserAgentFactory { get; }
        IPlatformTheme PlatformTheme { get; }
        IImageLoader ImageLoader { get; }
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
            return new CommandFinderAddonProxy(CommandFinderSupportAddons, PluginContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, DispatcherWrapper, LoggerFactory);
        }

        public BackgroundAddonProxy GetBackground()
        {
            return new BackgroundAddonProxy(BackgroundSupportAddons, PluginContextFactory, BackgroundAddonContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, DispatcherWrapper, LoggerFactory);
        }

        public IReadOnlyList<WidgetAddonProxy> GetWidgets()
        {
            return WidgetSupportAddons
                .Select(i => new WidgetAddonProxy(i, PluginContextFactory, UserAgentFactory, PlatformTheme, ImageLoader, DispatcherWrapper, LoggerFactory))
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

        #endregion

        #region PluginContainerBase

        /// <inheritdoc cref="PluginContainerBase.Plugins"/>
        public override IEnumerable<IPlugin> Plugins => Addons;

        #endregion

    }
}
