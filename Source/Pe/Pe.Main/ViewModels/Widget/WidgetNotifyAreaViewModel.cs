using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public class WidgetNotifyAreaViewModel: ViewModelBase, INotifyArea
    {
        internal WidgetNotifyAreaViewModel(IWidget widget, IPluginInformations pluginInformations, PluginContextFactory pluginContextFactory, WidgetAddonContextFactory widgetAddonContextFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Widget = widget;
            PluginInformations = pluginInformations;
            PluginContextFactory = pluginContextFactory;
            WidgetAddonContextFactory = widgetAddonContextFactory;
            DispatcherWrapper = dispatcherWrapper;
        }

        #region property

        IWidget Widget { get; }
        IPluginInformations PluginInformations { get; }
        PluginContextFactory PluginContextFactory { get; }
        WidgetAddonContextFactory WidgetAddonContextFactory { get; }

        IDispatcherWrapper DispatcherWrapper { get; }

        #endregion

        #region INotifyArea

        public string MenuHeader
        {
            get
            {
                using(var reader = PluginContextFactory.BarrierRead()) {
                    var context = PluginContextFactory.CreateContext(PluginInformations, reader, true);
                    return Widget.GetMenuHeader(context);
                }
            }
        }
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture? MenuKeyGesture { get; }
        public DependencyObject? MenuIcon
        {
            get
            {
                using(var reader = PluginContextFactory.BarrierRead()) {
                    var context = PluginContextFactory.CreateContext(PluginInformations, reader, true);
                    return Widget.GetMenuIcon(context);
                }
            }
        }
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked { get; } = false;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {

            },
            () => MenuIsEnabled
        ));

        #endregion
    }
}
