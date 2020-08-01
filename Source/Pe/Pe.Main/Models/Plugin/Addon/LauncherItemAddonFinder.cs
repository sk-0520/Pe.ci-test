using System;
using System.Collections.Generic;
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
            throw new NotImplementedException();
        }

        ILauncherItemExtension? ILauncherItemAddonFinder.Find(Guid pluginId) => Find(pluginId);

        #endregion
    }
}
