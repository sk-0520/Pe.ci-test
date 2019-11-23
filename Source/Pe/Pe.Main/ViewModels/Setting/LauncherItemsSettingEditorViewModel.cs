using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherIcon;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherItemCustomize;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemsSettingEditorViewModel : SettingEditorViewModelBase<LauncherItemsSettingEditorElement>
    {
        #region variable

        bool _isPopupCreateItemMenu;
        LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>? _selectedItem;

        #endregion

        public LauncherItemsSettingEditorViewModel(LauncherItemsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            ItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>>(Model.Items, LoggerFactory) {
                ToViewModel = m => LauncherItemWithIconViewModel.Create(new LauncherItemCustomizeEditorViewModel(m.Element, LoggerFactory), new LauncherIcon.LauncherIconViewModel(m.Icon, DispatcherWrapper, LoggerFactory), LoggerFactory),
            };
            Items = ItemCollection.GetCollectionView();
        }

        #region property


        ModelViewModelObservableCollectionManagerBase<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>> ItemCollection { get; }
        public ICollectionView Items { get; }

        public bool IsPopupCreateItemMenu
        {
            get => this._isPopupCreateItemMenu;
            set => SetProperty(ref this._isPopupCreateItemMenu, value);
        }

        public LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>? SelectedItem
        {
            get => this._selectedItem;
            set
            {
                var prev = this._selectedItem;
                SetProperty(ref this._selectedItem, value);
            }
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
