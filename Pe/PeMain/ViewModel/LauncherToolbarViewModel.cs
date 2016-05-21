/*
This file is part of Pe.

Pe is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

Pe is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with Pe.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ContentTypeTextNet.Library.PInvoke.Windows;
using ContentTypeTextNet.Library.SharedLibrary.Attribute;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.IF;
using ContentTypeTextNet.Library.SharedLibrary.Logic;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.PeMain.IF;
using ContentTypeTextNet.Pe.PeMain.Logic.Property;
using ContentTypeTextNet.Pe.PeMain.View;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.Data;
using System.ComponentModel;
using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
using System.Diagnostics;
using ContentTypeTextNet.Library.SharedLibrary.IF.WindowsViewExtend;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentTypeTextNet.Library.SharedLibrary.Data;
using ContentTypeTextNet.Pe.PeMain.Define;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using System.Threading;
using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility.UI;

namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    /// <summary>
    /// プロパティが状態持ちすぎててしんどいなぁ。
    /// </summary>
    public class LauncherToolbarViewModel: HasViewSingleModelWrapperViewModelBase<LauncherToolbarDataModel, LauncherToolbarWindow>, IApplicationDesktopToolbarData, IVisualStyleData, IHasAppNonProcess, IWindowAreaCorrectionData, IWindowHitTestData, IHasAppSender, IRefreshFromViewModel, IMenuItem, ILauncherButton
    {
        #region define
        #endregion

        #region static

        static double GetCaptionWidth()
        {
            return 10;
        }

        static Size GetCaptionSize(Orientation orientation, double captionWidth)
        {
            if(orientation == Orientation.Horizontal) {
                return new Size(captionWidth, 0);
            } else {
                return new Size(0, captionWidth);
            }
        }

        static Thickness GetBorderThickness(DockType dockType, Visual visual)
        {
            //var deviceBorderSize = SystemInformation.BorderSize;
            //var logicalBorderSize = UIUtility.ToLogicalPixel(visual, deviceBorderSize);

            //var map = new Dictionary<DockType, Thickness>() {
            //	{ DockType.None, new Thickness(logicalBorderSize.Width, logicalBorderSize.Height, logicalBorderSize.Width, logicalBorderSize.Height) },
            //	{ DockType.Left, new Thickness(0, 0, logicalBorderSize.Width, 0) },
            //	{ DockType.Top, new Thickness(0, 0, 0, logicalBorderSize.Height) },
            //	{ DockType.Right, new Thickness(logicalBorderSize.Width, 0, 0, 0)},
            //	{ DockType.Bottom, new Thickness(0, logicalBorderSize.Height, 0, 0)},
            //};

            var floatThickness = SystemParameters.WindowResizeBorderThickness;
            var barBorder = SystemParameters.BorderWidth;
            var map = new Dictionary<DockType, Thickness>() {
                { DockType.None, new Thickness(floatThickness.Left, 0, floatThickness.Right, 0) },
                { DockType.Left, new Thickness(0, 0, barBorder, 0) },
                { DockType.Top, new Thickness(0, 0, 0, barBorder) },
                { DockType.Right, new Thickness(barBorder, 0, 0, 0)},
                { DockType.Bottom, new Thickness(0, barBorder, 0, 0)},
            };

            return map[dockType];
        }

        static Size GetIconSize(IconScale iconScale)
        {
            return iconScale.ToSize();
        }

        /// <summary>
        /// メニューボタンを表示するか。
        /// </summary>
        /// <param name="hasMenu">表示するか。</param>
        /// <param name="menuWidth">表示する場合のサイズ。</param>
        /// <returns></returns>
        static double GetMenuWidth(bool hasMenu, double menuWidth)
        {
            if(hasMenu) {
                return menuWidth;
            }

            return 0;
        }

        static Thickness GetButtonPadding()
        {
            return new Thickness(3, 2, 3, 2);
        }

        static Thickness GetIconMargin()
        {
            return new Thickness(2);
        }

        static Size GetButtonSize(Size iconSize, double menuWidth, bool showText, double textWidth, Thickness buttonPadding, Thickness iconMargin)
        {
            var mainButtonSize = iconSize;
            return new Size(mainButtonSize.Width + iconMargin.GetHorizon() + buttonPadding.GetHorizon() + menuWidth + (showText ? textWidth : 0), mainButtonSize.Height + iconMargin.GetVertical() + buttonPadding.GetVertical());
        }

        double GetHideWidth(DockType dockType)
        {
            var map = new Dictionary<DockType, double>() {
                { DockType.Left, BorderThickness.Right },
                { DockType.Top, BorderThickness.Bottom },
                { DockType.Right, BorderThickness.Left },
                { DockType.Bottom, BorderThickness.Top },
            };

            return map[dockType];
        }

        static Orientation GetToolbarButtonOrientation(DockType dockType)
        {
            switch(dockType) {
                case DockType.Left:
                case DockType.Right:
                    return Orientation.Vertical;

                case DockType.Top:
                case DockType.Bottom:
                case DockType.None:
                    return Orientation.Horizontal;

                default:
                    throw new NotImplementedException();
            }
        }

        static Size GetMinSize(DockType dockType, Orientation orientation, Thickness borderThickness, Size buttonSize, double captionWidth)
        {
            var captionSize = GetCaptionSize(orientation, captionWidth);

            return new Size(
                captionSize.Width + borderThickness.GetHorizon() + buttonSize.Width,
                captionSize.Height + borderThickness.GetVertical() + buttonSize.Height
            );
        }

        static PlacementMode GetDropDownPlacement(DockType dockeType)
        {
            var map = new Dictionary<DockType, PlacementMode>() {
                { DockType.None, PlacementMode.Bottom },
                { DockType.Left, PlacementMode.Right },
                { DockType.Top, PlacementMode.Bottom },
                { DockType.Right, PlacementMode.Left },
                { DockType.Bottom, PlacementMode.Top },
            };
            return map[dockeType];
        }

        #endregion

        #region variable

        LauncherGroupItemModel _selectedGroup = null;
        CollectionModel<LauncherItemButtonViewModel> _launcherItems = null;
        double _captionWidth;
        Thickness _borderThickness;
        Brush _borderBrush;
        Size _minSize;

        Rect _showLogicalBarArea;
        bool _isHidden;
        bool _nowFullScreen;
        DateTime _prevFullScreenTime;
        bool _prevFullScreenCancel;

        bool _isMenuOpen;

        CollectionModel<LauncherGroupItemViewModel> _groupItems;

        #endregion

        public LauncherToolbarViewModel(LauncherToolbarDataModel model, LauncherToolbarWindow view, ScreenModel screen, IAppNonProcess appNonProcess, IAppSender appSender)
            : base(model, view)
        {
            DockScreen = screen;
            AppNonProcess = appNonProcess;
            AppSender = appSender;

            this._captionWidth = GetCaptionWidth();
            MenuWidth = GetMenuWidth(IsVisibleMenu, 20);
            IconSize = GetIconSize(Model.Toolbar.IconScale);
            ButtonPadding = GetButtonPadding();
            IconMargin = GetIconMargin();
            ButtonSize = GetButtonSize(IconSize, MenuWidth, Model.Toolbar.TextVisible, Model.Toolbar.TextWidth, ButtonPadding, IconMargin);

            //BorderThickness = GetBorderThickness(Model.Toolbar.DockType, View);
            if(HasView) {
                BorderBrush = View.Background;

                var backgroundPropertyDescriptor = DependencyPropertyDescriptor.FromProperty(
                    Window.BackgroundProperty, typeof(Brush)
                );
                if(backgroundPropertyDescriptor != null) {
                    backgroundPropertyDescriptor.AddValueChanged(View, OnBackgroundChanged);
                }

                CalculateWindowStatus(DockType);
            }

            GroupItems = CreateGroupItems();
            SetSelectedGroup(SelectedGroup, GroupItems);
            LauncherItems = GetLauncherItemButtons(SelectedGroup);
        }

        #region property

        public Thickness BorderThickness
        {
            get { return this._borderThickness; }
            set { SetVariableValue(ref this._borderThickness, value); }
        }

        public Brush BorderBrush
        {
            get { return this._borderBrush; }
            set { SetVariableValue(ref this._borderBrush, value); }
        }

        public Visibility ToolbarContentVisibility
        {
            get
            {
                return IsHidden
                    ? Visibility.Collapsed
                    : Visibility.Visible
                ;
            }
        }

        public Dock GripDock
        {
            get
            {
                switch(DockType) {
                    case DockType.Left:
                    case DockType.Right:
                        return Dock.Top;

                    case DockType.Top:
                    case DockType.Bottom:
                    case DockType.None:
                        return Dock.Left;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public HorizontalAlignment ToolbarButtonHorizontalAlignment
        {
            get
            {
                switch(DockType) {
                    case DockType.None:
                    case DockType.Left:
                    case DockType.Right:
                        return HorizontalAlignment.Left;

                    case DockType.Top:
                    case DockType.Bottom:
                        switch(Model.Toolbar.ButtonPosition) {
                            case ToolbarButtonPosition.Near:
                                return HorizontalAlignment.Left;

                            case ToolbarButtonPosition.Center:
                                return HorizontalAlignment.Center;

                            case ToolbarButtonPosition.Far:
                                return HorizontalAlignment.Right;

                            default:
                                throw new NotImplementedException();
                        }

                    default:
                        throw new NotImplementedException();
                }
            }
        }
        public VerticalAlignment ToolbarButtonVerticalAlignment
        {
            get
            {
                switch(DockType) {
                    case DockType.None:
                    case DockType.Top:
                    case DockType.Bottom:
                        return VerticalAlignment.Center;

                    case DockType.Left:
                    case DockType.Right:
                        switch(Model.Toolbar.ButtonPosition) {
                            case ToolbarButtonPosition.Near:
                                return VerticalAlignment.Top;

                            case ToolbarButtonPosition.Center:
                                return VerticalAlignment.Center;

                            case ToolbarButtonPosition.Far:
                                return VerticalAlignment.Bottom;

                            default:
                                throw new NotImplementedException();
                        }

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        Size IconSize { get; set; }
        public double IconWidth
        {
            get { return IconSize.Width; }
            set { SetPropertyValue(IconSize, value, nameof(IconSize.Width)); }
        }
        public double IconHeight
        {
            get { return IconSize.Height; }
            set { SetPropertyValue(IconSize, value, nameof(IconSize.Height)); }
        }
        Size ButtonSize { get; set; }
        public double ButtonWidth
        {
            get { return ButtonSize.Width; }
            set { SetPropertyValue(ButtonSize, value, nameof(ButtonSize.Width)); }
        }
        public double ButtonHeight
        {
            get { return ButtonSize.Height; }
            set { SetPropertyValue(ButtonSize, value, nameof(ButtonSize.Height)); }
        }
        public double MenuWidth { get; set; }
        public bool IsVisibleMenu { get { return Model.Toolbar.IsVisibleMenuButton; } }
        public double ContentWidth { get { return ButtonSize.Width - MenuWidth; } }
        public Thickness ButtonPadding { get; set; }
        public Thickness IconMargin { get; set; }

        public double FirstWidth
        {
            get
            {
                if(IsEnabledCorrection) {
                    return MenuWidth;
                } else {
                    return ContentWidth;
                }
            }
        }
        public double SecondWidth
        {
            get
            {
                if(!IsEnabledCorrection) {
                    return MenuWidth;
                } else {
                    return ContentWidth;
                }
            }
        }

        public double TextWidth
        {
            get { return Model.Toolbar.TextWidth; }
        }

        public bool NowFloatWindow { get { return DockType == DockType.None; } }
        //public bool CanWindowDrag { get { return NowFloatWindow; } }

        public Visibility CaptionVisibility
        {
            get { return NowFloatWindow ? Visibility.Visible : Visibility.Collapsed; }
        }
        public double CaptionWidth
        {
            get
            {
                if(ToolbarButtonOrientation == Orientation.Horizontal) {
                    return this._captionWidth;
                } else {
                    return HasView ? View.Width : ButtonSize.Width;
                }
            }
        }
        public double CaptionHeight
        {
            get
            {
                if(ToolbarButtonOrientation == Orientation.Vertical) {
                    return this._captionWidth;
                } else {
                    return HasView ? View.Height : ButtonSize.Height;
                }
            }
        }

        public ResizeMode ResizeMode
        {
            get
            {
                if(DockType == DockType.None) {
                    return ResizeMode.CanResize;
                } else {
                    return ResizeMode.NoResize;
                }
            }
        }


        public Orientation ToolbarButtonOrientation
        {
            get { return GetToolbarButtonOrientation(DockType); }
        }

        public Size MinSize
        {
            get { return this._minSize; }
            set
            {
                if(this._minSize != value) {
                    this._minSize = value;
                    OnPropertyChanged();
                }
            }
        }

        public CollectionModel<LauncherGroupItemViewModel> GroupItems
        {
            get { return this._groupItems; }
            set { SetVariableValue(ref this._groupItems, value); }
        }

        public LauncherGroupItemModel SelectedGroup
        {
            get
            {
                if(this._selectedGroup == null) {
                    if(Model.Toolbar.DefaultGroupId != Guid.Empty) {
                        Model.GroupItems.TryGetValue(Model.Toolbar.DefaultGroupId, out this._selectedGroup);
                    }
                    if(this._selectedGroup == null) {
                        var group = Model.GroupItems.FirstOrDefault();
                        if(group == null) {
                            group = SettingUtility.CreateLauncherGroup(Model.GroupItems, AppNonProcess);
                        }
                        this._selectedGroup = group;
                    }
                }

                return this._selectedGroup;
            }
            set
            {
                if(SetVariableValue(ref this._selectedGroup, value)) {
                    SetSelectedGroup(this._selectedGroup, GroupItems);

                    var oldLauncherItems = LauncherItems;
                    LauncherItems = GetLauncherItemButtons(SelectedGroup);
                    if(oldLauncherItems != null) {
                        foreach(var oldItem in oldLauncherItems) {
                            oldItem.Dispose();
                        }
                        oldLauncherItems.Dispose();
                        oldLauncherItems = null;
                    }
                    RefreshHiddenItem();

                    AppSender.SendApplicationCommand(ApplicationCommand.MemoryGarbageCollect, this, ApplicationCommandArg.Empty);
                }
            }
        }

        public CollectionModel<LauncherItemButtonViewModel> LauncherItems
        {
            get { return this._launcherItems; }
            set { SetVariableValue(ref this._launcherItems, value); }
        }

        public Visibility TextVisible { get { return Model.Toolbar.TextVisible ? Visibility.Visible : Visibility.Collapsed; } }

        public PlacementMode DropDownPlacement
        {
            get { return GetDropDownPlacement(DockType); }
        }

        #region font

        public FontFamily FontFamily
        {
            get { return FontModelProperty.GetFamilyDefault(Model.Toolbar.Font); }
            //set { FontModelProperty.SetFamily(Model.Toolbar.Font, value, OnPropertyChanged); }
        }

        public bool FontBold
        {
            get { return FontModelProperty.GetBold(Model.Toolbar.Font); }
            //set { FontModelProperty.SetBold(Model.Toolbar.Font, value, OnPropertyChanged); }
        }

        public bool FontItalic
        {
            get { return FontModelProperty.GetItalic(Model.Toolbar.Font); }
            //set { FontModelProperty.SetItalic(Model.Toolbar.Font, value, OnPropertyChanged); }
        }

        public double FontSize
        {
            get { return FontModelProperty.GetSize(Model.Toolbar.Font); }
            //set { FontModelProperty.SetSize(Model.Toolbar.Font, value, OnPropertyChanged); }
        }

        #endregion

        public int PositionContentButton
        {
            get
            {
                if(IsEnabledCorrection) {
                    return 1;
                }

                return 0;
            }
        }

        public int PositionMenuButton
        {
            get
            {
                if(!IsEnabledCorrection) {
                    return 1;
                }

                return 0;
            }
        }

        public bool IsEnabledCorrection
        {
            get
            {
                return Model.Toolbar.MenuPositionCorrection && DockType == DockType.Right;
            }
        }

        public CollectionModel<LauncherItemButtonViewModel> HiddenLauncherItems
        {
            get
            {
                var items = MakeHiddenItem();

                return items;
            }
        }

        public int LauncherMenuCount
        {
            get
            {
                return Math.Max(GroupItems.Count, HiddenLauncherItems.Count);
            }
        }


        #endregion

        #region command

        public ICommand PositionChangeCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var dockType = (DockType)o;
                        View.Docking(dockType, AutoHide);
                    },
                    o => {
                        var dockType = (DockType)o;
                        return dockType != DockType;
                    }
                );

                return result;
            }
        }

        public ICommand SwitchTopMostCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        IsTopmost = !IsTopmost;
                    }
                );

                return result;
            }
        }

        public ICommand SwitchAutoHideCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        AutoHide = !AutoHide;
                    },
                    o => {
                        return IsDocking;
                    }
                );

                return result;
            }
        }

        public ICommand GroupChangeCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var group = (LauncherGroupItemViewModel)o;
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.logGroupChange, group.Model.Name }
                        };
                        AppNonProcess.Logger.Trace(AppNonProcess.Language["log/group/change", map], group);
                        SelectedGroup = group.Model;
                    }
                );

                return result;
            }
        }

        public ICommand ChangeVisibleCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var visible = (bool)o;
                        IsVisible = visible;
                    }
                );

                return result;
            }
        }

        public ICommand DragOverCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;
                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
                            if(filePathList != null && filePathList.Count() == 1) {
                                eventData.EventArgs.Effects = DragDropEffects.Copy;
                            } else {
                                eventData.EventArgs.Effects = DragDropEffects.None;
                            }
                        } else {
                            eventData.EventArgs.Effects = DragDropEffects.None;
                        }

                        eventData.EventArgs.Handled = true;
                    }
                );

                return result;
            }
        }

        public ICommand DragDropCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var eventData = (EventData<DragEventArgs>)o;
                        if(eventData.EventArgs.Data.GetDataPresent(DataFormats.FileDrop)) {
                            var filePathList = eventData.EventArgs.Data.GetData(DataFormats.FileDrop) as string[];
                            if(filePathList != null && filePathList.Count() == 1) {
                                var filePath = filePathList.First();
                                var loadShorcut = true;
                                if(PathUtility.IsShortcut(filePath)) {
                                    var dialogResult = MessageBox.Show(
                                        AppNonProcess.Language["confirm/shortcut/message"],
                                        AppNonProcess.Language["confirm/shortcut/caption"],
                                        MessageBoxButton.YesNoCancel,
                                        MessageBoxImage.Question
                                    );
                                    if(dialogResult == MessageBoxResult.Cancel) {
                                        // やめる
                                        return;
                                    }
                                    loadShorcut = dialogResult == MessageBoxResult.Yes;
                                }
                                var item = LauncherItemUtility.CreateFromFile(filePath, loadShorcut, AppNonProcess);
                                SelectedGroup.LauncherItems.Add(item.Id);
                                Model.LauncherItems.Add(item);
                                var view = HasView ? View : null;
                                AppSender.SendRefreshView(WindowKind.LauncherToolbar, view);
                            }
                        }
                    }
                );

                return result;
            }
        }

        public ICommand RemoveItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        CastUtility.AsAction<LauncherItemButtonViewModel>(o, vm => {
                            Model.LauncherItems.Remove(vm.Model.Id);
                            AppSender.SendRefreshView(WindowKind.LauncherToolbar, View);
                            vm.Dispose();
                        });
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        void CalculateWindowStatus(DockType dockType)
        {
            Debug.Assert(HasView);

            BorderThickness = GetBorderThickness(dockType, View);

            BarSize = new Size(ButtonSize.Width + BorderThickness.GetHorizon(), ButtonSize.Height + BorderThickness.GetVertical());

            if(dockType != DockType.None) {
                HideWidth = GetHideWidth(dockType);
                MinSize = new Size(0, 0);
            } else {
                var orientation = GetToolbarButtonOrientation(dockType);
                MinSize = GetMinSize(dockType, orientation, BorderThickness, ButtonSize, this._captionWidth);
            }

        }

        IEnumerable<LauncherItemModel> GetLauncherItems(LauncherGroupItemModel groupItem)
        {
            if(groupItem.GroupKind == GroupKind.LauncherItems) {
                return groupItem.LauncherItems
                    .Where(i => Model.LauncherItems.Contains(i)) //TODO: Xアイコン表示をどうしよう
                    .Select(i => Model.LauncherItems[i])
                ;
            }

            // 当面はランチャーアイテムのみ
            throw new NotImplementedException();
        }

        CollectionModel<LauncherItemButtonViewModel> GetLauncherItemButtons(LauncherGroupItemModel group)
        {
            var list = GetLauncherItems(group)
                .Select(m => new LauncherItemButtonViewModel(m, DockScreen, Model.LauncherItemSetting, AppNonProcess, AppSender) {
                    IconScale = Model.Toolbar.IconScale,
                })
            ;
            return new CollectionModel<LauncherItemButtonViewModel>(list);
        }

        double CalculateViewWidth(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth)
        {
            var captionSize = GetCaptionSize(orientation, captionWidth);
            return Model.Toolbar.FloatToolbar.WidthButtonCount * ButtonSize.Width + captionSize.Width + borderThickness.GetHorizon();
        }
        int CalculateButtonWidthCount(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth, double viewWidth)
        {
            var captionSize = GetCaptionSize(orientation, captionWidth);
            return (int)((viewWidth - borderThickness.GetHorizon() - captionSize.Width) / ButtonSize.Width);
        }
        double CalculateViewHeight(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth)
        {
            var captionSize = GetCaptionSize(orientation, captionWidth);
            return Model.Toolbar.FloatToolbar.HeightButtonCount * ButtonSize.Height + captionSize.Height + borderThickness.GetVertical();
        }
        int CalculateButtonHeightCount(DockType dockType, Orientation orientation, Thickness borderThickness, double captionWidth, double viewHeight)
        {
            var captionSize = GetCaptionSize(orientation, captionWidth);
            return (int)((viewHeight - borderThickness.GetVertical() - captionSize.Height) / ButtonSize.Height);
        }

        BitmapSource GetAppIcon()
        {
            return AppResource.GetLauncherToolbarMainIcon(Model.Toolbar.IconScale);
        }

        Color GetAppIconColor()
        {
            return AppUtility.GetHotTrackColor(GetAppIcon());
        }

        public void ChangingWindowMode(DockType dockType)
        {
            DockType = dockType;
            var logicalRect = new Rect(
                WindowLeft,
                WindowTop,
                WindowWidth,
                WindowHeight
            );
            var deviceRect = UIUtility.ToDevicePixel(View, logicalRect);
            var podRect = PodStructUtility.Convert(deviceRect);
            //NativeMethods.MoveWindow(View.Handle, podRect.Left, podRect.Top, podRect.Width, podRect.Height, false);
            NativeMethods.SetWindowPos(View.Handle, IntPtr.Zero, podRect.Left, podRect.Top, podRect.Width, podRect.Height, SWP.SWP_NOSENDCHANGING | SWP.SWP_NOREDRAW);
            //View.InvalidateVisual();
            //View.UpdateLayout()
            //DockType = dockType;
            //MinSize = GetMinSize(DockType, Orientation, BorderThickness, ButtonSize);
        }

        CollectionModel<LauncherGroupItemViewModel> CreateGroupItems()
        {
            var items = Model.GroupItems
                .Select((model, index) => new LauncherGroupItemViewModel(model) {
                    RowIndex = index,
                })
            ;
            return new CollectionModel<LauncherGroupItemViewModel>(items);
        }

        static void SetSelectedGroup(LauncherGroupItemModel selectedItemModel, CollectionModel<LauncherGroupItemViewModel> items)
        {
            foreach(var item in items) {
                item.IsChecked = item.Model == selectedItemModel;
            }
        }

        CollectionModel<LauncherItemButtonViewModel> MakeHiddenItem()
        {
            //TODO: epsilon
            if(WindowWidth == 0 || WindowHeight == 0) {
                return new CollectionModel<LauncherItemButtonViewModel>();
            }

            var trayArea = new Size(
                WindowWidth - BorderThickness.GetHorizon(),
                WindowHeight - BorderThickness.GetVertical()
            );

            if(NowFloatWindow) {
                trayArea.Width -= CaptionWidth;
            }

            var isHorizontal = ToolbarButtonOrientation == Orientation.Horizontal;

            var trayAreaLength = isHorizontal
                ? trayArea.Width
                : trayArea.Height
            ;
            var buttonLength = isHorizontal
                ? ButtonWidth
                : ButtonHeight
            ;

            var appButtonCount = 1;
            var showingItemCount = (int)(trayAreaLength / buttonLength) - appButtonCount;
            var hiddenItemCount = LauncherItems.Count - showingItemCount;

            var hiddenItems = LauncherItems
                .Skip(showingItemCount)
                .Select((item, index) => new { Item = item, Index = index })
            ;
            foreach(var pair in hiddenItems) {
                pair.Item.RowIndex = pair.Index;
            }

            return new CollectionModel<LauncherItemButtonViewModel>(hiddenItems.Select(i => i.Item));
        }

        void RefreshHiddenItem()
        {
            var propertyNames = new[] {
                nameof(HiddenLauncherItems),
                nameof(LauncherMenuCount),
            };
            CallOnPropertyChange(propertyNames);
        }

        #endregion

        #region IHavingAppSender

        public IAppSender AppSender { get; private set; }

        #endregion

        #region IHavingAppNonPorocess

        public IAppNonProcess AppNonProcess { get; private set; }

        #endregion

        #region ITopMost

        public bool IsTopmost
        {
            get
            {
                if(NowFullScreen) {
                    if(HasView) {
                        WindowsUtility.MoveZoderBttom(View.Handle);
                    }
                    return false;
                } else {
                    if(IsDocking && AutoHide && IsHidden) {
                        return true;
                    }
                    //if(DockType != DockType.None) {
                    //	return false;
                    //}
                    return TopMostProperty.GetTopMost(Model.Toolbar);
                }
            }
            set { TopMostProperty.SetTopMost(Model.Toolbar, value, OnPropertyChanged); }
        }

        #endregion

        #region IVisible

        public Visibility Visibility
        {
            get { return VisibleVisibilityProperty.GetVisibility(Model.Toolbar); }
            set { VisibleVisibilityProperty.SetVisibility(Model.Toolbar, value, OnPropertyChanged); }
        }

        public bool IsVisible
        {
            get { return VisibleVisibilityProperty.GetVisible(Model.Toolbar); }
            set
            {
                if(VisibleVisibilityProperty.SetVisible(Model.Toolbar, value, OnPropertyChanged)) {
                    CallOnPropertyChangeDisplayItem();
                }
            }
        }

        #endregion

        #region window

        public double WindowLeft
        {
            get
            {
                if(DockType == DockType.None) {
                    return Model.Toolbar.FloatToolbar.Left;
                } else {
                    return IsHidden
                        ? HideLogicalBarArea.Left
                        : ShowLogicalBarArea.Left;
                }
            }
            set
            {
                //TODO: epsilon
                if(DockType == DockType.None && Model.Toolbar.FloatToolbar.Left != value) {
                    Model.Toolbar.FloatToolbar.Left = value;
                    OnPropertyChanged();
                } else if(!IsHidden && ShowLogicalBarArea.X != value) {
                    this._showLogicalBarArea.X = value;
                    OnPropertyChanged();
                }
            }
        }
        public double WindowTop
        {
            get
            {
                if(DockType == DockType.None) {
                    return Model.Toolbar.FloatToolbar.Top;
                } else {
                    return IsHidden
                        ? HideLogicalBarArea.Top
                        : ShowLogicalBarArea.Top;
                }
            }
            set
            {
                //TODO: epsilon
                if(DockType == DockType.None && Model.Toolbar.FloatToolbar.Top != value) {
                    Model.Toolbar.FloatToolbar.Top = value;
                    OnPropertyChanged();
                } else if(!IsHidden && ShowLogicalBarArea.Y != value) {
                    this._showLogicalBarArea.Y = value;
                    OnPropertyChanged();
                }
            }
        }
        public double WindowWidth
        {
            get
            {
                if(DockType == DockType.None) {
                    return CalculateViewWidth(DockType, ToolbarButtonOrientation, BorderThickness, this._captionWidth);
                } else {
                    return IsHidden
                        ? HideLogicalBarArea.Width
                        : ShowLogicalBarArea.Width
                    ;
                }
            }
            set
            {
                //TODO: epsilon
                if(DockType == DockType.None) {
                    Model.Toolbar.FloatToolbar.WidthButtonCount = CalculateButtonWidthCount(DockType, ToolbarButtonOrientation, BorderThickness, this._captionWidth, value);
                    OnPropertyChanged();
                    CallOnPropertyChange(nameof(CaptionHeight));
                    RefreshHiddenItem();
                } else if(!IsHidden && ShowLogicalBarArea.Width != value) {
                    this._showLogicalBarArea.Width = value;
                    OnPropertyChanged();
                    if(ToolbarButtonOrientation == Orientation.Horizontal) {
                        RefreshHiddenItem();
                    }
                }
            }
        }
        public double WindowHeight
        {
            get
            {
                if(DockType == DockType.None) {
                    return CalculateViewHeight(DockType, ToolbarButtonOrientation, BorderThickness, this._captionWidth);
                } else {
                    return IsHidden
                        ? HideLogicalBarArea.Height
                        : ShowLogicalBarArea.Height
                    ;
                }
            }
            set
            {
                //TODO: epsilon
                if(DockType == DockType.None) {
                    Model.Toolbar.FloatToolbar.HeightButtonCount = CalculateButtonHeightCount(DockType, ToolbarButtonOrientation, BorderThickness, this._captionWidth, value);
                    OnPropertyChanged();
                    CallOnPropertyChange(nameof(CaptionWidth));
                } else if(!IsHidden && ShowLogicalBarArea.Height != value) {
                    this._showLogicalBarArea.Height = value;
                    OnPropertyChanged();
                    if(ToolbarButtonOrientation == Orientation.Vertical) {
                        RefreshHiddenItem();
                    }
                }
            }
        }

        #endregion

        #region IApplicationDesktopToolbarData

        public uint CallbackMessage { get; set; }

        /// <summary>
        /// 他ウィンドウがフルスクリーン表示。
        /// </summary>
        public bool NowFullScreen
        {
            get { return this._nowFullScreen; }
            // TODO: 二回取得しちゃってワケわかめ状態
            set
            {
                if(Model.Toolbar.IsTopmost) {
                    var prevFullScreen = this._nowFullScreen;

                    AppNonProcess.Logger.Debug(string.Format("fullscreen: [OLD]:{0} -> [NEW]:{1}", this._nowFullScreen, value));
                    if(SetVariableValue(ref this._nowFullScreen, value)) {
                        var nowTime = DateTime.Now;
                        if(this._nowFullScreen) {
                            AppNonProcess.Logger.Debug("fullscreen: first, cancel-flag on");
                            CallOnPropertyChange(nameof(IsTopmost));
                            this._prevFullScreenTime = DateTime.Now;
                        } else {
                            var nowSpan = nowTime - _prevFullScreenTime;
                            AppNonProcess.Logger.Debug(string.Format("fullscreen: catch cancel, WAIT:{0}, NOW:{1}", Constants.FullScreenIgnoreTime, nowSpan));
                            if(nowSpan < Constants.FullScreenIgnoreTime) {
                                // 二重で発行されたので無視する
                                AppNonProcess.Logger.Debug("fullscreen: ignore cancel");
                                this._nowFullScreen = prevFullScreen;
                                this._prevFullScreenCancel = true;
                            } else {
                                AppNonProcess.Logger.Debug(string.Format("fullscreen: [CHANGE]:{0}, [IsTopmost]:{1}", this._nowFullScreen, IsTopmost));
                                CallOnPropertyChange(nameof(IsTopmost));
                                if(this._nowFullScreen && this._prevFullScreenCancel) {
                                    // 前回フルクリーンが二重発行されてた場合は解除する
                                    this._prevFullScreenCancel = false;
                                    AppNonProcess.Logger.Debug("fullscreen: cancel-flag off, topmost: " + IsTopmost);
                                }

                                this._prevFullScreenTime = nowTime;
                            }
                        }
                    }
                } else {
                    SetVariableValue(ref this._nowFullScreen, value);
                }
            }
        }

        public bool IsDocking { get; set; }
        /// <summary>
        /// ドッキング種別。
        /// </summary>
        public DockType DockType
        {
            get { return Model.Toolbar.DockType; }
            set
            {
                if(Model.Toolbar.DockType != value) {
                    if(HasView) {
                        CalculateWindowStatus(value);
                    }

                    Model.Toolbar.DockType = value;
                    OnPropertyChanged();

                    View.InvalidateArrange();
                    var propertyNames = new[] {
                        nameof(ToolbarButtonOrientation),
                        nameof(CaptionVisibility),
                        nameof(CaptionWidth),
                        nameof(CaptionHeight),
                        nameof(DropDownPlacement),
                        nameof(GripDock),
                        nameof(ToolbarButtonHorizontalAlignment),
                        nameof(ToolbarButtonVerticalAlignment),
                        nameof(FirstWidth),
                        nameof(SecondWidth),
                        nameof(IsEnabledCorrection),
                        nameof(PositionContentButton),
                        nameof(PositionMenuButton),
                        nameof(ResizeMode),
                        nameof(IsTopmost),
                    };
                    CallOnPropertyChange(propertyNames);
                    RefreshHiddenItem();

                    View.UpdateLayout();
                }
            }
        }
        /// <summary>
        /// 自動的に隠す。
        /// </summary>
        public bool AutoHide
        {
            get { return Model.Toolbar.AutoHide; }
            set
            {
                //if (Model.Toolbar.AutoHide != value) {
                //	Model.Toolbar.AutoHide = value;
                //	OnPropertyChanged();
                //	if (HasView) {
                //		View.Docking(DockType, AutoHide);
                //	}
                //}
                if(SetPropertyValue(Model.Toolbar, value)) {
                    if(HasView) {
                        View.Docking(DockType, AutoHide);
                    }
                }
            }
        }
        /// <summary>
        /// 隠れているか。
        /// </summary>
        public bool IsHidden
        {
            get { return this._isHidden; }
            set
            {
                //if (this._isHidden != value) {
                //	this._isHidden = value;
                //	OnPropertyChanged();
                //	OnPropertyChanged("ToolbarContentVisibility");
                //}
                if(SetVariableValue(ref this._isHidden, value)) {
                    CallOnPropertyChange(
                        nameof(ToolbarContentVisibility),
                        nameof(IsTopmost)
                    );
                    if(!this._isHidden) {
                        HasViewUtility.BeginInvoke(this, () => {
                            WindowsUtility.ShowNoActiveForeground(View.Handle);
                        }, DispatcherPriority.SystemIdle);
                    }
                }
            }
        }
        /// <summary>
        /// バーの論理サイズ
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size BarSize { get; set; }
        /// <summary>
        /// 表示中の論理バーサイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect ShowLogicalBarArea { get { return this._showLogicalBarArea; } set { this._showLogicalBarArea = value; } }
        /// <summary>
        /// 隠れた状態のバー論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public double HideWidth { get; set; }
        /// <summary>
        /// 表示中の隠れたバーの論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect HideLogicalBarArea { get; set; }
        /// <summary>
        /// 自動的に隠すまでの時間。
        /// </summary>
        public TimeSpan HideWaitTime
        {
            get { return Model.Toolbar.HideWaitTime; }
            set
            {
                //if (Model.Toolbar.HideWaitTime != value) {
                //	Model.Toolbar.HideWaitTime = value;
                //	OnPropertyChanged();
                //}
                SetPropertyValue(Model.Toolbar, value);
            }
        }
        /// <summary>
        /// 自動的に隠す際のアニメーション時間。
        /// </summary>
        public TimeSpan HideAnimateTime
        {
            get { return Model.Toolbar.HideAnimateTime; }
            set
            {
                //if (Model.Toolbar.HideAnimateTime != value) {
                //	Model.Toolbar.HideAnimateTime = value;
                //	OnPropertyChanged();
                //}
                SetPropertyValue(Model.Toolbar, value);
            }
        }
        /// <summary>
        /// ドッキングに使用するスクリーン。
        /// </summary>
        public ScreenModel DockScreen { get; private set; }

        #endregion

        #region IVisualStyleData

        public bool UsingVisualStyle { get { return true; } }
        public bool EnabledVisualStyle { get; set; }
        public Color VisualPlainColor { get; set; }
        public Color VisualAlphaColor { get; set; }

        #endregion

        #region IWindowAreaCorrectionData

        /// <summary>
        /// ウィンドウサイズの倍数制御を行うか。
        /// </summary>
        public bool UsingMultipleResize { get { return NowFloatWindow && IsVisible; } }
        /// <summary>
        /// ウィンドウサイズの倍数制御に使用する元となる論理サイズ。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Size MultipleSize
        {
            //TODO: 手抜き
            get { return ButtonSize; }
        }
        /// <summary>
        /// タイトルバーとかボーダーを含んだ領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness MultipleThickness
        {
            get
            {
                var captionSize = GetCaptionSize(ToolbarButtonOrientation, this._captionWidth);
                var result = new Thickness(
                    BorderThickness.Left + captionSize.Width,
                    BorderThickness.Top + captionSize.Height,
                    BorderThickness.Right,
                    BorderThickness.Bottom
                );
                return result;
            }
        }

        /// <summary>
        /// 移動制限を行うか。
        /// </summary>
        public bool UsingMoveLimitArea { get { return CaptionVisibility == Visibility.Visible; } }
        /// <summary>
        /// 移動制限に使用する論理領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect MoveLimitArea
        {
            get
            {
                if(HasView) {
                    return UIUtility.ToLogicalPixel(View, DockScreen.DeviceWorkingArea);
                } else {
                    //AppNonProcess.Logger.SafeDebug("device pixel");
                    return DockScreen.DeviceWorkingArea;
                }
            }
        }

        /// <summary>
        /// 最大化・最小化を抑制するか。
        /// </summary>
        public bool UsingMaxMinSuppression { get { return true; } }

        #endregion

        #region IWindowHitTestData

        /// <summary>
        /// ヒットテストを行うか
        /// </summary>
        public bool UsingBorderHitTest { get { return NowFloatWindow; } }
        public bool UsingCaptionHitTest { get { return NowFloatWindow; } }
        /// <summary>
        /// タイトルバーとして認識される領域。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Rect CaptionArea
        {
            get
            {
                var result = new Rect(BorderThickness.Left, BorderThickness.Top, CaptionWidth, CaptionHeight);
                return result;
            }
        }
        /// <summary>
        /// サイズ変更に使用する境界線。
        /// </summary>
        [PixelKind(Px.Logical)]
        public Thickness ResizeThickness { get { return BorderThickness; } }

        #endregion

        #region HasViewSingleModelWrapperViewModelBase

        protected override void InitializeView()
        {
            Debug.Assert(HasView);

            //View.Loaded += View_Loaded;
            View.UserClosing += View_UserClosing;

            base.InitializeView();
        }

        protected override void UninitializeView()
        {
            Debug.Assert(HasView);

            View.UserClosing -= View_UserClosing;
            //View.Loaded -= View_Loaded;

            base.UninitializeView();
        }

        protected override void CallOnPropertyChangeDisplayItem()
        {
            base.CallOnPropertyChangeDisplayItem();
            CallOnPropertyChange(nameof(MenuIcon));
        }

        #endregion

        #region IRefreshFromViewModel

        public void Refresh()
        {
            var nowGroup = this._selectedGroup;
            this._selectedGroup = null;
            SelectedGroup = nowGroup;
        }

        #endregion

        #region IMenuItem

        public string MenuText
        {
            get
            {
                return ScreenUtility.GetScreenName(DockScreen);
            }
        }

        public FrameworkElement MenuIcon
        {
            get
            {
                var canvas = LauncherToolbarUtility.MakeScreenIcon(DockScreen, IconScale.Small);
                return canvas;
            }
        }

        #endregion

        #region ILauncherButton

        public ImageSource ToolbarImage { get { return GetAppIcon(); } }
        public ImageSource MenuImage { get { throw new NotSupportedException(); } }
        public string ToolbarText { get { return DisplayTextUtility.GetDisplayName(SelectedGroup); } }
        public Color ToolbarHotTrack { get { return GetAppIconColor(); } }

        public string ToolTipTitle { get { return ToolbarText; } }

        public string ToolTipMessage { get { throw new NotSupportedException(); } }
        public bool HasToolTipMessage { get { return false; } }
        public ImageSource ToolTipImage { get { throw new NotSupportedException(); } }

        public bool IsMenuOpen
        {
            get { return this._isMenuOpen; }
            set { SetVariableValue(ref this._isMenuOpen, value); }
        }

        #endregion

        void OnBackgroundChanged(object sender, EventArgs e)
        {
            var viewBrush = View.Background as SolidColorBrush;
            if(viewBrush != null) {
                BorderBrush = viewBrush;
            }
        }

        //void View_Loaded(object sender, RoutedEventArgs e)
        //{
        //	var value = WindowsUtility.GetWindowLong(View.Handle, (int)GWL.GWL_STYLE).ToInt32();
        //	var resetValue = value & ~(int)(WS.WS_MAXIMIZEBOX | WS.WS_MINIMIZEBOX);
        //	WindowsUtility.SetWindowLong(View.Handle, (int)GWL.GWL_STYLE, new IntPtr(resetValue));
        //}

        void View_UserClosing(object sender, CancelEventArgs e)
        {
            Debug.Assert(HasView);

            e.Cancel = true;
            IsVisible = false;
        }
    }
}
