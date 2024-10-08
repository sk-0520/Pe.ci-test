using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Library.Base;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeContainerViewModel: ElementViewModelBase<LauncherItemCustomizeContainerElement>, IViewLifecycleReceiver, ILauncherItemId
    {
        public LauncherItemCustomizeContainerViewModel(LauncherItemCustomizeContainerElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            Editor = new LauncherItemCustomizeEditorViewModel(Model.Editor, DispatcherWrapper, loggerFactory);
        }

        #region property
        public RequestSender CloseRequest { get; } = new RequestSender();

        public LauncherItemCustomizeEditorViewModel Editor { get; }

        public string Title
        {
            get
            {
                return TextUtility.ReplaceFromDictionary(
                    Properties.Resources.String_LauncherItemCustomizeWindow_Caption,
                    new Dictionary<string, string>() {
                        ["ITEM"] = Model.CaptionName,
                    }
                );
            }
        }


        #endregion

        #region command

        private ICommand? _SubmitCommand;
        public ICommand SubmitCommand => this._SubmitCommand ??= new DelegateCommand(
            () => {
                Editor.Flush();
                if(Validate()) {
                    Model.Save();
                    CloseRequest.Send();
                }
            }
        );

        private ICommand? _CancelCommand;
        public ICommand CancelCommand => this._CancelCommand ??= new DelegateCommand(
            () => {
                CloseRequest.Send();
            }
        );

        #endregion

        #region function

        #endregion

        #region ILauncherItemId

        public LauncherItemId LauncherItemId => Model.LauncherItemId;

        #endregion

        #region SingleModelViewModelBase


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

        public Task ReceiveViewClosedAsync(Window window, bool isUserOperation, CancellationToken cancellationToken)
        {
            return Model.ReceiveViewClosedAsync(isUserOperation, cancellationToken);
        }

        #endregion
    }
}
