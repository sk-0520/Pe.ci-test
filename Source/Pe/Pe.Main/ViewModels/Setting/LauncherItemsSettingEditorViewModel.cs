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

        public LauncherItemsSettingEditorViewModel(LauncherItemsSettingEditorElement model, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            CustomizeEditorCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>>(Model.Items, LoggerFactory) {
                ToViewModel = m => LauncherItemWithIconViewModel.Create(new LauncherItemCustomizeEditorViewModel(m.Element, LoggerFactory), new LauncherIcon.LauncherIconViewModel(m.Icon, DispatcherWrapper, LoggerFactory), LoggerFactory),
            };
            CustomizeEditorItems = CustomizeEditorCollection.GetCollectionView();
        }

        #region property


        ModelViewModelObservableCollectionManagerBase<LauncherItemWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>> CustomizeEditorCollection { get; }
        public ICollectionView CustomizeEditorItems { get; }

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
