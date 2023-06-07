using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Bridge.Plugin.Theme;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Element.Setting;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.ViewModels.Font;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Setting
{
    public class LauncherToolbarSettingEditorViewModel: SettingItemViewModelBase<LauncherToolbarSettingEditorElement>
    {
        #region variable

        //bool _isChangeDefaultGroup;
        private bool _showGroupPopupMenu;

        #endregion

        public LauncherToolbarSettingEditorViewModel(LauncherToolbarSettingEditorElement model, ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> allLauncherGroups, Func<bool> isSelectedGetter, IGeneralTheme generalTheme, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(model, dispatcherWrapper, loggerFactory)
        {
            AllLauncherGroups = allLauncherGroups;
            GeneralTheme = generalTheme;
            IsSelectedGetter = isSelectedGetter;
            //Refresh();
        }

        #region property

        private Func<bool> IsSelectedGetter { get; }
        private ModelViewModelObservableCollectionManagerBase<LauncherGroupSettingEditorElement, LauncherGroupSettingEditorViewModel> AllLauncherGroups { get; }
        private IGeneralTheme GeneralTheme { get; }
        public FontViewModel? Font { get; private set; }

        //public bool IsChangeDefaultGroup
        //{
        //    get => this._isChangeDefaultGroup;
        //    set
        //    {
        //        SetProperty(ref this._isChangeDefaultGroup, value);
        //        if(!IsChangeDefaultGroup) {
        //            LauncherGroupId = Guid.Empty;
        //        }
        //    }
        //}

        public bool ShowGroupPopupMenu
        {
            get => this._showGroupPopupMenu;
            set => SetProperty(ref this._showGroupPopupMenu, value);
        }

        public LauncherGroupSettingEditorViewModel? SelectedLauncherGroup => AllLauncherGroups.ViewModels.FirstOrDefault(i => i.LauncherGroupId == LauncherGroupId);

        public LauncherGroupId LauncherGroupId
        {
            get => Model.LauncherGroupId;
            set
            {
                //if(IsSelectedGetter()) {
                //    SetModelValue(value);
                //}
                SetModelValue(value);
            }
        }

        public AppDesktopToolbarPosition ToolbarPosition
        {
            get => Model.ToolbarPosition;
            set => SetModelValue(value);
        }
        public LauncherToolbarIconDirection IconDirection
        {
            get => Model.IconDirection;
            set => SetModelValue(value);
        }

        public IconBox IconBox
        {
            get => Model.IconBox;
            set => SetModelValue(value);
        }

        //public TimeSpan AutoHideTimeout
        //{
        //    get => Model.AutoHideTimeout;
        //    set => SetModelValue(value);
        //}

        public TimeSpan DisplayDelayTime
        {
            get => Model.DisplayDelayTime;
            set => SetModelValue(value);
        }

        public TimeSpan DisplayDelayTimeMinimum => TimeSpan.Zero;
        public TimeSpan DisplayDelayTimeMaximum => TimeSpan.FromSeconds(1);

        public double AutoHideTimeSeconds
        {
            get => Model.AutoHideTime.TotalSeconds;
            set => SetModelValue(TimeSpan.FromSeconds(value), nameof(Model.AutoHideTime));
        }
        public double MaximumAutoHideTimeSeconds => 10;
        public double MinimumAutoHideTimeSeconds => 0.5;

        public int TextWidth
        {
            get => Model.TextWidth;
            set => SetModelValue(value);
        }
        public double MaximumTextWidth => 200;
        public double MinimumTextWidth => 40;

        public bool IsVisible
        {
            get => Model.IsVisible;
            set => SetModelValue(value);
        }

        public bool IsTopmost
        {
            get => Model.IsTopmost;
            set => SetModelValue(value);
        }

        public bool IsAutoHide
        {
            get => Model.IsAutoHide;
            set => SetModelValue(value);
        }

        public bool IsIconOnly
        {
            get => Model.IsIconOnly;
            set => SetModelValue(value);
        }

        public bool EnabledScreen => Model.Screen != null;
        public string ScreenName => Model.ScreenName;
        public string ScreenDisplayName
        {
            get
            {
                if(Model.Screen == null) {
                    return string.Empty;
                }

                return ScreenUtility.GetName(Model.Screen, LoggerFactory);
            }
        }
        public string ScreenDeviceName => Model.Screen?.DeviceName ?? string.Empty;

        #endregion

        #region function

        //public void Refresh()
        //{
        //    //RaisePropertyChanged(nameof(IsChangeDefaultGroup));
        //    //RaisePropertyChanged(nameof(LauncherGroupId));

        //    if(LauncherGroupId != Guid.Empty && 0 < AllLauncherGroups.Count) {
        //        if(AllLauncherGroups.ViewModels.Any(i => i.LauncherGroupId == LauncherGroupId)) {
        //            RaisePropertyChanged(nameof(LauncherGroupId));
        //            IsChangeDefaultGroup = true;
        //        } else {
        //            LauncherGroupId = Guid.Empty;
        //            IsChangeDefaultGroup = false;
        //        }
        //    } else {
        //        LauncherGroupId = Guid.Empty;
        //        IsChangeDefaultGroup = false;
        //    }

        //}
        public void Refresh()
        {
            var selectedItem = SelectedLauncherGroup;
            RaisePropertyChanged(nameof(SelectedLauncherGroup));
            if(selectedItem == null) {
                LauncherGroupId = LauncherGroupId.Empty;
            }
        }

        #endregion

        #region command

        public ICommand SelectDefaultLauncherGroupCommand => GetOrCreateCommand(() => new DelegateCommand<LauncherGroupSettingEditorViewModel?>(
            o => {
                LauncherGroupId = o?.LauncherGroupId ?? LauncherGroupId.Empty;
                ShowGroupPopupMenu = false;
                Refresh();
            }
        ));

        #endregion

        #region SingleModelViewModelBase

        protected override void BuildChildren()
        {
            base.BuildChildren();

            Font = new FontViewModel(Model.Font!, DispatcherWrapper, LoggerFactory);
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                if(disposing) {
                    Font?.Dispose();
                    Font = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
