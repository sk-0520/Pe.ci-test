using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    public class LauncherItemSettingViewModel : SingleModelViewModelBase<LauncherItemSettingElement>
    {
        #region variable

        LauncherItemCustomizeViewModel? _selectedCustomizeItem;
        bool _isOpendAddItemMenu;
        #endregion

        public LauncherItemSettingViewModel(LauncherItemSettingElement model, IDispatcherWapper dispatcherWapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            CustomizeCollection = new ActionModelViewModelObservableCollectionManager<LauncherItemCustomizeElementBase, LauncherItemCustomizeViewModel>(Model.CustomizeItems, LoggerFactory) {
                ToViewModel = m => new LauncherItemCustomizeViewModel(m, dispatcherWapper, LoggerFactory),
            };
            CustomizeItems = CustomizeCollection.GetCollectionView();
        }

        #region property

        public RequestSender ScrollItemRequest { get; } = new RequestSender();

        ModelViewModelObservableCollectionManagerBase<LauncherItemCustomizeElementBase, LauncherItemCustomizeViewModel> CustomizeCollection { get; }
        public ICollectionView CustomizeItems { get; }

        public LauncherItemCustomizeViewModel? SelectedCustomizeItem
        {
            get => this._selectedCustomizeItem;
            set
            {
                SetProperty(ref this._selectedCustomizeItem, value);
            }
        }

        public bool IsOpendAddItemMenu
        {
            get => this._isOpendAddItemMenu;
            set => SetProperty(ref this._isOpendAddItemMenu, value);
        }


        #endregion

        #region command

        public ICommand AddFileItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CreateItem(LauncherItemKind.File);
            }
        ));
        public ICommand AddStoreAppItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CreateItem(LauncherItemKind.StoreApp);
            }
        ));
        public ICommand AddAddonItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CreateItem(LauncherItemKind.Addon);
            }
        ));

        public ICommand RemoveSelectedItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                if(SelectedCustomizeItem == null) {
                    return;
                }
                Model.RemoveItem(SelectedCustomizeItem.LauncherItemId);
                SelectedCustomizeItem = null;
            }
        ));
        #endregion

        #region function

        void CreateItem(LauncherItemKind kind)
        {
            IsOpendAddItemMenu = false;
            var launcherItemId = Model.CreateItem(kind);
            var newItem = CustomizeCollection.ViewModels.First(i => i.LauncherItemId == launcherItemId);
            SelectedCustomizeItem = newItem;
            ScrollItemRequest.Send();
        }

        #endregion
    }
}
