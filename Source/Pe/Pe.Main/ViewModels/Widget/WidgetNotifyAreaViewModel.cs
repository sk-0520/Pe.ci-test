using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin;
using ContentTypeTextNet.Pe.Bridge.Plugin.Addon;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Plugin.Addon;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.Manager;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Widget
{
    public class WidgetNotifyAreaViewModel: WidgetViewModelBase<WidgetElement>, INotifyArea
    {
        public WidgetNotifyAreaViewModel(WidgetElement model, IUserTracker userTracker, IWindowManager windowManager, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, windowManager, dispatcherWrapper, loggerFactory)
        { }

        #region property
        #endregion

        #region command

        #endregion

        #region function

        #endregion

        #region INotifyArea

        public string MenuHeader => Model.GetMenuHeader();
        public bool MenuHeaderHasAccessKey { get; } = false;
        public KeyGesture? MenuKeyGesture { get; }
        public DependencyObject? MenuIcon => DispatcherWrapper.Get(() => Model.GetMenuIcon());
        public bool MenuHasIcon { get; } = true;
        public bool MenuIsEnabled { get; } = true;
        public bool MenuIsChecked => Model.ViewCreated;

        public ICommand MenuCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Model.ViewCreated) {
                    Model.SaveStatus(false);
                    Model.HideView();
                } else {
                    Model.ShowView(this);
                    Model.SaveStatus(true);
                }
            },
            () => MenuIsEnabled
        ));

        #endregion
    }
}
