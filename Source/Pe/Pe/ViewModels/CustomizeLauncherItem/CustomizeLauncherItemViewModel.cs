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
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.CustomizeLauncherItem
{
    public class CustomizeLauncherItemViewModel : SingleModelViewModelBase<CustomizeLauncherItemElement>, IViewLifecycleReceiver, ILauncherItemId
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

        private void Save()
        {

            var common = CustomizeItems.OfType<CustomizeLauncherCommonViewModel>().First();
            var tag = CustomizeItems.OfType<CustomizeLauncherTagViewModel>().First();
            var comment = CustomizeItems.OfType<CustomizeLauncherCommentViewModel>().First();

            switch(Model.Kind) {
                case LauncherItemKind.File:
                    var file = CustomizeItems.OfType<CustomizeLauncherFileViewModel>().First();
                    var env = CustomizeItems.OfType<CustomizeLauncherEnvironmentVariableViewModel>().First();

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

                    // この層でやるべきなんかねぇ
                    var envVarItems = env.GetMergeItems().ToList();
                    foreach(var item in env.GetRemoveItems()) {
                        var index = envVarItems.FindIndex(i => i.Name == item);
                        if(index != -1) {
                            envVarItems.RemoveAt(index);
                        }
                        envVarItems.Add(new LauncherEnvironmentVariableData() {
                            Name = item
                        });
                    }

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
