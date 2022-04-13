using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Plugin
{
    public class PluginWebInstallViewModel: ElementViewModelBase<PluginWebInstallElement>, IViewLifecycleReceiver
    {
        #region variable

        bool _nowInstalling;

        #endregion

        public PluginWebInstallViewModel(PluginWebInstallElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        { }

        #region property

        public string PluginIdOrInfoUrl
        {
            get => Model.PluginIdOrInfoUrl;
            set => SetModelValue(value);
        }

        public bool NowInstalling
        {
            get => this._nowInstalling;
            set
            {
                SetProperty(ref this._nowInstalling, value);
            }
        }

        #endregion

        #region command

        public ICommand InstallCommand => GetOrCreateCommand(() => new DelegateCommand<Window>(
            async o => {
                NowInstalling = true;
                try {
                    await Task.Delay(1000);
                } finally {
                    NowInstalling = false;
                }
            },
            o => !NowInstalling
        ));

        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        {
            Model.ReceiveViewClosed(isUserOperation);
        }

        #endregion
    }
}
