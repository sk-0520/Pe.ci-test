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
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupSettingEditorViewModel : SingleModelViewModelBase<LauncherGroupElement>, ILauncherGroupId
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

        ObservableCollection<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> AllLauncherItems { get; }

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
            if(e.Data.TryGet<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(out var selfItem)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherItemWithIconViewModel<CommonLauncherItemViewModel>)listBoxItem.DataContext;
                        if(currentItem != selfItem) {
                            canDrag = true;
                        }
                    }
                }
            }

            if(canDrag) {
                e.Effects = DragDropEffects.Move;
            } else {
                e.Effects = DragDropEffects.None;
            }
            e.Handled = true;
        }

        private void DragLeave(UIElement sender, DragEventArgs e)
        { }

        private void Drop(UIElement sender, DragEventArgs e)
        {
            if(e.Data.TryGet<LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(out var selfItem)) {
                if(e.OriginalSource is DependencyObject dependencyObject) {
                    var listBoxItem = UIUtility.GetVisualClosest<ListBoxItem>(dependencyObject);
                    if(listBoxItem != null) {
                        var currentItem = (LauncherItemWithIconViewModel<CommonLauncherItemViewModel>)listBoxItem.DataContext;
                        if(currentItem != selfItem) {
                            var selfIndex = LauncherItems.IndexOf(selfItem);
                            var currentIndex = LauncherItems.IndexOf(currentItem);

                            // 自分自身より上のアイテムであれば自分自身をさらに上に設定
                            if(currentIndex < selfIndex) {
                                LauncherItems.RemoveAt(selfIndex);
                                LauncherItems.Insert(currentIndex, selfItem);
                            } else {
                                // 自分自身より下のアイテムであれば自分自身をさらに下に設定
                                Debug.Assert(selfIndex < currentIndex);
                                LauncherItems.RemoveAt(selfIndex);
                                LauncherItems.Insert(currentIndex, selfItem); // 自分消えてるからインデックスずれていいかんじになるはず
                            }
                        }
                    }
                }
            }
        }

        private IResultSuccessValue<DragParameter> GetDragParameter(UIElement sender, MouseEventArgs e)
        {
            if(e.Source is ListBox listbox) {
                var scollbar = UIUtility.GetVisualClosest<ScrollBar>((DependencyObject)e.OriginalSource);
                if(scollbar == null && listbox.SelectedItem != null) {
                    SelectedLauncherItem = (LauncherItemWithIconViewModel<CommonLauncherItemViewModel>)listbox.SelectedItem;
                    var data = new DataObject(SelectedLauncherItem.GetType(), SelectedLauncherItem);
                    return ResultSuccessValue.Success(new DragParameter(sender, DragDropEffects.Move, data));
                }
            }

            return ResultSuccessValue.Failure<DragParameter>();
        }

        #endregion


        #endregion

        #region ILauncherGroupId

        public Guid LauncherGroupId => Model.LauncherGroupId;

        #endregion

    }
}
