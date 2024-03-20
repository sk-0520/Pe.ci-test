using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class SettingContainerViewModel: ElementViewModelBase<SettingContainerElement>, IViewLifecycleReceiver
    {
        #region variable

        private ISettingEditorViewModel _selectedEditor;

        #endregion

        public SettingContainerViewModel(SettingContainerElement model, ApplicationConfiguration applicationConfiguration, IGeneralTheme generalTheme, IUserTracker userTracker, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            GeneralTheme = generalTheme;

            AllLauncherItemCollection = new ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel>(Model.AllLauncherItems, new ModelViewModelObservableCollectionOptions<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel>() {
                ToViewModel = m => new LauncherItemSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            });
            AllLauncherGroupCollection = new ModelViewModelObservableCollectionManager<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel>(Model.AllLauncherGroups, new ModelViewModelObservableCollectionOptions<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel>() {
                ToViewModel = m => new LauncherGroupSettingEditorViewModel(m, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory),
            });

            GeneralSettingEditor = new GeneralsSettingEditorViewModel(Model.GeneralsSettingEditor, applicationConfiguration, generalTheme, imageLoader, DispatcherWrapper, LoggerFactory);
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorViewModel(Model.LauncherItemsSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);
            LauncherGroupsSettingEditor = new LauncherGroupsSettingEditorViewModel(Model.LauncherGroupsSettingEditor, AllLauncherItemCollection, AllLauncherGroupCollection, DispatcherWrapper, LoggerFactory);
            LauncherToolbarsSettingEditor = new LauncherToolbarsSettingEditorViewModel(Model.LauncherToolbarsSettingEditor, AllLauncherGroupCollection, generalTheme, DispatcherWrapper, LoggerFactory);
            KeyboardSettingEditor = new KeyboardSettingEditorViewModel(Model.KeyboardSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);
            PluginsSettingEditor = new PluginsSettingEditorViewModel(Model.PluginsSettingEditor, imageLoader, DispatcherWrapper, LoggerFactory);

            EditorItems = new List<ISettingEditorViewModel>() {
                GeneralSettingEditor,
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
                LauncherToolbarsSettingEditor,
                KeyboardSettingEditor,
                PluginsSettingEditor,
            };
            //#if DEBUG
            //            this._selectedEditor = PluginsSettingEditor;
            //#else
            //            this._selectedEditor = GeneralSettingEditor;
            //#endif
            this._selectedEditor = GeneralSettingEditor;
        }

        #region property

        private IGeneralTheme GeneralTheme { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        [IgnoreValidation]
        public ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ModelViewModelObservableCollectionManager<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> AllLauncherGroupCollection { get; }

        [IgnoreValidation]
        public IReadOnlyList<ISettingEditorViewModel> EditorItems { get; }

        [IgnoreValidation]
        public ISettingEditorViewModel SelectedEditor
        {
            get => this._selectedEditor;
            set
            {
                var prev = this._selectedEditor;
                SetProperty(ref this._selectedEditor, value);
                if(this._selectedEditor != null) {
                    this._selectedEditor.Load();
                }
            }
        }

        public GeneralsSettingEditorViewModel GeneralSettingEditor { get; }
        public LauncherItemsSettingEditorViewModel LauncherItemsSettingEditor { get; }
        public LauncherGroupsSettingEditorViewModel LauncherGroupsSettingEditor { get; }
        public LauncherToolbarsSettingEditorViewModel LauncherToolbarsSettingEditor { get; }
        public KeyboardSettingEditorViewModel KeyboardSettingEditor { get; }
        public PluginsSettingEditorViewModel PluginsSettingEditor { get; }

        #endregion

        #region command

        public ICommand SubmitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(Validate()) {
                    foreach(var editor in EditorItems) {
                        editor.Flush();
                    }
                    Model.Save();
                    CloseRequest.Send();
                }
            }
        ));

        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            RaisePropertyChanged(nameof(SelectedEditor));
            SelectedEditor.Load();
        }

        public void ReceiveViewUserClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosing(Window window, CancelEventArgs e)
        { }

        public void ReceiveViewClosed(Window window, bool isUserOperation)
        { }

        #endregion

        #region ElementViewModelBase

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    foreach(var editor in EditorItems) {
                        editor.Dispose();
                    }

                    foreach(var item in AllLauncherGroupCollection.ViewModels) {
                        item.Dispose();
                    }
                    AllLauncherGroupCollection.Dispose();

                    foreach(var item in AllLauncherItemCollection.ViewModels) {
                        item.Dispose();
                    }
                    AllLauncherItemCollection.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
