using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.About;
using ContentTypeTextNet.Pe.Main.Models.Platform;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.About
{
    public class AboutViewModel: ElementViewModelBase<AboutElement>
    {
        #region variable

        private string _uninstallBatchFilePath = string.Empty;

        #endregion

        public AboutViewModel(AboutElement model, IUserTracker userTracker, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            ComponentCollection = new ObservableCollection<AboutComponentItemViewModel>(model.Components.Select(i => new AboutComponentItemViewModel(i, LoggerFactory)));
            ComponentItems = CollectionViewSource.GetDefaultView(ComponentCollection);
            ComponentItems.GroupDescriptions.Add(new PropertyGroupDescription(nameof(AboutComponentItemViewModel.Kind)));
            ComponentItems.SortDescriptions.Add(new SortDescription(nameof(AboutComponentItemViewModel.Sort), ListSortDirection.Ascending));
        }

        #region property

        public RequestSender CloseRequest { get; } = new RequestSender();
        public RequestSender FileSelectRequest { get; } = new RequestSender();
        public RequestSender ShowMessageRequest { get; } = new RequestSender();

        private ObservableCollection<AboutComponentItemViewModel> ComponentCollection { get; }
        public ICollectionView ComponentItems { get; }

        /// <summary>
        /// 削除対象。
        /// <para>初期値ではユーザーデータを一応残しておく。</para>
        /// </summary>
        private UninstallTarget UninstallTargets { get; set; } = UninstallTarget.Application | UninstallTarget.Batch | UninstallTarget.Machine | UninstallTarget.Temporary;
        public string UninstallBatchFilePath
        {
            get => this._uninstallBatchFilePath;
            set => SetProperty(ref this._uninstallBatchFilePath, value);
        }

        public bool UninstallTargetUser
        {
            get => UninstallTargets.HasFlag(UninstallTarget.User);
            set => ChangeUninstallTarget(UninstallTarget.User, value);
        }
        public bool UninstallTargetMachine
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Machine);
            set => ChangeUninstallTarget(UninstallTarget.Machine, value);
        }
        public bool UninstallTargetTemporary
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Temporary);
            set => ChangeUninstallTarget(UninstallTarget.Temporary, value);
        }
        public bool UninstallTargetApplication
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Application);
            set => ChangeUninstallTarget(UninstallTarget.Application, value);
        }

        public bool UninstallTargetBatch
        {
            get => UninstallTargets.HasFlag(UninstallTarget.Batch);
            set => ChangeUninstallTarget(UninstallTarget.Batch, value);
        }

        #endregion

        #region command

        public ICommand OpenLicenseCommand => GetOrCreateCommand(() => new DelegateCommand<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.LicenseUri);
            }
        ));

        public ICommand OpenUriCommand => GetOrCreateCommand(() => new DelegateCommand<AboutComponentItemViewModel>(
            o => {
                Model.OpenUri(o.Uri);
            }
        ));
        public ICommand OpenForumUriCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenForumUri();
            }
        ));
        public ICommand OpenRepositoryUriCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenRepositoryUri();
            }
        ));
        public ICommand OpenWebsiteUriCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenWebsiteUri();
            }
        ));
        public ICommand CopyShortInformationCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyShortInformation();
            }
        ));
        public ICommand CopyLongInformationCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.CopyLongInformation();
            }
        ));

        public ICommand OpenApplicationDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenApplicationDirectory();
            }
        ));
        public ICommand OpenUserDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenUserDirectory();
            }
        ));
        public ICommand OpenMachineDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenMachineDirectory();
            }
        ));
        public ICommand OpenTemporaryDirectoryCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.OpenTemporaryDirectory();
            }
        ));

        public ICommand SelectUninstallBatchFilePathCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 var dialogRequester = new DialogRequester(LoggerFactory);
                 dialogRequester.SelectFile(
                     FileSelectRequest,
                     string.Empty,
                     false,
                     new[] {
                        new DialogFilterItem(Properties.Resources.String_FileDialog_Filter_About_Uninstall, "bat", "*.bat"),
                     },
                     r => {
                         var path = r.ResponseFilePaths[0];
                         try {
                             UninstallBatchFilePath = path;
                         } catch(Exception ex) {
                             Logger.LogError(ex, ex.Message);
                         }
                     }
                 );
             }
         ));

        public ICommand CreateUninstallBatchCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Model.CheckCreateUninstallBatch(UninstallBatchFilePath, UninstallTargets)) {
                    try {
                        Model.CreateUninstallBatch(UninstallBatchFilePath, UninstallTargets);

                        try {
                            var systemExecutor = new SystemExecutor();
                            systemExecutor.OpenDirectoryWithFileSelect(UninstallBatchFilePath);
                        } catch(Exception ex) {
                            Logger.LogWarning(ex, ex.Message);
                        }

                        ShowMessageRequest.Send(new CommonMessageDialogRequestParameter() {
                            Caption = Properties.Resources.String_About_Uninstall_Create_Caption,
                            Message = Properties.Resources.String_About_Uninstall_Create_Message,
                            Button = System.Windows.MessageBoxButton.OK,
                            DefaultResult = System.Windows.MessageBoxResult.OK,
                            Icon = System.Windows.MessageBoxImage.Information,
                        });
                    } catch(Exception ex) {
                        Logger.LogError(ex, ex.Message);
                        ShowMessageRequest.Send(new CommonMessageDialogRequestParameter() {
                            Caption = Properties.Resources.String_About_Uninstall_Create_Caption,
                            Message = ex.ToString(),
                            Button = System.Windows.MessageBoxButton.OK,
                            DefaultResult = System.Windows.MessageBoxResult.OK,
                            Icon = System.Windows.MessageBoxImage.Error,
                        });
                    }
                }
            }
        ));


        #endregion

        #region function

        private void ChangeUninstallTarget(UninstallTarget uninstallTarget, bool isEnabled, [CallerMemberName] string callerMemberName = "")
        {
            if(isEnabled) {
                UninstallTargets |= uninstallTarget;
            } else {
                UninstallTargets &= ~uninstallTarget;
            }

            RaisePropertyChanged(callerMemberName);
        }

        #endregion
    }
}
