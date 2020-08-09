using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public interface ILauncherItemAddonFinder
    {
        #region function

        bool Exists(Guid pluginId);

        IPlugin GetPlugin(Guid pluginId);

        ILauncherItemExtension Find(Guid launcherItemId, Guid pluginId);

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

        AddonContainer AddonContainer { get; }
        ILogger Logger { get; }

        #endregion

        #region ILauncherItemAddonFinder

        public bool Exists(Guid pluginId)
        {
            return AddonContainer.Plugins.Any(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
        }

        public IPlugin GetPlugin(Guid pluginId)
        {
            return AddonContainer.Plugins.First(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
        }

        public LauncherItemAddonProxy Find(Guid launcherItemId, Guid pluginId)
        {
            return AddonContainer.GetLauncherItemAddon(launcherItemId, pluginId);
        }

        ILauncherItemExtension ILauncherItemAddonFinder.Find(Guid launcherItemId, Guid pluginId) => Find(launcherItemId, pluginId);

        #endregion
    }
}
