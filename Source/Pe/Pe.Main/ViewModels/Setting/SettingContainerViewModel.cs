using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
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

        public SettingContainerViewModel(SettingContainerElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            LauncherItemsSettingEditor = new LauncherItemsSettingEditorViewModel(Model.LauncherItemsSettingEditor, LoggerFactory);

            EditorItems = new List<ISettingEditorViewModel>() {
                LauncherItemsSettingEditor,
            };
            this._selectedEditor = EditorItems.First();
        }

        #region property

        public IReadOnlyList<ISettingEditorViewModel> EditorItems { get; }

        public ISettingEditorViewModel SelectedEditor {
            get => this._selectedEditor;
            set
            {
                SetProperty(ref this._selectedEditor, value);
            }
        }

        public LauncherItemsSettingEditorViewModel LauncherItemsSettingEditor { get; }

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
