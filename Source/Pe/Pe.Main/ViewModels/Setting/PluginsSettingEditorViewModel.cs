using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class PluginsSettingEditorViewModel: SettingEditorViewModelBase<PluginsSettingEditorElement>
    {
        #region variable

        PluginSettingEditorViewModel? _selectedPlugin;

        #endregion

        public PluginsSettingEditorViewModel(PluginsSettingEditorElement model, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ImageLoader = imageLoader;
            PluginCollection = new ActionModelViewModelObservableCollectionManager<PluginSettingEditorElement, PluginSettingEditorViewModel>(Model.PluginItems) {
                ToViewModel = m => new PluginSettingEditorViewModel(m, ImageLoader, DispatcherWrapper, LoggerFactory),
            };
            PluginItems = PluginCollection.GetDefaultView();
        }

        #region property

        public RequestSender SelectPluginFileRequest { get; } = new RequestSender();

        ModelViewModelObservableCollectionManagerBase<PluginSettingEditorElement, PluginSettingEditorViewModel> PluginCollection { get; }
        public ICollectionView PluginItems { get; }

        public PluginSettingEditorViewModel? SelectedPlugin
        {
            get => this._selectedPlugin;
            set => SetProperty(ref this._selectedPlugin, value);
        }

        IImageLoader ImageLoader { get; }

        #endregion

        #region command

        public ICommand ManualInstallCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                var parameter = new FileSystemSelectDialogRequestParameter() {
                    FileSystemDialogMode = FileSystemDialogMode.FileOpen,
                };
                parameter.Filter.Add(new Core.Models.DialogFilterItem("plugin", "7z", new[] { "*.7z", "*.zip" }));

                SelectPluginFileRequest.Send<FileSystemSelectDialogRequestResponse>(parameter, r => {
                    if(r.ResponseIsCancel) {
                        Logger.LogTrace("cancel");
                        return;
                    }
                    //Model.Import
                });
            }
        ));

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
