using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using ContentTypeTextNet.Pe.Library.Base;
using System.Threading.Tasks;
using System.Threading;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherItemsSettingEditorViewModel: SettingEditorViewModelBase<LauncherItemsSettingEditorElement>
    {
        #region variable

        private bool _isPopupAddItemMenu;
        private LauncherItemSettingEditorViewModel? _selectedItem;

        private string _nameFilterQuery = string.Empty;

        #endregion

        public LauncherItemsSettingEditorViewModel(LauncherItemsSettingEditorElement model, ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> allLauncherItems, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {

            AllLauncherItemCollection = allLauncherItems;
            //AllLauncherItemItems = AllLauncherItemCollection.CreateView();
            AllLauncherItemItems = AllLauncherItemCollection.GetDefaultView();
            AllLauncherItemItems.Filter = FilterAllLauncherItemItems;

            DragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = CanDragStart,
                DragEnterAction = DragOverOrEnter,
                DragOverAction = DragOverOrEnter,
                DragLeaveAction = DragLeave,
                DropActionAsync = DropAsync,
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
        private ModelViewModelObservableCollectionManager<LauncherItemSettingEditorElement, LauncherItemSettingEditorViewModel> AllLauncherItemCollection { get; }
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

        private ICommand? _AddNewFileItemCommand;
        public ICommand AddNewFileItemCommand => this._AddNewFileItemCommand ??= new DelegateCommand(async () => {
            await AddNewItemAsync(LauncherItemKind.File, LauncherSeparatorKind.None, PluginId.Empty, CancellationToken.None);
        });

        private ICommand? _AddNewStoreAppItemCommand;
        public ICommand AddNewStoreAppItemCommand => this._AddNewStoreAppItemCommand ??= new DelegateCommand(async () => {
            await AddNewItemAsync(LauncherItemKind.StoreApp, LauncherSeparatorKind.None, PluginId.Empty, CancellationToken.None);
        });

        private ICommand? _AddNewAddonItemCommand;
        public ICommand AddNewAddonItemCommand => this._AddNewAddonItemCommand ??= new DelegateCommand<LauncherItemAddonViewModel>(
            async o => {
                await AddNewItemAsync(LauncherItemKind.Addon, LauncherSeparatorKind.None, o.PluginId, CancellationToken.None);
            }
        );

        private ICommand? _AddNewSeparatorItemCommand;
        public ICommand AddNewSeparatorItemCommand => this._AddNewSeparatorItemCommand ??= new DelegateCommand(async () => {
            await AddNewItemAsync(LauncherItemKind.Separator, LauncherSeparatorKind.Line, PluginId.Empty, CancellationToken.None);
        });

        private ICommand? _RemoveItemCommand;
        public ICommand RemoveItemCommand => this._RemoveItemCommand ??= new DelegateCommand(
            () => {
                Model.RemoveItem(SelectedItem!.LauncherItemId);
                SelectedItem = null;
            },
            () => SelectedItem != null
        ).ObservesProperty(() => SelectedItem);

        #endregion

        #region function

        private async Task AddNewItemAsync(LauncherItemKind kind, LauncherSeparatorKind launcherSeparatorKind, PluginId pluginId, CancellationToken cancellationToken)
        {
            IsPopupAddItemMenu = false;
            var newLauncherItemId = await Model.AddNewItemAsync(kind, launcherSeparatorKind, pluginId, cancellationToken);
            var newItem = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == newLauncherItemId);
            SelectedItem = newItem;
            ScrollSelectedItemRequest.Send();
        }

        #region DragAndDrop

        private IResultSuccess<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.GetDragParameter(sender, e);
        }

        private bool CanDragStart(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            return dd.CanDragStart(sender, e);
        }

        private void DragOverOrEnter(UIElement sender, DragEventArgs e)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            dd.DragOverOrEnter(sender, e);
        }

        private async Task DropAsync(UIElement sender, DragEventArgs e, CancellationToken cancellationToken)
        {
            var dd = new LauncherFileItemDragAndDrop(DispatcherWrapper, LoggerFactory);
            await dd.DropAsync(sender, e, s => dd.RegisterDropFile(ExpandShortcutFileRequest, s, async (path, expand) => {
                var launcherItemId = await Model.RegisterFileAsync(path, expand, cancellationToken);
                var newItem = AllLauncherItemCollection.ViewModels.First(i => i.LauncherItemId == launcherItemId);
                SelectedItem = newItem;
                ScrollSelectedItemRequest.Send();
            }), cancellationToken);
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
