using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Plugin;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginsSettingEditorViewModel: SettingEditorViewModelBase<PluginsSettingEditorElement>
    {
        #region variable

        private PluginSettingEditorViewModel? _selectedPlugin;

        #endregion

        public PluginsSettingEditorViewModel(PluginsSettingEditorElement model, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ImageLoader = imageLoader;
            PluginCollection = new ModelViewModelObservableCollectionManager<PluginSettingEditorElement, PluginSettingEditorViewModel>(Model.PluginItems, new ModelViewModelObservableCollectionOptions<PluginSettingEditorElement, PluginSettingEditorViewModel>() {
                ToViewModel = m => new PluginSettingEditorViewModel(m, ImageLoader, DispatcherWrapper, LoggerFactory),
            });
            PluginItems = PluginCollection.GetDefaultView();

            InstallPluginCollection = new ModelViewModelObservableCollectionManager<PluginInstallItemElement, PluginInstallItemViewModel>(Model.InstallPluginItems, new ModelViewModelObservableCollectionOptions<PluginInstallItemElement, PluginInstallItemViewModel>() {
                ToViewModel = m => new PluginInstallItemViewModel(m, LoggerFactory),
            });
            InstallPluginItems = InstallPluginCollection.GetDefaultView();
        }

        #region property

        public RequestSender SelectPluginFileRequest { get; } = new RequestSender();
        public RequestSender ShowMessageRequest { get; } = new RequestSender();
        public RequestSender WebInstallRequest { get; } = new RequestSender();

        private ModelViewModelObservableCollectionManager<PluginSettingEditorElement, PluginSettingEditorViewModel> PluginCollection { get; }
        public ICollectionView PluginItems { get; }

        private ModelViewModelObservableCollectionManager<PluginInstallItemElement, PluginInstallItemViewModel> InstallPluginCollection { get; }
        public ICollectionView InstallPluginItems { get; }

        public PluginSettingEditorViewModel? SelectedPlugin
        {
            get => this._selectedPlugin;
            set => SetProperty(ref this._selectedPlugin, value);
        }

        private IImageLoader ImageLoader { get; }

        #endregion

        #region command

        private ICommand? _CancelInstallCommand;
        public ICommand CancelInstallCommand => this._CancelInstallCommand ??= new DelegateCommand<PluginInstallItemViewModel>(
            o => {
                Model.CancelInstall(o.PluginId);
            }
        );

        private ICommand? _LocalInstallCommand;
        public ICommand LocalInstallCommand => this._LocalInstallCommand ??= new DelegateCommand(
            () => {
                var parameter = new FileSystemSelectDialogRequestParameter() {
                    FileSystemDialogMode = FileSystemDialogMode.FileOpen,
                };
                parameter.Filter.Add(new Core.Models.DialogFilterItem(Properties.Resources.String_Setting_Plugins_Install_File, "7z", new[] { "*.7z", "*.zip" }));

                SelectPluginFileRequest.Send<FileSystemSelectDialogRequestResponse>(parameter, async r => {
                    if(r.ResponseIsCancel) {
                        Logger.LogTrace("cancel");
                        return;
                    }
                    var file = new FileInfo(r.ResponseFilePaths[0]);
                    try {
                        await Model.InstallPluginArchiveAsync(file, CancellationToken.None);
                    } catch(Exception ex) {
                        var parameter = new CommonMessageDialogRequestParameter() {
                            Message = ex.ToString(),
                            Buttons = System.Windows.MessageBoxButton.OK,
                            Caption = ex.Message,
                            Icon = System.Windows.MessageBoxImage.Error,
                        };
                        ShowMessageRequest.Send(parameter);
                    }
                });
            }
        );

        private ICommand? _WebInstallCommand;
        public ICommand WebInstallCommand => this._WebInstallCommand ??= new DelegateCommand(
        async () => {
                var parameter = await Model.CreatePluginWebInstallRequestParameterAsync(CancellationToken.None);
                WebInstallRequest.Send(parameter, async r => {
                    var response = (PluginWebInstallRequestResponse)r;
                    if(response.ResponseIsCancel) {
                        Logger.LogTrace("cancel");
                        return;
                    }
                    try {
                        await Model.InstallPluginArchiveAsync(response.ArchiveFile, CancellationToken.None);
                    } catch(Exception ex) {
                        var parameter = new CommonMessageDialogRequestParameter() {
                            Message = ex.ToString(),
                            Buttons = System.Windows.MessageBoxButton.OK,
                            Caption = ex.Message,
                            Icon = System.Windows.MessageBoxImage.Error,
                        };
                        ShowMessageRequest.Send(parameter);
                    }
                });
            }
        );

        #endregion

        #region function

        #endregion

        #region SettingEditorViewModelBase
        public override string Header => Properties.Resources.String_Setting_Plugins_Header;

        public override void Flush()
        { }

        public override void Refresh()
        { }

        protected override void ValidateDomain()
        {
            base.ValidateDomain();

            foreach(var plugin in PluginCollection.ViewModels) {
                plugin.Validate();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    PluginCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
