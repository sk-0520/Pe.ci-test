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
using ContentTypeTextNet.Pe.Main.Models.Element.CustomizeLauncherItem;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItem;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherItemViewModel : SingleModelViewModelBase<CustomizeLauncherItemElement>, IViewLifecycleReceiver
    {
        #region variable

        List<CustomizeLauncherDetailViewModelBase>? _customizeItems;

        #endregion

        public CustomizeLauncherItemViewModel(CustomizeLauncherItemElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        public List<CustomizeLauncherDetailViewModelBase> CustomizeItems
        {
            get
            {
                if(this._customizeItems == null) {
                    this._customizeItems = CreateCustomizeItems().ToList();
                    foreach(var item in this._customizeItems) {
                        item.Initialize();
                    }
                }

                return this._customizeItems;
            }
        }

        #endregion

        #region command

        public ICommand SubmitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Validate()) {
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

        IEnumerable<CustomizeLauncherDetailViewModelBase> CreateCustomizeItems()
        {
            yield return new CustomizeLauncherCommonViewModel(Model, LoggerFactory);

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    yield return new CustomizeLauncherFileViewModel(Model, LoggerFactory);
                    yield return new CustomizeLauncherEnvironmentVariableViewModel(Model, LoggerFactory);
                    break;

                default:
                    throw new NotImplementedException();
            }

            yield return new CustomizeLauncherTagViewModel(Model, LoggerFactory);
            yield return new CustomizeLauncherCommentViewModel(Model, LoggerFactory);
        }

        #endregion

        #region CustomizeLauncherItemViewModel
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
