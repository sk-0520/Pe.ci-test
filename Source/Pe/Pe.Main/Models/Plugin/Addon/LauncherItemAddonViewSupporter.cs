using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemExtension;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonViewSupporterCollection
    {
        public LauncherItemAddonViewSupporterCollection(IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        IOrderManager OrderManager { get; }
        IWindowManager WindowManager { get; }
        IUserTracker UserTracker { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        ISet<LauncherItemAddonViewSupporter> LauncherItemAddonViewSupporters { get; } = new HashSet<LauncherItemAddonViewSupporter>();

        #endregion

        #region function

        public bool Exists(Guid launcherItemId)
        {
            return LauncherItemAddonViewSupporters.Any(i => i.LauncherItemId == launcherItemId);
        }

        /// <summary>
        /// 対象のランチャーアイテムアドオンを活性化。
        /// </summary>
        /// <param name="launcherItemId"></param>
        public void Foreground(Guid launcherItemId)
        {
            // TODO: ウィンドウ位置移動
        }

        public ILauncherItemAddonViewSupporter Create(IPluginInformations pluginInformations, Guid launcherItemId)
        {
            if(Exists(launcherItemId)) {
                throw new InvalidOperationException($"{nameof(launcherItemId)}: {launcherItemId}");
            }

            var result = new LauncherItemAddonViewSupporter(pluginInformations, launcherItemId, OrderManager, WindowManager, UserTracker, DispatcherWrapper, LoggerFactory);
            LauncherItemAddonViewSupporters.Add(result);
            return result;
        }

        #endregion
    }

    public class LauncherItemAddonViewInformation
    {
        public LauncherItemAddonViewInformation(WindowItem windowItem, Func<bool>? userClosing, Action? closedWindow)
        {
            WindowItem = windowItem;
            UserClosing = userClosing ?? DefaultUserClosing;
            ClosedWindow = closedWindow ?? DefaultClosedWindow;
        }

        #region proeprty

        public WindowItem WindowItem { get; }
        public Func<bool> UserClosing { get; }
        public Action ClosedWindow { get; }

        #endregion

        #region function

        static bool DefaultUserClosing() => false;
        static void DefaultClosedWindow() { }

        #endregion
    }

    internal class LauncherItemAddonViewSupporter: ILauncherItemAddonViewSupporter, ILauncherItemId
    {
        public LauncherItemAddonViewSupporter(IPluginInformations pluginInformations, Guid launcherItemId, IOrderManager orderManager, IWindowManager windowManager, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            PluginInformations = pluginInformations;
            LauncherItemId = launcherItemId;
            OrderManager = orderManager;
            WindowManager = windowManager;
            UserTracker = userTracker;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        IPluginInformations PluginInformations { get; }
        IWindowManager WindowManager { get; }
        IOrderManager OrderManager { get; }
        IUserTracker UserTracker { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        LauncherItemExtensionElement? Element { get; set; }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ILauncherItemAddonViewSupporter

        /// <inheritdoc cref="ILauncherItemAddonViewSupporter.RegisterWindow(Window, Func{bool}?, Action?)"/>
        public bool RegisterWindow(Window window, Func<bool>? userClosing, Action? closedWindow)
        {
            if(window == null) {
                throw new ArgumentNullException(nameof(window));
            }
            if(window.IsVisible) {
                Logger.LogError("ランチャーアイテムアドオンの初期表示は Pe 側で処理される必要がある");
                window.Close();
                return false;
            }

            if(Element == null) {
                Element = OrderManager.CreateLauncherItemExtensionElement(PluginInformations, LauncherItemId);
            }
            //NOTE: 引数がどんどこ増えるようなら IOrderManager に移す
            var windowItem = new WindowItem(WindowKind.LauncherItemExtension, Element, new LauncherItemExtensionViewModel(Element, UserTracker, DispatcherWrapper, LoggerFactory), window);
            if(WindowManager.Register(windowItem)) {
                var info = new LauncherItemAddonViewInformation(windowItem, userClosing, closedWindow);
                Element.Add(info);
                window.Show();
                return true;
            }

            return false;
        }

        #endregion
    }
}
