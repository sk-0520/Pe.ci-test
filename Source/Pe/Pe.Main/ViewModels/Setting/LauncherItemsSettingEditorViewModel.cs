using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Standard.Base.Models;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemsSettingEditorViewModel: SettingEditorViewModelBase<LauncherItemsSettingEditorElement>
    {
        #region variable

        private bool _isPopupAddItemMenu;
        private LauncherItemSettingEditorViewModel? _selectedItem;

        private string _nameFilterQuery = string.Empty;

        #endregion

        public LauncherItemsSettingEditorViewModel(LauncherItemsSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItems, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {

            AllLauncherItemCollection = allLauncherItems;
            //AllLauncherItemItems = AllLauncherItemCollection.CreateView();
            AllLauncherItemItems = AllLauncherItemCollection.GetDefaultView();
            AllLauncherItemItems.Filter = FilterAllLauncherItemItems;

            DragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = CanDragStart,
                DragEnterAction = DragOrverOrEnter,
                DragOverAction = DragOrverOrEnter,
                DragLeaveAction = DragLeave,
                DropAction = Drop,
                GetDragParameter = GetDragParameter,
            };

            SimpleRegexFactory = new SimpleRegexFactory(LoggerFactory);
            NameFilterQueryRegex = SimpleRegexFactory.AllMatchRegex;

            LauncherItemAddonItems = Model.Addons.Select(i => new LauncherItemAddonViewModel(i, LoggerFactory)).ToList();
        }

        #region property

        private SimpleRegexFactory SimpleRegexFactory { get; }

        public RequestSender ScrollSelectedItemRequest { get; } = new RequestSender();
        public RequestSender ScrollToTopCustomizeRequest { get; } = new RequestSender();
        public RequestSender ExpandShortcutFileRequest { get; } = new RequestSender();

        public IDragAndDrop DragAndDrop { get; }

        // このViewModelが有効な際に検証が必要なため IgnoreValidation は付与しない
        private ModelViewModelObservableCollectionManagerBase<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
        public ICollectionView AllLauncherItemItems { get; }

        public bool IsPopupAddItemMenu
        {
            get => this._isPopupAddItemMenu;
            set => SetProperty(ref this._isPopupAddItemMenu, value);
        }

        [IgnoreValidation]
        public LauncherItemSettingEditorViewModel? SelectedItem
        {
            get => this._selectedItem;
            set
            {
                var prev = this._selectedItem;
                if(prev != null && !prev.IsDisposed && prev.IsChanged) {
                    if(prev.Validate()) {
                        //prev.Item.Save();
                        //prev.Icon.Reload();
                    }
                }

                if(value != null && value.IsLazyLoad) {
                    value.LazyLoad();
                }
                SetProperty(ref this._selectedItem, value);

                ScrollToTopCustomizeRequest.Send();
            }
        }

        private Regex NameFilterQueryRegex { get; set; }
        public string NameFilterQuery
        {
            get => this._nameFilterQuery;
            set
            {
                SetProperty(ref this._nameFilterQuery, value);
                NameFilterQueryRegex = SimpleRegexFactory.CreateFilterRegex(this._nameFilterQuery);
                AllLauncherItemItems.Refresh();
            }
        }

        public IReadOnlyList<LauncherItemAddonViewModel> LauncherItemAddonItems { get; }

        #endregion

        #region command

        public ICommand AddNewFileItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            AddNewItem(LauncherItemKind.File, PluginId.Empty);
        }));
        public ICommand AddNewStoreAppItemCommand => GetOrCreateCommand(() => new DelegateCommand(() => {
            AddNewItem(LauncherItemKind.StoreApp, PluginId.Empty);
        }));
        public ICommand AddNewAddonItemCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherItemAddonViewModel>(
            o => {
                AddNewItem(LauncherItemKind.Addon, o.PluginId);
            }
        ));

        public ICommand RemoveItemCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                Model.RemoveItem(SelectedItem!.LauncherItemId);
                SelectedItem = null;
            },
            () => SelectedItem != null
        ).ObservesProperty(() => SelectedItem));

        #endregion

        #region function

        private void AddNewItem(LauncherItemKind kind, PluginId pluginId)
        {
            IsPopupAddItemMenu = false;
            var newLauncherItemId = Model.AddNewItem(kind, pluginId);
            var newItem = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == newLauncherItemId);
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
                var newItem = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == launcherItemId);
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

        #region AllLauncherItemItems

        private bool FilterAllLauncherItemItems(object obj)
        {
            if(string.IsNullOrWhiteSpace(NameFilterQuery)) {
                return true;
            }

            var item = (LauncherItemSettingEditorViewModel)obj;
            if(item == SelectedItem) {
                return true;
            }
            return NameFilterQueryRegex.IsMatch(item.Common.Name);
        }

        #endregion

        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_LauncherItems_Header;

        public override void Flush()
        {
            foreach(var vm in AllLauncherItemCollection.ViewModels) {
                vm.SafeFlush();
            }
        }

        public override void Refresh()
        { }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region ILauncherItemId

        #endregion

    }
}
