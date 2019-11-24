using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class SettingContainerViewModel : SingleModelViewModelBase<SettingContainerElement>, IViewLifecycleReceiver
    {
        #region variable

        ISettingEditorViewModel _selectedEditor;

        #endregion

        public SettingContainerViewModel(SettingContainerElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            DispatcherWrapper = dispatcherWrapper;

            LauncherItemsSettingEditor = new LauncherItemsSettingEditorViewModel(Model.LauncherItemsSettingEditor, DispatcherWrapper, LoggerFactory);
            LauncherGroupsSettingEditor = new LauncherGroupsSettingEditorViewModel(Model.LauncherGroupsSettingEditor, DispatcherWrapper, LoggerFactory);
            EditorItems = new List<ISettingEditorViewModel>() {
                LauncherItemsSettingEditor,
                LauncherGroupsSettingEditor,
            };
            //this._selectedEditor = EditorItems.First();
            this._selectedEditor = LauncherGroupsSettingEditor;
        }

        #region property

        IDispatcherWrapper DispatcherWrapper { get; }

        public IReadOnlyList<ISettingEditorViewModel> EditorItems { get; }

        public ISettingEditorViewModel SelectedEditor
        {
            get => this._selectedEditor;
            set
            {
                var prev = this._selectedEditor;
                if(prev != null) {
                    prev.Save();
                }
                SetProperty(ref this._selectedEditor, value);
                if(this._selectedEditor != null) {
                    this._selectedEditor.Load();
                }
            }
        }

        public LauncherItemsSettingEditorViewModel LauncherItemsSettingEditor { get; }
        public LauncherGroupsSettingEditorViewModel LauncherGroupsSettingEditor { get; }
        #endregion

        #region command
        #endregion

        #region function
        #endregion

        #region IViewLifecycleReceiver

        public void ReceiveViewInitialized(Window window)
        { }

        public void ReceiveViewLoaded(Window window)
        {
            SelectedEditor.Load();
        }

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
