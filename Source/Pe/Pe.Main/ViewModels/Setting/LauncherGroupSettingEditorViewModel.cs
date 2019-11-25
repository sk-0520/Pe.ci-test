using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherGroup;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupElement>, ILauncherGroupId, ISettingEditorViewModel
    {
        #region variable

        string _name;
        Color _imageColor;
        LauncherGroupImageName _imageName;
        long _sequence;

        LauncherItemWithIconViewModel<CommonLauncherItemViewModel>? _selectedLauncherItem;

        #endregion

        public LauncherGroupSettingEditorViewModel(LauncherGroupElement model, ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> allLauncherItems, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, loggerFactory)
        {
            if(!Model.IsInitialized) {
                throw new ArgumentException(nameof(Model.IsInitialized));
            }

            LauncherGroupTheme = launcherGroupTheme;
            DispatcherWrapper = dispatcherWrapper;
            AllLauncherItems = allLauncherItems;

            this._name = Model.Name;
            this._imageColor = Model.ImageColor;
            this._imageName = Model.ImageName;
            Kind = Model.Kind;

            var launcherItems = Model.GetLauncherItemIds()
                .Join(
                    AllLauncherItems,
                    i => i,
                    i => i.LauncherItemId,
                    (id, item) => item
                )
            ;
            LauncherItems = new ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(launcherItems);

            var dragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = CanDragStart,
                DragEnterAction = DragOrverOrEnter,
                DragOverAction = DragOrverOrEnter,
                DragLeaveAction = DragLeave,
                DropAction = Drop,
                GetDragParameter = GetDragParameter,
            };
            dragAndDrop.DragStartSize = new Size(dragAndDrop.DragStartSize.Width, 0);
            DragAndDrop = dragAndDrop;
        }

        #region property

        /// <summary>
        /// 共用しているランチャーアイテム一覧。
        /// <para>親元でアイコンと共通項目構築済みのランチャーアイテム。毎回作るのあれだし。</para>
        /// </summary>
        ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> AllLauncherItems { get; }

        /// <summary>
        /// 所属ランチャーアイテム。
        /// <para>注意: 設定中データ状態はモデル側に送らない。</para>
        /// </summary>
        public ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherItems { get; }

        ILauncherGroupTheme LauncherGroupTheme { get; }
        IDispatcherWrapper DispatcherWrapper { get; }

        public IDragAndDrop DragAndDrop { get; }

        [Required]
        public string Name
        {
            get => this._name;
            set
            {
                SetProperty(ref this._name, value);
                ValidateProperty(value);
            }
        }

        public Color ImageColor
        {
            get => this._imageColor;
            set
            {
                SetProperty(ref this._imageColor, value);
                ReloadIcon();
            }
        }

        public LauncherGroupImageName ImageName
        {
            get => this._imageName;
            set
            {
                SetProperty(ref this._imageName, value);
                ReloadIcon();
            }
        }

        public long Sequence
        {
            get => this._sequence;
            set => SetProperty(ref this._sequence, value);
        }

        public LauncherGroupKind Kind { get; }

        public object GroupIcon => DispatcherWrapper.Get(() => LauncherGroupTheme.GetGroupImage(ImageName, ImageColor, IconBox.Small, false));

        public LauncherItemWithIconViewModel<CommonLauncherItemViewModel>? SelectedLauncherItem
        {
            get => this._selectedLauncherItem;
            set
            {
                SetProperty(ref this._selectedLauncherItem, value);
            }
        }

        #endregion

        #region command
        #endregion

        #region function

        void ReloadIcon()
        {
            RaisePropertyChanged(nameof(GroupIcon));
        }

        #region DragAndDrop

        private bool CanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void DragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            var canDrag = false;
            if(e.Data.TryGet<LauncherItemDragData>(out var dragData)) {
                if(dragData.FromAllItems) {
                    canDrag = true;
                } else {
                    if(e.OriginalSource is DependencyObject dependencyObject) {
                        var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                        if(listBoxItem != null) {
                            var currentItem = (LauncherItemWithIconViewModel<CommonLauncherItemViewModel>)listBoxItem.DataContext;
                            if(currentItem != dragData.Item) {
                                canDrag = true;
                            }
                        }
                    }
                }
            }

            if(canDrag) {
                Debug.Assert(dragData != null);
                if(dragData.FromAllItems) {
                    e.Effects = DragDropEffects.Copy;
                } else {
                    e.Effects = DragDropEffects.Move;
                }
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void DragLeave(UIElement sender, DragEventArgs e)
        { }

        private void Drop(UIElement sender, DragEventArgs e)
        {
            if(e.Data.TryGet<LauncherItemDragData>(out var dragData)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherItemWithIconViewModel<CommonLauncherItemViewModel>)listBoxItem.DataContext;
                        if(dragData.FromAllItems) {
                            // アイテム一覧からD&Dされた
                            var currentIndex = LauncherItems.IndexOf(currentItem);
                            // 複製しておかないと選択状態が死ぬ
                            var baseLauncherItem = AllLauncherItems.First(i => i == dragData.Item);
                            var newLauncherItem = new LauncherItemWithIconViewModel<CommonLauncherItemViewModel>(baseLauncherItem.Item, baseLauncherItem.Icon, LoggerFactory);
                            LauncherItems.Insert(currentIndex, newLauncherItem);
                            SelectedLauncherItem = newLauncherItem;
                            UIUtility.GetVisualClosest<ListBox>(listBoxItem)!.Focus();
                        } else {
                            // 現在アイテム内での並び替え
                            var selfIndex = LauncherItems.IndexOf(dragData.Item);
                            var currentIndex = LauncherItems.IndexOf(currentItem);

                            // 自分自身より上のアイテムであれば自分自身をさらに上に設定
                            if(currentIndex < selfIndex) {
                                LauncherItems.RemoveAt(selfIndex);
                                LauncherItems.Insert(currentIndex, dragData.Item);
                            } else {
                                // 自分自身より下のアイテムであれば自分自身をさらに下に設定
                                Debug.Assert(selfIndex < currentIndex);
                                LauncherItems.RemoveAt(selfIndex);
                                LauncherItems.Insert(currentIndex, dragData.Item); // 自分消えてるからインデックスずれていいかんじになるはず
                            }
                            SelectedLauncherItem = dragData.Item;
                        }
                    } else if(dragData.FromAllItems) {
                        // 一覧から持ってきた際にデータが空っぽだとここ
                        var baseLauncherItem = AllLauncherItems.First(i => i == dragData.Item);
                        var newLauncherItem = new LauncherItemWithIconViewModel<CommonLauncherItemViewModel>(baseLauncherItem.Item, baseLauncherItem.Icon, LoggerFactory);
                        LauncherItems.Add(newLauncherItem);
                        SelectedLauncherItem = newLauncherItem;
                        UIUtility.GetVisualClosest<ListBox>(dependencyObject)!.Focus();
                    }
                }
            }
        }

        private IResultSuccessValue<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherItemInLauncherGroupDragAndDrop(DispatcherWrapper, LoggerFactory);
            var parameter = dd.GetDragParameter(false, sender, e, d => {
                SelectedLauncherItem = d;
            });
            return parameter;
        }

        #endregion


        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId => Model.LauncherGroupId;

        #endregion

        #region ISettingEditorViewModel

        public string Header { get; } = nameof(NotSupportedException);//throw new NotSupportedException();

        public void Load()
        {
            throw new NotSupportedException();
        }

        public void Save()
        {
            var data = new LauncherGroupData() {
                LauncherGroupId = LauncherGroupId,
                Kind = Kind,
                Name = Name,
                ImageName = ImageName,
                ImageColor = ImageColor,
                Sequence = Sequence
            };
            var launcherItemIds = LauncherItems.Select(i => i.LauncherItemId).ToList();
            Model.Save(data, launcherItemIds);
        }

        #endregion


    }
}
