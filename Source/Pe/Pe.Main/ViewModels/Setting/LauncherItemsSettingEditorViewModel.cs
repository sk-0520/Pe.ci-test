using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
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
            ItemCollection = new ActionModelViewModelObservableCollectionManager<LauncherElementWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>>(Model.Items, LoggerFactory) {
                ToViewModel = m => LauncherItemWithIconViewModel.Create(new LauncherItemCustomizeEditorViewModel(m.Element, LoggerFactory), new LauncherIcon.LauncherIconViewModel(m.Icon, DispatcherWrapper, LoggerFactory), LoggerFactory),
            };
            Items = ItemCollection.GetCollectionView();

            DragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = CanDragStart,
                DragEnterAction = DragOrverOrEnter,
                DragOverAction = DragOrverOrEnter,
                DragLeaveAction = DragLeave,
                DropAction = Drop,
                GetDragParameter = GetDragParameter,
            };
        }


        #region property

        public RequestSender ScrollSelectedItemRequest { get; } = new RequestSender();
        public RequestSender ScrollToTopCustomizeRequest { get; } = new RequestSender();
        public RequestSender ExpandShortcutFileRequest { get; } = new RequestSender();

        public IDragAndDrop DragAndDrop { get; }

        ModelViewModelObservableCollectionManagerBase<LauncherElementWithIconElement<LauncherItemCustomizeEditorElement>, LauncherItemWithIconViewModel<LauncherItemCustomizeEditorViewModel>> ItemCollection { get; }
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
                if(prev != null && !prev.IsDisposed && prev.Item.IsChanged) {
                    if(prev.Item.Validate()) {
                        prev.Item.Save();
                        prev.Icon.Reload();
                    }
                }

                SetProperty(ref this._selectedItem, value);

                ScrollToTopCustomizeRequest.Send();
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

        public ICommand RemoveItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            var selectedItem = SelectedItem;
            if(selectedItem == null) {
                return;
            }
            Model.RemoveItem(selectedItem.LauncherItemId);
            SelectedItem = null;
        }));

        #endregion

        #region function

        void CreateNewItem(LauncherItemKind kind)
        {
            IsPopupCreateItemMenu = false;
            var newLauncherItemId = Model.CreateNewItem(kind);
            var newItem = ItemCollection.ViewModels.First(i => i.LauncherItemId == newLauncherItemId);
            SelectedItem = newItem;
            ScrollSelectedItemRequest.Send();
        }

        #region DragAndDrop

        private IResultSuccessValue<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.GetDragParameter(sender, e);
        }

        private bool CanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }

        private void DragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragOrverOrEnter(sender, e);
        }

        private void Drop(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.Drop(sender, e, s => dd.RegisterDropFile(ExpandShortcutFileRequest, s, (path, expand) => {
                var launcherItemId = Model.RegisterFile(path, expand);
                var newItem = ItemCollection.ViewModels.First(i => i.LauncherItemId == launcherItemId);
                SelectedItem = newItem;
                ScrollSelectedItemRequest.Send();
            }));
        }

        private void DragLeave(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragLeave(sender, e);
        }


        #endregion

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_LauncherItems;

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    ItemCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
