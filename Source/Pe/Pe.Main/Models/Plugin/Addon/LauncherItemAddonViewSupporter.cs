using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Plugin.Addon
{
    public class LauncherItemAddonViewSupporterCollection
    {
        public LauncherItemAddonViewSupporterCollection(IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            WindowManager = windowManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        IWindowManager WindowManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        ISet<LauncherItemAddonViewSupporter> LauncherItemAddonViewSupporters { get; } = new HashSet<LauncherItemAddonViewSupporter>();

        #endregion

        #region function

        public bool Exists(Guid launcherItemId)
        {
            return LauncherItemAddonViewSupporters.Any(i => i.LauncherItemId == launcherItemId);
        }

        public ILauncherItemAddonViewSupporter Create(Guid launcherItemId)
        {
            if(Exists(launcherItemId)) {
                throw new InvalidOperationException($"{nameof(launcherItemId)}: {launcherItemId}");
            }

            var result = new LauncherItemAddonViewSupporter(launcherItemId, WindowManager, DispatcherWrapper, LoggerFactory);
            LauncherItemAddonViewSupporters.Add(result);
            return result;
        }

        #endregion
    }

    internal class LauncherItemAddonViewSupporter: ILauncherItemAddonViewSupporter, ILauncherItemId
    {
        public LauncherItemAddonViewSupporter(Guid launcherItemId, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());
            LauncherItemId = launcherItemId;
            WindowManager = windowManager;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        ILogger Logger { get; }
        ILoggerFactory LoggerFactory { get; }
        IWindowManager WindowManager { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId { get; }

        #endregion

        #region ILauncherItemAddonViewSupporter

        /// <inheritdoc cref="ILauncherItemAddonViewSupporter.RegisterWindow(Window, Func{bool}?, Action?)"/>
        public bool RegisterWindow(Window window, Func<bool>? userClosing, Action? closedWindow)
        {
            //throw new NotImplementedException();
            window.Show();
            return true;
        }

        #endregion
    }
}
