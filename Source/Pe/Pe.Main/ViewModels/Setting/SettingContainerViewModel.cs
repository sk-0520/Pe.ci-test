using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Main.Models.Applications.Configuration;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class SettingContainerViewModel: ElementViewModelBase<SettingContainerElement>, IViewLifecycleReceiver
    {
        #region variable

        ISettingEditorViewModel _selectedEditor;

        #endregion

        public SettingContainerViewModel(SettingContainerElement model, ApplicationConfiguration applicationConfiguration, IGeneralTheme generalTheme, IUserTracker userTracker, IImageLoader imageLoader, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, userTracker, dispatcherWrapper, loggerFactory)
        {
            //TODO: #634, クッソ重い
#if DEBUG
            var sw = Stopwatch.StartNew();
#endif

            GeneralTheme = generalTheme;

            AllLauncherItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel>(Model.AllLauncherItems) {
                ToViewModel = m => new LauncherItemSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            };
#if DEBUG
            Logger.LogTrace("#634: {0}", sw.Elapsed);
#endif
            AllLauncherGroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel>(Model.AllLauncherGroups) {
                ToViewModel = m => new LauncherGroupSettingEditorViewModel(m, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory),
            };
#if DEBUG
            Logger.LogTrace("#634: {0}", sw.Elapsed);
#endif

            GeneralSettingEditor = new GeneralsSettingEditorViewModel(Model.GeneralsSettingEditor, applicationConfiguration, generalTheme, imageLoader, DispatcherWrapper, LoggerFactory);
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorViewModel(Model.LauncherItemsSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);
            LauncherGroupsSettingEditor = new LauncherGroupsSettingEditorViewModel(Model.LauncherGroupsSettingEditor, AllLauncherItemCollection, AllLauncherGroupCollection, DispatcherWrapper, LoggerFactory);
            LauncherToobarsSettingEditor = new LauncherToobarsSettingEditorViewModel(Model.LauncherToobarsSettingEditor, AllLauncherGroupCollection, generalTheme, DispatcherWrapper, LoggerFactory);
            KeyboardSettingEditor = new KeyboardSettingEditorViewModel(Model.KeyboardSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);
            PluginsSettingEditor = new PluginsSettingEditorViewModel(Model.PluginsSettingEditor, imageLoader, DispatcherWrapper, LoggerFactory);
#if DEBUG
            Logger.LogTrace("#634: {0}", sw.Elapsed);
#endif

            EditorItems = new List<ISettingEditorViewModel>() {
                GeneralSettingEditor,
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
                LauncherToobarsSettingEditor,
                KeyboardSettingEditor,
                PluginsSettingEditor,
            };
            //#if DEBUG
            //            this._selectedEditor = PluginsSettingEditor;
            //#else
            //            this._selectedEditor = GeneralSettingEditor;
            //#endif
            this._selectedEditor = GeneralSettingEditor;
#if DEBUG
            Logger.LogTrace("#634: {0}", sw.Elapsed);
#endif
        }

        #region property

        IGeneralTheme GeneralTheme { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        [IgnoreValidation]
        public ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        [IgnoreValidation]
        public ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> AllLauncherGroupCollection { get; }

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
        public LauncherToobarsSettingEditorViewModel LauncherToobarsSettingEditor { get; }
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
