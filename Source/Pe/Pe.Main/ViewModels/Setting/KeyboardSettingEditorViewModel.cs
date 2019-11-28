using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class KeyboardSettingEditorViewModel : SettingEditorViewModelBase<KeyboardSettingEditorElement>
    {
        #region variable

        bool _isPopupCreateJobMenu;

        #endregion

        public KeyboardSettingEditorViewModel(KeyboardSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ReplaceJobEditorCollection = new ActionModelViewModelObservableCollectionManager<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode>(model.ReplaceJobEditors) {
                ToViewModel = m => new KeyboardReplaceJobSettingEditorViewMode(m, LoggerFactory),
            };
            ReplaceJobEditors = ReplaceJobEditorCollection.GetDefaultView();
        }

        #region property

        ModelViewModelObservableCollectionManagerBase<KeyboardReplaceJobSettingEditorElement, KeyboardReplaceJobSettingEditorViewMode> ReplaceJobEditorCollection { get; }
        public ICollectionView ReplaceJobEditors { get; }

        public bool IsPopupCreateJobMenu
        {
            get => this._isPopupCreateJobMenu;
            set => SetProperty(ref this._isPopupCreateJobMenu, value);
        }

        #endregion

        #region command

        public ICommand CreateNewLauncherItemJobCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
            }
        ));

        #endregion

        #region function
        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_Keyboard;

        #endregion
    }
}
