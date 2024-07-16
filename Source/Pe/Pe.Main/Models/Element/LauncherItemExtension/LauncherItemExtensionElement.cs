using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemExtensionElement: ElementBase, ILauncherItemId
    {

        public LauncherItemExtensionElement(IPluginInformation pluginInformation, LauncherItemId launcherItemId, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            PluginInformation = pluginInformation;
            LauncherItemId = launcherItemId;
        }

        #region property

        private IPluginInformation PluginInformation { get; }
        private ISet<LauncherItemAddonViewInformation> InformationItems { get; } = new HashSet<LauncherItemAddonViewInformation>();

        public bool HasView => InformationItems.Any(i => i.WindowItem.Window.IsVisible);

        #endregion

        #region function

        public void CloseView()
        {
            foreach(var information in InformationItems.ToArray()) {
                information.WindowItem.Window.Close();
            }
        }

        public void Add(LauncherItemAddonViewInformation launcherItemAddonViewInformation)
        {
            InformationItems.Add(launcherItemAddonViewInformation);
        }

        public IEnumerable<LauncherItemAddonViewInformation> GetInformationItems() => InformationItems;

        public bool ReceiveViewUserClosing(Window window)
        {
            var information = InformationItems.FirstOrDefault(i => i.WindowItem.Window == window);
            if(information == null) {
                Logger.LogWarning("対象ウィンドウは未登録: {0}", HandleUtility.GetWindowHandle(window));
                return true;
            }

            return information.UserClosing();
        }

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            var information = InformationItems.FirstOrDefault(i => i.WindowItem.Window == window);
            if(information == null) {
                Logger.LogWarning("対象ウィンドウは未登録: {0}", HandleUtility.GetWindowHandle(window));
                return Task.CompletedTask;
            }

            information.ClosedWindow();
            InformationItems.Remove(information);

            return Task.CompletedTask;
        }

        #endregion

        #region LauncherItemExtensionElement

        protected override Task InitializeCoreAsync(CancellationToken cancellationToken)
        {
            Logger.LogTrace("ランチャーアイテムアドオン初期化: {0}", PluginInformation.PluginIdentifiers);
            return Task.CompletedTask;
        }

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId { get; }

        #endregion
    }
}
