using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class SettingContainerViewModel : SingleModelViewModelBase<SettingContainerElement>, IViewLifecycleReceiver
    {
        #region variable

        ISettingEditorViewModel _selectedEditor;

        #endregion

        public SettingContainerViewModel(SettingContainerElement model, Configuration configuration, IGeneralTheme generalTheme, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;

            AllLauncherItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel>(Model.AllLauncherItems) {
                ToViewModel = m => new LauncherItemSettingEditorViewModel(m, DispatcherWrapper, LoggerFactory),
            };

            GeneralSettingEditor = new GeneralsSettingEditorViewModel(Model.GeneralsSettingEditor, configuration, generalTheme, DispatcherWrapper, LoggerFactory);
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorViewModel(Model.LauncherItemsSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);
            LauncherGroupsSettingEditor = new LauncherGroupsSettingEditorViewModel(Model.LauncherGroupsSettingEditor, AllLauncherItemCollection, launcherGroupTheme, DispatcherWrapper, LoggerFactory);
            LauncherToobarsSettingEditor = new LauncherToobarsSettingEditorViewModel(Model.LauncherToobarsSettingEditor, generalTheme, DispatcherWrapper, LoggerFactory);
            KeyboardSettingEditor = new KeyboardSettingEditorViewModel(Model.KeyboardSettingEditor, AllLauncherItemCollection, DispatcherWrapper, LoggerFactory);

            EditorItems = new List<ISettingEditorViewModel>() {
                GeneralSettingEditor,
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
                LauncherToobarsSettingEditor,
                KeyboardSettingEditor,
            };
            this._selectedEditor = GeneralSettingEditor;
            this._selectedEditor = LauncherGroupsSettingEditor;
        }

        #region property

        IDispatcherWrapper DispatcherWrapper { get; }

        public RequestSender CloseRequest { get; } = new RequestSender();

        [IgnoreValidation]
        public ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }

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

        public void ReceiveViewUserClosing(CancelEventArgs e)
        { }

        public void ReceiveViewClosing(CancelEventArgs e)
        { }

        public void ReceiveViewClosed()
        { }

        #endregion

    }
}
