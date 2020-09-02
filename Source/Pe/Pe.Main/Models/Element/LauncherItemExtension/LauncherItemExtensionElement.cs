using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
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

        public bool HasView => Informations.Any(i => i.WindowItem.Window.IsVisible);

        #endregion

        #region function

        public void CloseView()
        {
            foreach(var information in Informations.ToArray()) {
                information.WindowItem.Window.Close();
            }
        }

        public void Add(LauncherItemAddonViewInformation launcherItemAddonViewInformation)
        {
            Informations.Add(launcherItemAddonViewInformation);
        }

        public IEnumerable<LauncherItemAddonViewInformation> GetInformations() => Informations;

        public bool ReceiveViewUserClosing(Window window)
        {
            var information = Informations.FirstOrDefault(i => i.WindowItem.Window == window);
            if(information == null) {
                Logger.LogWarning("対象ウィンドウは未登録: {0}", HandleUtility.GetWindowHandle(window));
                return true;
            }

            return information.UserClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            var information = Informations.FirstOrDefault(i => i.WindowItem.Window == window);
            if(information == null) {
                Logger.LogWarning("対象ウィンドウは未登録: {0}", HandleUtility.GetWindowHandle(window));
                return;
            }

            information.ClosedWindow();
            Informations.Remove(information);
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
