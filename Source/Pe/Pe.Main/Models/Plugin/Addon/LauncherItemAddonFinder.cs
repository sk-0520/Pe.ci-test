using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public interface ILauncherItemAddonFinder
    {
        #region function

        ILauncherItemExtension? Find(Guid pluginId);

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

        public LauncherItemAddonProxy? Find(Guid pluginId)
        {
            var addon = AddonContainer.Plugins.FirstOrDefault(i => i.PluginInformations.PluginIdentifiers.PluginId == pluginId);
            if(addon == null) {
                Logger.LogWarning("プラグイン見つからず: {0}", pluginId);
                return null;
            }

            return AddonContainer.GetLauncherItemAddon(pluginId);
        }

        ILauncherItemExtension? ILauncherItemAddonFinder.Find(Guid pluginId) => Find(pluginId);

        #endregion
    }
}
