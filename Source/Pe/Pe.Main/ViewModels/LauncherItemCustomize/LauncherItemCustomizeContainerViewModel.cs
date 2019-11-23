using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Bridge.Models;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeContainerViewModel : SingleModelViewModelBase<LauncherItemCustomizeContainerElement>, IViewLifecycleReceiver, ILauncherItemId
    {
        public LauncherItemCustomizeContainerViewModel(LauncherItemCustomizeContainerElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            Editor = new LauncherItemCustomizeEditorViewModel(Model.Editor, loggerFactory);
            Icon = new LauncherIconViewModel(model.Icon, dispatcherWapper, loggerFactory);
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        public LauncherItemCustomizeEditorViewModel Editor { get; }
        public LauncherIconViewModel Icon { get; }

        #endregion

        #region command

        public ICommand SubmitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Validate()) {
                    Editor.Save();
                    Model.Save();
                    CloseRequest.Send();
                }
            }
        ));

        public ICommand CancelCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        ));

        #endregion

        #region function

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

        #endregion

        #region SingleModelViewModelBase


        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        { }

        public void ReceiveViewUserClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewUserClosing();
        }


        public void ReceiveViewClosing(CancelEventArgs e)
        {
            e.Cancel = !Model.ReceiveViewClosing();
        }

        public void ReceiveViewClosed()
        {
            Model.ReceiveViewClosed();
        }

        #endregion
    }
}
