using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Compatibility.Windows;
using ContentTypeTextNet.Pe.Core.ViewModels;
using ContentTypeTextNet.Pe.Main.Models;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.Models.Element.Note;
using ContentTypeTextNet.Pe.Main.Models.Element.ReleaseNote;
using ContentTypeTextNet.Pe.Main.Models.Element.Widget;
using ContentTypeTextNet.Pe.Main.Models.KeyAction;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Telemetry;
using ContentTypeTextNet.Pe.Main.ViewModels.LauncherToolbar;
using ContentTypeTextNet.Pe.Main.ViewModels.Note;
using ContentTypeTextNet.Pe.Main.ViewModels.Widget;
using ContentTypeTextNet.Pe.PInvoke.Windows;
using Microsoft.Extensions.Logging;
using Prism.Commands;

namespace ContentTypeTextNet.Pe.Main.ViewModels.Manager
{
    internal class ManagerViewModel: ViewModelBase, IBuildStatus
    {
        #region variable

        private bool _isOpenNoteMenu;
        private bool _isOpenSystemMenu;
        private bool _isOpenWidgetsMenu;
        private bool _isOpenContextMenu;
        private bool _isEnabledManager = true;

        #endregion

        public ManagerViewModel(ApplicationManager applicationManager, IKeyGestureGuide keyGestureGuide, IUserTracker userTracker, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            ApplicationManager = applicationManager;
            KeyGestureGuide = keyGestureGuide;
            UserTracker = userTracker;

            LauncherToolbarCollection = ApplicationManager.GetLauncherNotifyCollection();
            LauncherToolbarItems = LauncherToolbarCollection.ViewModels;

            WidgetCollection = ApplicationManager.GetWidgetCollection();
            WidgetItems = WidgetCollection.GetDefaultView();

            NoteCollection = ApplicationManager.GetNoteCollection();
            NoteVisibleItems = NoteCollection.CreateView();
            NoteHiddenItems = NoteCollection.CreateView();
            NoteVisibleItems.Filter = o => ((NoteNotifyAreaViewModel)o).IsVisible;
            NoteHiddenItems.Filter = o => !((NoteNotifyAreaViewModel)o).IsVisible;

            ApplicationManager.StatusManager.StatusChanged += StatusManager_StatusChanged;
            ApplicationManager.NotifyManager.SettingChanged += NotifyManager_SettingChanged;
        }

        #region property

        // 月初だけ windows アイコンを古くする(理由はない)
        public bool ShowPlatformOldVersion => DateTime.Now.Day == 1;

        private ApplicationManager ApplicationManager { get; }
        private IKeyGestureGuide KeyGestureGuide { get; }
        private IUserTracker UserTracker { get; }

        private ModelViewModelObservableCollectionManager<LauncherToolbarElement, LauncherToolbarNotifyAreaViewModel> LauncherToolbarCollection { get; }
        public ReadOnlyObservableCollection<LauncherToolbarNotifyAreaViewModel> LauncherToolbarItems { get; }


        #region ノート

        [SettingChangedTarget]
        public string NoteCreateKeyGesture => KeyGestureGuide.GetNoteKey(Models.Data.KeyActionContentNote.Create);
        [SettingChangedTarget]
        public string NoteZOrderTopKeyGesture => KeyGestureGuide.GetNoteKey(Models.Data.KeyActionContentNote.ZOrderTop);
        [SettingChangedTarget]
        public string NoteZOrderBottomKeyGesture => KeyGestureGuide.GetNoteKey(Models.Data.KeyActionContentNote.ZOrderBottom);

        public bool IsOpenNoteMenu
        {
            get => this._isOpenNoteMenu;
            set
            {
                SetProperty(ref this._isOpenNoteMenu, value);
                if(IsOpenNoteMenu) {
                    NoteVisibleItems.Refresh();
                    NoteHiddenItems.Refresh();
                }
            }
        }

        private ModelViewModelObservableCollectionManager<NoteElement, NoteNotifyAreaViewModel> NoteCollection { get; }
        public ICollectionView NoteVisibleItems { get; }
        public ICollectionView NoteHiddenItems { get; }

        private ModelViewModelObservableCollectionManager<WidgetElement, WidgetNotifyAreaViewModel> WidgetCollection { get; }
        public ICollectionView WidgetItems { get; }

        #endregion

        #region コマンド

        [SettingChangedTarget]
        public string CommandKeyGesture => KeyGestureGuide.GetCommandKey();

        #endregion

        #region システム

        public bool IsOpenSystemMenu
        {
            get => this._isOpenSystemMenu;
            set
            {
                SetProperty(ref this._isOpenSystemMenu, value);
                if(IsOpenSystemMenu) {
                    RaisePropertyChanged(nameof(IsEnabledHook));
                    RaisePropertyChanged(nameof(IsDisabledSystemIdle));
                    RaisePropertyChanged(nameof(IsSupportedExplorer));

                    ProxyIsEnabled = ApplicationManager.GetProxyIsEnabled();
                    RaisePropertyChanged(nameof(ProxyIsEnabled));
                }
            }
        }

