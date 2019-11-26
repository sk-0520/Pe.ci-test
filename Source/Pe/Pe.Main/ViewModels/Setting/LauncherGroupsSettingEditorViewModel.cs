using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherGroupsSettingEditorViewModel : SettingEditorViewModelBase<LauncherGroupsSettingEditorElement>
    {
        #region variable

        bool _isPopupCreateGroupMenu;
        LauncherGroupSettingEditorViewModel? _selectedGroup;
        LauncherItemWithIconViewModel<CommonLauncherItemViewModel>? _selectedLauncherItem;
        #endregion

        public LauncherGroupsSettingEditorViewModel(LauncherGroupsSettingEditorElement model, ILauncherGroupTheme launcherGroupTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            LauncherGroupTheme = launcherGroupTheme;

            LauncherCollection = new ActionModelViewModelObservableCollectionManager<LauncherElementWithIconElement<CommonLauncherItemElement>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>>(Model.LauncherItems, LoggerFactory) {
                ToViewModel = m => LauncherItemWithIconViewModel.Create(new CommonLauncherItemViewModel(m.Element, LoggerFactory), new LauncherIcon.LauncherIconViewModel(m.Icon, DispatcherWrapper, loggerFactory), LoggerFactory),
            };
            LauncherItems = LauncherCollection.GetCollectionView();

            GroupCollection = new ActionModelViewModelObservableCollectionManager<LauncherGroupElement, LauncherGroupSettingEditorViewModel>(Model.GroupItems, LoggerFactory) {
                ToViewModel = m => new LauncherGroupSettingEditorViewModel(m, LauncherCollection.ViewModels, LauncherGroupTheme, DispatcherWrapper, LoggerFactory)
            };
            GroupItems = GroupCollection.GetCollectionView();

            var groupImageItems = EnumUtility.GetMembers<LauncherGroupImageName>()
                .OrderBy(i => (int)i)
                .Select(i => new ThemeIconViewModel<LauncherGroupImageName>(i, c => LauncherGroupTheme.GetGroupImage(i, c, IconBox.Small, false), LoggerFactory))
            ;
            GroupIconItems = new ObservableCollection<ThemeIconViewModel<LauncherGroupImageName>>(groupImageItems);

            var groupsDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = GroupsCanDragStart,
                DragEnterAction = GroupsDragOrverOrEnter,
                DragOverAction = GroupsDragOrverOrEnter,
                DragLeaveAction = GroupsDragLeave,
                DropAction = GroupsDrop,
                GetDragParameter = GroupsGetDragParameter,
            };
            groupsDragAndDrop.DragStartSize = new Size(groupsDragAndDrop.DragStartSize.Width, 0);
            GroupsDragAndDrop = groupsDragAndDrop;

            var launcherItemsDragAndDrop = new DelegateDragAndDrop(LoggerFactory) {
                CanDragStart = LauncherItemsCanDragStart,
                DragEnterAction = LauncherItemsDragOrverOrEnter,
                DragOverAction = LauncherItemsDragOrverOrEnter,
                DragLeaveAction = LauncherItemsDragLeave,
                DropAction = LauncherItemsDrop,
                GetDragParameter = LauncherItemsGetDragParameter,
            };
            launcherItemsDragAndDrop.DragStartSize = new Size(launcherItemsDragAndDrop.DragStartSize.Width, 0);
            LauncherItemsDragAndDrop = launcherItemsDragAndDrop;
        }

        #region property
        ILauncherGroupTheme LauncherGroupTheme { get; }
        public IDragAndDrop GroupsDragAndDrop { get; }
        public IDragAndDrop LauncherItemsDragAndDrop { get; }

        ModelViewModelObservableCollectionManagerBase<LauncherElementWithIconElement<CommonLauncherItemElement>, LauncherItemWithIconViewModel<CommonLauncherItemViewModel>> LauncherCollection { get; }
        public ICollectionView LauncherItems { get; }

        ModelViewModelObservableCollectionManagerBase<LauncherGroupElement, LauncherGroupSettingEditorViewModel> GroupCollection { get; }
        public ICollectionView GroupItems { get; }


        public ObservableCollection<ThemeIconViewModel<LauncherGroupImageName>> GroupIconItems { get; }

        public bool IsPopupCreateGroupMenu
        {
            get => this._isPopupCreateGroupMenu;
            set => SetProperty(ref this._isPopupCreateGroupMenu, value);
        }

        public LauncherGroupSettingEditorViewModel? SelectedGroup
        {
            get => this._selectedGroup;
            set
            {
                var prev = this._selectedGroup;
                if(prev != null && !prev.IsDisposed) {
                    prev.PropertyChanged -= SelectedGroup_PropertyChanged;
                    if(prev.Validate()) {
                        prev.Save();
                    }
                }
                SetProperty(ref this._selectedGroup, value);
                if(this._selectedGroup != null) {
                    this._selectedGroup.PropertyChanged += SelectedGroup_PropertyChanged;
                }
                ChangeGroupIconsColorFromCurrentGroup();
            }
        }

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

        public ICommand CreateNewNormalGroupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                CreateNewCommand(LauncherGroupKind.Normal);
            }
        ));

        #endregion

        #region function
        private void CreateNewCommand(LauncherGroupKind kind)
        {
            IsPopupCreateGroupMenu = false;
            /*
            var newLauncherGroupId = Model.CreateNewGroup(kind);
            var newItem = ItemCollection.ViewModels.First(i => i.LauncherItemId == newLauncherItemId);
            SelectedItem = newItem;
            ScrollSelectedItemRequest.Send();
            */
        }

        void ChangeGroupIconsColorFromCurrentGroup()
        {
            if(SelectedGroup != null) {
                foreach(var groupIcon in GroupIconItems) {
                    groupIcon.ChangeColor(SelectedGroup.ImageColor);
                }
            }
        }

        #region GroupsDragAndDrop
        private bool GroupsCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void GroupsDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
        }

        private void GroupsDragLeave(UIElement sender, DragEventArgs e)
        { }

        private void GroupsDrop(UIElement sender, DragEventArgs e)
        { }

        private IResultSuccessValue<DragParameter> GroupsGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            return ResultSuccessValue.Failure<DragParameter>();
        }

        #endregion

        #region LauncherItemsDragAndDrop
        private bool LauncherItemsCanDragStart(UIElement sender, MouseEventArgs e)
        {
            return true;
        }

        private void LauncherItemsDragOrverOrEnter(UIElement sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            e.Handled = true;
        }

        private void LauncherItemsDragLeave(UIElement sender, DragEventArgs e)
        { }

        private void LauncherItemsDrop(UIElement sender, DragEventArgs e)
        { }

        private IResultSuccessValue<DragParameter> LauncherItemsGetDragParameter(UIElement sender, MouseEventArgs e)
        {
            var dd = new LauncherItemInLauncherGroupDragAndDrop(DispatcherWrapper, LoggerFactory);
            var parameter = dd.GetDragParameter(true, sender, e, d => {
                SelectedLauncherItem = d;
            });
            return parameter;
        }

        #endregion


        #endregion

        #region SettingEditorViewModelBase

        public override string Header => Properties.Resources.String_Setting_Header_LauncherGroups;

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    if(SelectedGroup != null) {
                        SelectedGroup.PropertyChanged -= SelectedGroup_PropertyChanged;
                    }

                    GroupCollection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        public override void Load()
        {
            SelectedGroup = null;
            SelectedLauncherItem = null;
            base.Load();
        }
        public override void Save()
        {
            SelectedGroup?.Save();
            base.Save();
        }

        #endregion

        private void SelectedGroup_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedGroup.ImageColor)) {
                ChangeGroupIconsColorFromCurrentGroup();
            }
        }

    }
}
