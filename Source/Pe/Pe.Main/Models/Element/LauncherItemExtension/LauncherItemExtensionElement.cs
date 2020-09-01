using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Element;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemExtensionElement: ElementBase, ILauncherItemId
    {

        public LauncherItemExtensionElement(IPluginInformations pluginInformations, Guid launcherItemId, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginInformations = pluginInformations;
            LauncherItemId = launcherItemId;
        }

        #region property

        IPluginInformations PluginInformations { get; }
        ISet<LauncherItemAddonViewInformation> Informations { get; } = new HashSet<LauncherItemAddonViewInformation>();

        #endregion

        #region function

        public void Add(LauncherItemAddonViewInformation launcherItemAddonViewInformation)
        {
            Informations.Add(launcherItemAddonViewInformation);
        }


        #endregion

        #region LauncherItemExtensionElement

        protected override void InitializeImpl()
        {
            Logger.LogTrace("ランチャーアイテムアドオン初期化: {0}", PluginInformations.PluginIdentifiers);
        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion
    }
}