        public bool IsEnabledHook => ApplicationManager.IsEnabledHook;
        public bool IsDisabledSystemIdle => ApplicationManager.IsDisabledSystemIdle;
        public bool IsSupportedExplorer => ApplicationManager.IsSupportedExplorer;
        public bool ProxyIsEnabled { get; private set; }

        #endregion

        #region ウィジェット

        public bool HasWidgetsMenu => 0 < WidgetCollection.Count;

        public bool IsOpenWidgetsMenu
        {
            get => this._isOpenWidgetsMenu;
            set
            {
                SetProperty(ref this._isOpenWidgetsMenu, value);
                if(IsOpenWidgetsMenu) {
                    WidgetItems.Refresh();
                }
            }
        }

        #endregion

        public bool IsEnabledManager
        {
            get => this._isEnabledManager;
            set => SetProperty(ref this._isEnabledManager, value);
        }

        public bool IsOpenContextMenu
        {
            get => this._isOpenContextMenu;
            set
            {
                SetProperty(ref this._isOpenContextMenu, value);
                if(IsOpenContextMenu) {
                    RaisePropertyChanged(nameof(ShowPlatformOldVersion));
                    RaisePropertyChanged(nameof(UpdateInfo));
                    RaisePropertyChanged(nameof(ExistsPluginChanges));
                }
                Logger.LogDebug("[#530調査] <IsOpenContextMenu> IsOpenContextMenu = {0}, IsEnabledManager = {1}", IsOpenContextMenu, IsEnabledManager);
            }
        }

        public IReadOnlyNewVersionInfo UpdateInfo => ApplicationManager.ApplicationUpdateInfo;

        /// <summary>
        /// プラグイン変更有無。
        /// </summary>
        public bool ExistsPluginChanges => ApplicationManager.ExistsPluginChanges;

        #endregion

        #region command

        public ICommand CreateNoteCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                NativeMethods.GetCursorPos(out var rawCursorPosition);
                var deviceCursorPosition = PodStructUtility.Convert(rawCursorPosition);
                var currentScreen = Screen.FromDevicePoint(deviceCursorPosition);

                var noteElement = ApplicationManager.CreateNote(currentScreen, Models.Data.NoteStartupPosition.CenterScreen);
                noteElement.StartView();
            }
        ));

        public ICommand CompactAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.CompactAllNotes();
            }
        ));
        public ICommand MoveZOrderTopAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.MoveZOrderAllNotes(true);
             }
         ));

        public ICommand MoveZOrderBottomAllNotesCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.MoveZOrderAllNotes(false);
            }
        ));

        public ICommand ShowCommandViewCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => {
               ApplicationManager.ShowCommandView();
           }
       ));

        public ICommand OpenSettingCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowSettingView();
            }
        ));
        public ICommand OpenStartupCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowStartupView(false);
            }
        ));
        public ICommand OpenHelpCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ShowHelp();
            }
        ));

        public ICommand OpenAboutCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                // めんどいし直接ビュー開くよ
                ApplicationManager.ShowAboutView();
            }
        ));

        public ICommand UpdateCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.CheckNewVersionsAsync(false);
             }
         ));

        public ICommand ToggleHookCommand => GetOrCreateCommand(() => new DelegateCommand(
           () => {
               ApplicationManager.ToggleHook();
           }
        ));
        public ICommand ToggleDisabledSystemIdleCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ToggleDisableSystemIdle();
            }
        ));

        public ICommand ToggleSupportExplorerCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ToggleSupportExplorer();
            }
        ));

        public ICommand ToggleProxyIsEnabled => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ToggleProxyIsEnabled();
            }
        ));

        public ICommand RebootCommand => GetOrCreateCommand(() => new DelegateCommand(
             () => {
                 ApplicationManager.Reboot();
             }
         ));

        public ICommand ExitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.Exit(false);
            }
        ));

        public ICommand NoUpdateExitCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.Exit(true);
            }
        ));

        public ICommand ShowFeedbackViewCommand => GetOrCreateCommand(() => new DelegateCommand(
            () => {
                ApplicationManager.ShowFeedbackView();
            }
        ));


        #endregion

        #region function
        #endregion

        #region IBuildStatus

        public BuildType BuildType => BuildStatus.BuildType;

        public Version Version => BuildStatus.Version;
        public string Revision => BuildStatus.Revision;

        #endregion

        private void StatusManager_StatusChanged(object? sender, StatusChangedEventArgs e)
        {
            if(e.StatusProperty == StatusProperty.CanCallNotifyAreaMenu) {
                IsEnabledManager = (bool)e.NewValue!;
                Logger.LogDebug("[#530調査] <StatusChanged> IsOpenContextMenu = {0}, IsEnabledManager = {1}", IsOpenContextMenu, IsEnabledManager);
            }
        }

        private void NotifyManager_SettingChanged(object? sender, NotifyEventArgs e)
        {
            var members = SettingChangedTargetHelper.GetMembers(GetType());
            foreach(var member in members) {
                Logger.LogTrace("{0}", member);
                RaisePropertyChanged(member.Name);
            }
        }
    }
}
