using System;
using System.Linq;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public interface ILauncherItemAddonFinder
    {
        #region function

        bool Exists(PluginId pluginId);

        IPlugin GetPlugin(PluginId pluginId);

        ILauncherItemExtension Find(LauncherItemId launcherItemId, PluginId pluginId);

        #endregion
    }

    internal class LauncherItemAddonFinder: ILauncherItemAddonFinder
    {
        public LauncherItemAddonFinder(AddonContainer addonContainer, ILoggerFactory loggerFactory)
        {
            AddonContainer = addonContainer;
            Logger = loggerFactory.CreateLogger(GetType());
        }

        #region property

        private AddonContainer AddonContainer { get; }
        private ILogger Logger { get; }

        #endregion

        #region ILauncherItemAddonFinder

        public bool Exists(PluginId pluginId)
        {
            return AddonContainer.Plugins.Any(i => i.PluginInformation.PluginIdentifiers.PluginId == pluginId);
        }

        public IPlugin GetPlugin(PluginId pluginId)
        {
            return AddonContainer.Plugins.First(i => i.PluginInformation.PluginIdentifiers.PluginId == pluginId);
        }

        public LauncherItemAddonProxy Find(LauncherItemId launcherItemId, PluginId pluginId)
        {
            return AddonContainer.GetLauncherItemAddon(launcherItemId, pluginId);
        }

        ILauncherItemExtension ILauncherItemAddonFinder.Find(LauncherItemId launcherItemId, PluginId pluginId) => Find(launcherItemId, pluginId);

        #endregion
    }
}
