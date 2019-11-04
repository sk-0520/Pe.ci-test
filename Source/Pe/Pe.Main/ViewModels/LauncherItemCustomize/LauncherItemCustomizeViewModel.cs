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

namespace ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize
{
    public class LauncherItemCustomizeViewModel : SingleModelViewModelBase<LauncherItemCustomizeElement>, IViewLifecycleReceiver, ILauncherItemId
    {
        #region variable

        List<LauncherItemCustomizeDetailViewModelBase>? _customizeItems;

        #endregion

        public LauncherItemCustomizeViewModel(LauncherItemCustomizeElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        { }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();

        public List<LauncherItemCustomizeDetailViewModelBase> CustomizeItems
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
                    Save();
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

        IEnumerable<LauncherItemCustomizeDetailViewModelBase> CreateCustomizeItems()
        {
            yield return new LauncherItemCustomizeCommonViewModel(Model, LoggerFactory);

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    yield return new LauncherItemCustomizeFileViewModel(Model, LoggerFactory);
                    yield return new LauncherItemCustomizeEnvironmentVariableViewModel(Model, LoggerFactory);
                    break;

                default:
                    throw new NotImplementedException();
            }

            yield return new LauncherItemCustomizeTagViewModel(Model, LoggerFactory);
            yield return new LauncherItemCustomizeCommentViewModel(Model, LoggerFactory);
        }

        private void Save()
        {

            var common = CustomizeItems.OfType<LauncherItemCustomizeCommonViewModel>().First();
            var tag = CustomizeItems.OfType<LauncherItemCustomizeTagViewModel>().First();
            var comment = CustomizeItems.OfType<LauncherItemCustomizeCommentViewModel>().First();

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    var file = CustomizeItems.OfType<LauncherItemCustomizeFileViewModel>().First();
                    var env = CustomizeItems.OfType<LauncherItemCustomizeEnvironmentVariableViewModel>().First();

                    var itemData = new LauncherItemData() {
                        LauncherItemId = Model.LauncherItemId,
                        Kind = Model.Kind,
                        Code = common.Code,
                        Name = common.Name,
                        IsEnabledCommandLauncher = true,
                        Comment = comment.CommentDocument!.Text,
                        Icon = new IconData() {
                            Path = common.IconData!.Path,
                            Index = common.IconData!.Index,
                        },
                    };
                    var fileData = new LauncherFileData() {
                        Path = file.Path,
                        Option = file.Option,
                        WorkDirectoryPath = file.WorkingDirectoryPath,
                        IsEnabledCustomEnvironmentVariable = file.IsEnabledCustomEnvironmentVariable,
                        IsEnabledStandardInputOutput = file.IsEnabledStandardInputOutput,
                        RunAdministrator = file.RunAdministrator,
                    };

                    var envVarItems = env.GetEnvironmentVariableItems();

                    var tagItems = tag.GetTagItems();
                    Model.SaveFile(itemData, fileData, envVarItems, tagItems);
                    break;
            }

        }

        #endregion

        #region ILauncherItemId

        public Guid LauncherItemId => Model.LauncherItemId;

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
