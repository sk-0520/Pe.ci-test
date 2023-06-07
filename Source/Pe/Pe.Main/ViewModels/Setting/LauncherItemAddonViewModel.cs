using System;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemAddonViewModel: ViewModelBase
    {
        public LauncherItemAddonViewModel(IAddon addon, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Addon = addon;
        }

        #region property

        private IAddon Addon { get; }

        public string AddonName => Addon.PluginInformation.PluginIdentifiers.PluginName;

        public PluginId PluginId => Addon.PluginInformation.PluginIdentifiers.PluginId;

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
