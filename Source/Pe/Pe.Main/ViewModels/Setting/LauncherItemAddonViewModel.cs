using System;
using System.Collections.Generic;
using System.Text;
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

        IAddon Addon { get; }

        public string AddonName => Addon.PluginInformations.PluginIdentifiers.PluginName;

        public Guid PluginId => Addon.PluginInformations.PluginIdentifiers.PluginId;

        #endregion

        #region command

        #endregion

        #region function

        #endregion
    }
}
