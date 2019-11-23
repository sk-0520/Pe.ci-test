using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemsSettingEditorViewModel : SettingEditorViewModelBase<LauncherItemsSettingEditorElement>
    {
        #region variable

        bool _isPopupCreateItemMenu;

        #endregion

        public LauncherItemsSettingEditorViewModel(LauncherItemsSettingEditorElement model, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            CustomizeEditorCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemCustomizeEditorElement, LauncherItemCustomizeEditorViewModel>(Model.CustomizeEditorItems, LoggerFactory) {
                ToViewModel = m => new LauncherItemCustomizeEditorViewModel(m, LoggerFactory),
            };
            CustomizeEditorItems = CustomizeEditorCollection.GetCollectionView();
        }

        #region property

        ModelViewModelObservableCollectionManagerBase<LauncherItemCustomizeEditorElement, LauncherItemCustomizeEditorViewModel> CustomizeEditorCollection { get; }
        ICollectionView CustomizeEditorItems { get; }

        public bool IsPopupCreateItemMenu
        {
            get => this._isPopupCreateItemMenu;
            set => SetProperty(ref this._isPopupCreateItemMenu, value);
        }

        #endregion

        #region command

        public ICommand CreateNewFileItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            CreateNewItem(LauncherItemKind.File);
        }));
        public ICommand CreateNewStoreAppItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            CreateNewItem(LauncherItemKind.StoreApp);
        }));
        public ICommand CreateNewAddonItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            CreateNewItem(LauncherItemKind.Addon);
        }));

        #endregion

        #region function

        void CreateNewItem(LauncherItemKind kind)
        {
            Model.CreateNewItem(kind);
        }

        #endregion
    }
}
