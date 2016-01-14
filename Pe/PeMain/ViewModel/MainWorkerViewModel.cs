/**
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
namespace ContentTypeTextNet.Pe.PeMain.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net.NetworkInformation;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Media.Imaging;
    using System.Windows.Threading;
    using ContentTypeTextNet.Library.PInvoke.Windows;
    using ContentTypeTextNet.Library.SharedLibrary.Attribute;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleForms;
    using ContentTypeTextNet.Library.SharedLibrary.CompatibleWindows.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Define;
    using ContentTypeTextNet.Library.SharedLibrary.IF;
    using ContentTypeTextNet.Library.SharedLibrary.Logic;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Extension;
    using ContentTypeTextNet.Library.SharedLibrary.Logic.Utility;
    using ContentTypeTextNet.Library.SharedLibrary.Model;
    using ContentTypeTextNet.Library.SharedLibrary.ViewModel;
    using ContentTypeTextNet.Pe.Library.PeData.Define;
    using ContentTypeTextNet.Pe.Library.PeData.Item;
    using ContentTypeTextNet.Pe.Library.PeData.Setting;
    using ContentTypeTextNet.Pe.PeMain.Data.Temporary;
    using ContentTypeTextNet.Pe.PeMain.Define;
    using ContentTypeTextNet.Pe.PeMain.IF;
    using ContentTypeTextNet.Pe.PeMain.Logic;
    using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
    using ContentTypeTextNet.Pe.PeMain.View;
    using ContentTypeTextNet.Pe.PeMain.View.Parts.Window;
    using Hardcodet.Wpf.TaskbarNotification;
    using Microsoft.Win32;
    using System.Runtime;
    using ContentTypeTextNet.Pe.PeMain.Data;
    using System.Runtime.InteropServices;
    using ContentTypeTextNet.Pe.PeMain.Data.Model;
    using System.Net;
    using System.Text;
    using System.IO.Compression;

    public sealed class MainWorkerViewModel: ViewModelBase, IAppSender, IClipboardWatcher, IHavingView<TaskbarIcon>, IHavingCommonData
    {
        #region variable

        //bool _isContextMenuOpen;

        DateTime _clipboardPreviousTime = DateTime.MinValue;
        uint _clipboardPreviousSequenceNumber = 0;
        DateTime _toolbarPrevResetTime = DateTime.MinValue;

        #endregion

        public MainWorkerViewModel(VariableConstants variableConstants, ILogger logger)
        {
            CommonData = new CommonData() {
                Logger = logger,
                VariableConstants = variableConstants,
                AppSender = this,
                ClipboardWatcher = this,
            };

            WindowSaveData = new WindowSaveData();

            StreamWindows = new HashSet<LauncherItemStreamWindow>();
            OtherWindows = new HashSet<Window>();

            IndexBodyCaching = new IndexBodyCaching(
                Constants.CacheIndexTemplate,
                Constants.CacheIndexClipboard,
                CommonData.VariableConstants
            );


            var indexTimer = new Dictionary<IndexKind, TimeSpan>() {
                { IndexKind.Clipboard, Constants.SaveIndexClipboardTime },
                { IndexKind.Template, Constants.SaveIndexTemplateTime },
                { IndexKind.Note, Constants.SaveIndexNoteTime },
            };
            IndexSaveTimers = EnumUtility.GetMembers<IndexKind>()
                .ToDictionary(
                    ik => ik,
                    ik => new IndexDispatcherTimer(ik) { Interval = indexTimer[ik] }
                )
            ;
            foreach(var timer in IndexSaveTimers.Values) {
                timer.Tick += IndexTimer_Tick;
            }
        }

        #region property

        bool ResetToolbarRunning { get; set; }
        DateTime PrevResetToolbar { get; set; }

        public bool IsQuickExecute { get; private set; }

        #region IHavingCommonData

        public CommonData CommonData { get; private set; }

        #endregion

        public LanguageManager Language { get { return CommonData.Language; } }

        public bool IsPause { get; set; }

        public bool IsContextMenuOpen
        {
            get
            {
                return IsPause;
                //return this._isContextMenuOpen; 
            }
            set
            {
                //if (SetVariableValue(ref this._isContextMenuOpen, value)) {
                IsPause = value;
                //}
            }
        }

        HashSet<LauncherItemStreamWindow> StreamWindows { get; set; }
        HashSet<Window> OtherWindows { get; set; }

        LoggingWindow LoggingWindow { get; set; }
        public LoggingViewModel Logging { get { return LoggingWindow.ViewModel; } }

        List<LauncherToolbarWindow> LauncherToolbarWindows { get; set; }
        public IEnumerable<LauncherToolbarViewModel> LauncherToolbars { get { return LauncherToolbarWindows.Select(l => l.ViewModel); } }

        List<NoteWindow> NoteWindows { get; set; }
        public IEnumerable<NoteViewModel> NoteShowItems { get { return NoteWindows.Select(w => w.ViewModel); } }
        public IEnumerable<NoteMenuViewModel> NoteHiddenItems { get { return CommonData.NoteIndexSetting.Items.Where(n => !n.IsVisible).Select(n => new NoteMenuViewModel(n, CommonData.NonProcess, CommonData.AppSender)); } }

        MessageWindow MessageWindow { get; set; }

        WindowSaveData WindowSaveData { get; set; }

        public ImageSource ApplicationIcon
        {
            get
            {
                //TODO: 自前で生成したいけどHardcodet.Wpf.TaskbarNotificationの都合上厳しい
                //#if DEBUG
                //				var path = "/Resources/Icon/Tasktray/App-debug.ico";
                //#elif BETA
                //				var path = "/Resources/Icon/Tasktray/App-beta.ico";
                //#else
                //				var path = "/Resources/Icon/Tasktray/App-release.ico";
                //#endif
                var uri = SharedConstants.GetEntryUri(AppResource.ApplicationTasktrayPath);
                return new BitmapImage(uri);
            }
        }

        DispatcherTimer WindowSaveTimer { get; set; }

        public IEnumerable<WindowItemCollectionViewModel> WindowTimerItems
        {
            get { return WindowSaveData.TimerItems.Select(w => new WindowItemCollectionViewModel(w)); }
        }
        public IEnumerable<WindowItemCollectionViewModel> WindowSystemItems
        {
            get { return WindowSaveData.SystemItems.Select(w => new WindowItemCollectionViewModel(w)); }
        }

        public bool IsVisibledShellHideFile { get { return SystemEnvironmentUtility.IsHiddenFileShow(); } }
        public bool IsVisibledShellExtension { get { return SystemEnvironmentUtility.IsExtensionShow(); } }

        TemplateWindow TemplateWindow { get; set; }
        public TemplateViewModel Template { get { return TemplateWindow.ViewModel; } }

        ClipboardWindow ClipboardWindow { get; set; }
        public ClipboardViewModel Clipboard { get { return ClipboardWindow.ViewModel; } }

        IndexBodyCaching IndexBodyCaching { get; set; }

        CommandWindow CommandWindow { get; set; }
        CommandViewModel Command { get { return CommandWindow.ViewModel; } }

        public string CreateNoteHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Note.CreateHotKey, CommonData.Language); } }
        public string CompactNoteItemsHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Note.CompactHotKey, CommonData.Language); } }
        public string HideNoteItemsHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Note.HideHotKey, CommonData.Language); } }
        public string FrontNoteItemsHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Note.ShowFrontHotKey, CommonData.Language); } }

        public string SwitchTemplateWindowHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Template.ToggleHotKey, CommonData.Language); } }
        public string ShowCommandWindowHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Command.ShowHotkey, CommonData.Language); } }

        public string SwitchShellHideFileHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.SystemEnvironment.HideFileHotkey, CommonData.Language); } }
        public string SwitchShellExtensionHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.SystemEnvironment.ExtensionHotkey, CommonData.Language); } }

        public string SwitchClipboardWindowHotKey { get { return LanguageUtility.GetMenuTextFromHotKeyModel(CommonData.MainSetting.Clipboard.ToggleHotKey, CommonData.Language); } }

        public FrameworkElement NoteCompactIcon
        {
            get
            {
                var noteItem = new NoteIndexItemModel() {
                    Id = Guid.Empty,
                    IsCompacted = true,
                    ForeColor = ImageUtility.GetMenuIconColor(false, true),
                    BackColor = ImageUtility.GetMenuIconColor(true, true),
                };
                var result = NoteUtility.MakeMenuIcon(noteItem);

                return result;
            }
        }

        IDictionary<IndexKind, IndexDispatcherTimer> IndexSaveTimers { get; set; }

        #endregion

        #region command

        //public ICommand OpenContextMenuCommand
        //{
        //	get
        //	{
        //		var result = CreateCommand(
        //			o => {
        //				//LanguageUtility.RecursiveSetLanguage((ContextMenu)o, CommonData.Language);
        //			}
        //		);

        //		return result;
        //	}
        //}

        public ICommand OpenSystemEnvironmentMenuCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        CallPropertyChangeSystemEnvironment();
                    }
                );

                return result;
            }
        }

        /// <summary>
        /// 設定ウィンドウ表示。
        /// </summary>
        public ICommand ShowSettingWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var cloneCommonData = new CommonData() {
                            AppSender = CommonData.AppSender,
                            ClipboardWatcher = CommonData.ClipboardWatcher,
                            Language = CommonData.Language,
                            Logger = CommonData.Logger,
                            LauncherIconCaching = CommonData.LauncherIconCaching,
                            VariableConstants = CommonData.VariableConstants,
                            //-----------------------------------------
                            MainSetting = (MainSettingModel)CommonData.MainSetting.DeepClone(),
                            LauncherGroupSetting = (LauncherGroupSettingModel)CommonData.LauncherGroupSetting.DeepClone(),
                            LauncherItemSetting = (LauncherItemSettingModel)CommonData.LauncherItemSetting.DeepClone(),
                            NoteIndexSetting = (NoteIndexSettingModel)CommonData.NoteIndexSetting.DeepClone(),
                            TemplateIndexSetting = (TemplateIndexSettingModel)CommonData.TemplateIndexSetting.DeepClone(),
                            ClipboardIndexSetting = (ClipboardIndexSettingModel)CommonData.ClipboardIndexSetting.DeepClone(),
                        };

                        PausingBasicAction(() => {
                            var window = new SettingWindow();
                            window.SetCommonData(cloneCommonData, null);
                            if(window.ShowDialog().GetValueOrDefault()) {
                                CommonData = window.CommonData;

                                ApplyLanguage();

                                var notifiy = window.ViewModel.SettingNotifiyItem;

                                Debug.Assert(notifiy.StartupRegist.HasValue);
                                var startupPath = Environment.ExpandEnvironmentVariables(Constants.StartupShortcutPath);
                                if(notifiy.StartupRegist.Value) {
                                    AppUtility.MakeAppShortcut(startupPath);
                                } else {
                                    if(File.Exists(startupPath)) {
                                        File.Delete(startupPath);
                                    }
                                }

                                SaveSetting();
                                ResetSetting();

                                CommonData.AppSender.SendApplicationCommand(ApplicationCommand.MemoryGarbageCollect, this, ApplicationCommandArg.Empty);
                            } else {
                                ResetCache(true);
                            }
                        });
                    }
                );

                return result;
            }
        }

        public ICommand SwitchTemplateWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SwitchShowTemplateWindow();
                    }
                );

                return result;
            }
        }

        public ICommand ShowCommandWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ShowCommandWindow();
                    }
                );

                return result;
            }
        }

        /// <summary>
        /// ログウィンドウ切り替え。
        /// </summary>
        public ICommand SwitchLoggingWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        Debug.Assert(Logging != null);
                        Logging.IsVisible = !Logging.IsVisible;
                    }
                );

                return result;
            }
        }

        public ICommand SwitchClipboardWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SwitchShowClipboardWindow();
                    }
                );

                return result;
            }
        }

        public ICommand AboutCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        PausingBasicAction(() => {
                            var window = new AboutWindow();
                            var notifiy = new AboutNotifyData();
                            window.SetCommonData(CommonData, notifiy);
                            window.ShowDialog();
                            if(notifiy.CheckUpdate) {
                                CheckUpdateProcessWait(true);
                            }
                        });
                    }
                );

                return result;
            }
        }

        public ICommand FeedbackCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var model = new HtmlViewerModel() {
                            TitleLanguageKey = LanguageKey.htmlViewerTitleFeedBack,
                            HtmlSource = File.ReadAllText(Path.Combine(Constants.ApplicationHtmlDirectoryPath, Constants.HtmlFeedbackFileName)),
                            CustomStylesheet = File.ReadAllText(Path.Combine(Constants.ApplicationStyleDirectoryPath, Constants.StyleFeedbackFileName)),
                        };
                        model.ReplaceKeys["URI-FEEDBACK"] = Constants.UriFeedback;
                        var map = HtmlViewerUtility.MakeCommonParameter(
                            CommonData.MainSetting.RunningInformation,
                            CommonData.Language,
                            CommonData.VariableConstants,
                            CommonData.ClipboardIndexSetting.Items,
                            CommonData.NoteIndexSetting.Items,
                            CommonData.TemplateIndexSetting.Items
                        );
                        foreach(var pair in map) {
                            model.ReplaceKeys[pair.Key] = pair.Value;
                        }

                        var window = SendCreateWindow(WindowKind.HtmlViewer, model, null);
                        window.Show();
                    }
                );

                return result;
            }
        }

        public ICommand HelpCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var helpPath = Path.Combine(Constants.ApplicationDocumentDirectoryPath, Constants.HelpIndexFileName);
                        var helpCallPath = helpPath + "?lang=" + CommonData.Language.CultureCode;
                        try {
                            ExecuteUtility.OpenFile(helpPath, CommonData.NonProcess);
                        } catch(Exception ex) {
                            CommonData.Logger.Error(ex);
                        }
                    }
                );

                return result;
            }
        }

        /// <summary>
        /// プログラム終了。
        /// </summary>
        public ICommand ExitApplicationCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        ExitApplication(true, true);
                    }
                );

                return result;
            }
        }


        public ICommand CreateNoteItemCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        var devicePoint = MouseUtility.GetDevicePosition();
                        var screen = Screen.FromDevicePoint(devicePoint);
                        // TODO: 論理領域取れてない！
                        var logcalArea = screen.DeviceBounds;

                        var size = Constants.noteDefualtSize;
                        var point = new Point(
                            logcalArea.Width / 2 - size.Width / 2,
                            logcalArea.Height / 2 - size.Height / 2
                        );

                        CreateNoteItem(point, size, true);
                    }
                );

                return result;
            }
        }

        public ICommand CompactNoteItemsCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        CompactNoteItems();
                    }
                );

                return result;
            }
        }

        public ICommand HideNoteItemsCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        HideNoteItems();
                    }
                );

                return result;
            }
        }

        public ICommand FrontNoteItemsCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        MoveFrontNoteItems();
                    }
                );

                return result;
            }
        }

        public ICommand SaveTemporaryWindow
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SaveWindowItemAsync(WindowSaveType.Temporary);
                        //CommandManager.InvalidateRequerySuggested();
                    }
                );

                return result;
            }
        }

        public ICommand LoadTemporaryWindow
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        AppUtility.ChangeWindowFromWindowList(WindowSaveData.TemporaryItem);
                    },
                    o => {
                        return WindowSaveData.TemporaryItem != null;
                    }
                );

                return result;
            }
        }

        public ICommand SwitchShellHideFileCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SwitchShellHideFile();
                    }
                );

                return result;
            }
        }

        public ICommand SwitchShellExtensionCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        SwitchShellExtension();
                    }
                );

                return result;
            }
        }

        public ICommand ShowHomeWindowCommand
        {
            get
            {
                var result = CreateCommand(
                    o => {
                        if(!IsPause) {
                            var dialogResult = ShowHomeDialog();
                            if(dialogResult) {
                                ResetToolbar();
                            }
                        }
                    }
                );

                return result;
            }
        }

        #endregion

        #region function

        public bool ShowHomeDialog()
        {
            var isAppReload = false;
            PausingBasicAction(() => {
                var window = new HomeWindow();
                window.SetCommonData(CommonData, null);
                window.ShowDialog();
                var viewModel = window.ViewModel;
                isAppReload = viewModel.IsAppReload;
                var log = viewModel.LogList;
                viewModel = null;
                window = null;
                CastUtility.AsAction<ILogAppender>(CommonData.Logger, appAppender => {
                    var detail = string.Join(Environment.NewLine, log.Where(l => l != null).Select(l => l.ToString()));
                    CommonData.Logger.Information(CommonData.Language["log/auto-regist/message"], detail);
                });
            });

            return isAppReload;
        }

        void ExitApplication(bool saveSetting, bool gc)
        {
#if DEBUG
            var startupPath = Environment.ExpandEnvironmentVariables(Constants.StartupShortcutPath);
            if(File.Exists(startupPath)) {
                File.Delete(startupPath);
            }
#endif
            StopIndexTimer();
            foreach(var timer in IndexSaveTimers.Values) {
                timer.Tick -= IndexTimer_Tick;
            }

            if(saveSetting) {
                SaveSetting();
            }
            if(gc) {
                IndexItemUtility.GarbageCollectionBody(IndexKind.Note, CommonData.NoteIndexSetting.Items, IndexBodyCaching.NoteItems.Archive, CommonData.NonProcess);
                IndexItemUtility.GarbageCollectionBody(IndexKind.Template, CommonData.TemplateIndexSetting.Items, IndexBodyCaching.TemplateItems.Archive, CommonData.NonProcess);
                IndexItemUtility.GarbageCollectionBody(IndexKind.Clipboard, CommonData.ClipboardIndexSetting.Items, IndexBodyCaching.ClipboardItems.Archive, CommonData.NonProcess);
                GarbageCollectionMainSettingTemporary();
            }
            Application.Current.Shutdown();
        }

        void GarbageCollectionMainSettingTemporary()
        {
            var userSettingDirPath = Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingDirectoryPath);
            AppUtility.GarbageCollectionTemporaryFile(userSettingDirPath, CommonData.Logger);
        }

        public void SetView(TaskbarIcon view)
        {
            Debug.Assert(!HasView);

            View = view;

            View.PreviewTrayContextMenuOpen += View_PreviewTrayContextMenuOpen;
        }

        /// <summary>
        /// 各種設定の読み込み。
        /// </summary>
        /// <returns>本体設定が存在せず、Forms版での設定ファイルが存在する場合は偽、それ以外は真。</returns>
        StartupNotifyData LoadSetting()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                // 各種設定の読込
                var mainSettingPath = Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingMainSettingFilePath);
                CommonData.MainSetting = AppUtility.LoadSetting<MainSettingModel>(mainSettingPath, Constants.fileTypeMainSetting, CommonData.Logger);
                ApplyLanguage();
                CommonData.LauncherItemSetting = AppUtility.LoadSetting<LauncherItemSettingModel>(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingLauncherItemSettingFilePath), Constants.fileTypeLauncherItemSetting, CommonData.Logger);
                CommonData.LauncherGroupSetting = AppUtility.LoadSetting<LauncherGroupSettingModel>(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingLauncherGroupItemSettingFilePath), Constants.fileTypeLauncherGroupSetting, CommonData.Logger);
                // インデックスファイル読み込み
                CommonData.NoteIndexSetting = AppUtility.LoadSetting<NoteIndexSettingModel>(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingNoteIndexFilePath), Constants.fileTypeNoteIndex, CommonData.Logger);
                CommonData.ClipboardIndexSetting = AppUtility.LoadSetting<ClipboardIndexSettingModel>(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingClipboardIndexFilePath), Constants.fileTypeTemplateIndex, CommonData.Logger);
                CommonData.TemplateIndexSetting = AppUtility.LoadSetting<TemplateIndexSettingModel>(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingTemplateIndexFilePath), Constants.fileTypeClipboardIndex, CommonData.Logger);

                var result = new StartupNotifyData();
                result.ExistsSetting = File.Exists(mainSettingPath);

                return result;
            }
        }

        void SaveSetting()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                BackupSetting();

                SaveMainSetting();
                AppUtility.SaveSetting(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingLauncherItemSettingFilePath), CommonData.LauncherItemSetting, Constants.fileTypeLauncherItemSetting, true, CommonData.Logger);
                AppUtility.SaveSetting(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingLauncherGroupItemSettingFilePath), CommonData.LauncherGroupSetting, Constants.fileTypeLauncherGroupSetting, true, CommonData.Logger);

                foreach(var indexKind in EnumUtility.GetMembers<IndexKind>()) {
                    CommonData.AppSender.SendSaveIndex(indexKind, Timing.Instantly);
                }
            }
        }

        void SaveMainSetting()
        {
            AppUtility.SaveSetting(Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserSettingMainSettingFilePath), CommonData.MainSetting, Constants.fileTypeMainSetting, true, CommonData.Logger);
        }

        void RotateSetting(string backupDirectory, string backupPattern, int backupCount)
        {
            // 旧データの削除
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                FileUtility.RotateFiles(backupDirectory, backupPattern, OrderBy.Desc, backupCount, ex => {
                    CommonData.Logger.Error(ex);
                    return true;
                });
            }
        }

        void MakeSettingArchive(string backupDirectory, string settingBaseDirectory)
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                var fileName = PathUtility.AppendExtension(Constants.GetNowTimestampFileName(), "zip");
                var backupFileFilePath = Path.Combine(backupDirectory, fileName);
                FileUtility.MakeFileParentDirectory(backupFileFilePath);

                // zip
                var basePath = Environment.ExpandEnvironmentVariables(settingBaseDirectory);
                var archiveParameters = new[] {
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingMainSettingFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingLauncherItemSettingFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingLauncherGroupItemSettingFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingNoteIndexFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingNoteBodyArchivePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingNoteDirectoryPath, SearchPattern = Constants.IndexJsonFileSearchPattern, SearchOption = SearchOption.TopDirectoryOnly, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingTemplateIndexFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingTemplateBodyArchivePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingTemplateDirectoryPath, SearchPattern = Constants.IndexJsonFileSearchPattern, SearchOption = SearchOption.TopDirectoryOnly, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingClipboardIndexFilePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingClipboardBodyArchivePath, },
                    new ArchiveParameter() { RelativePath = CommonData.VariableConstants.UserSettingClipboardDirectoryPath, CompressionLevel = CompressionLevel.Fastest, SearchPattern = Constants.IndexBinaryFileSearchPattern, SearchOption = SearchOption.TopDirectoryOnly },
                };
                foreach(var ap in archiveParameters) {
                    ap.RelativePath = ArchiveUtility.GetArchiveEntryPath(Environment.ExpandEnvironmentVariables(ap.RelativePath), basePath);
                }
                ArchiveUtility.CreateZipFile(backupFileFilePath, basePath, archiveParameters);
            }
        }

        void BackupSetting()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                var backupDir = Environment.ExpandEnvironmentVariables(CommonData.VariableConstants.UserBackupDirectoryPath);

                RotateSetting(backupDir, Constants.BackupSearchPattern, Constants.BackupSettingCount);
                MakeSettingArchive(backupDir, CommonData.VariableConstants.UserSettingDirectoryPath);
            }
        }

        void ApplyLanguage()
        {
            // 言語ファイル
            CommonData.Language = AppUtility.LoadLanguageFile(Constants.ApplicationLanguageDirectoryPath, CommonData.MainSetting.Language.Name, CommonData.VariableConstants.LanguageCode, CommonData.Logger);
        }

        /// <summary>
        ///プログラム実行を準備。
        /// </summary>
        public StartupNotifyData Initialize()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                var startupNotifyData = LoadSetting();

                if(CommonData.VariableConstants.IsQuickExecute) {
                    if(CommonData.VariableConstants.ForceAccept) {
                        CommonData.MainSetting.RunningInformation.Accept = false;
                        SaveSetting();
                    }
                } else {
                    var ieVersion = SystemEnvironmentUtility.GetInternetExplorerVersion();
                    CommonData.Logger.Information("IE version: " + ieVersion);
                    SystemEnvironmentUtility.SetUsingBrowserVersionForExecutingAssembly(ieVersion);
                    Application.Current.Exit += Current_Exit;

                    // 前回バージョンが色々必要なのでインクリメント前の生情報を保持しておく。
                    var previousVersion = (Version)CommonData.MainSetting.RunningInformation.LastExecuteVersion;
                    ResetCulture(CommonData.NonProcess);
                    startupNotifyData.AcceptRunning = InitializeAccept();
                    if(!startupNotifyData.AcceptRunning) {
                        return startupNotifyData;
                    }
                    if(previousVersion == null) {
                        foreach(var screen in Screen.AllScreens) {
                            var toolbar = new ToolbarItemModel();
                            toolbar.Id = screen.DeviceName;
                            CommonData.Logger.Information(CommonData.Language["log/create/toolbar-setting"], screen);
                            CommonData.MainSetting.Toolbar.Items.Add(toolbar);
                        }
                    }
                    InitializeSetting(previousVersion);
                    InitializeStatus();
                    CallPropertyChangeHotkey();
                    InitializeSystem();

                    CreateMessage();
                    CreateLogger(null);

                    CreateToolbar();
                    CreateNote();
                    CreateTemplate();
                    CreateClipboard();
                    CreateCommandWindow();

                    // #326
                    CommonData.AppSender.SendClipboardChanged();

                    CommonData.AppSender.SendApplicationCommand(ApplicationCommand.MemoryGarbageCollect, this, ApplicationCommandArg.Empty);
                }

                return startupNotifyData;
            }
        }

        /// <summary>
        /// 使用許諾まわり。
        /// </summary>
        bool InitializeAccept()
        {
            if(SettingUtility.CheckAccept(CommonData.MainSetting.RunningInformation, CommonData.NonProcess)) {
                SettingUtility.IncrementRunningInformation(CommonData.MainSetting.RunningInformation);
            } else {
                // 使用許諾表示前に使用しない状態にしておく。
                CommonData.MainSetting.RunningInformation.Accept = false;
                var window = new AcceptWindow();
                window.SetCommonData(CommonData, null);
                window.ShowDialog();
                if(CommonData.MainSetting.RunningInformation.Accept) {
                    CommonData.Logger.Information(CommonData.Language["log/accept/ok"]);
                    SettingUtility.IncrementRunningInformation(CommonData.MainSetting.RunningInformation);
                } else {
                    CommonData.Logger.Information(CommonData.Language["log/accept/ng"]);
                    return false;
                }
            }

            return true;
        }

        void InitializeSetting(Version previousVersion)
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                SettingUtility.InitializeMainSetting(CommonData.MainSetting, previousVersion, CommonData.NonProcess);
                SettingUtility.InitializeLauncherItemSetting(CommonData.LauncherItemSetting, previousVersion, CommonData.NonProcess);
                SettingUtility.InitializeLauncherGroupSetting(CommonData.LauncherGroupSetting, previousVersion, CommonData.NonProcess);
                SettingUtility.InitializeNoteIndexSetting(CommonData.NoteIndexSetting, previousVersion, CommonData.NonProcess);
                SettingUtility.InitializeTemplateIndexSetting(CommonData.TemplateIndexSetting, previousVersion, CommonData.NonProcess);
                SettingUtility.InitializeClipboardIndexSetting(CommonData.ClipboardIndexSetting, previousVersion, CommonData.NonProcess);
            }
        }

        void InitializeStatus()
        {
            WindowSaveData.TimerItems.LimitSize = CommonData.MainSetting.WindowSave.SaveCount;
            WindowSaveData.SystemItems.LimitSize = CommonData.MainSetting.WindowSave.SaveCount;

            if(WindowSaveTimer != null) {
                WindowSaveTimer.Stop();
            }

            WindowSaveTimer = new DispatcherTimer();
            WindowSaveTimer.Tick += Timer_Tick;
            WindowSaveTimer.Interval = CommonData.MainSetting.WindowSave.SaveIntervalTime;
            WindowSaveTimer.Start();
        }

        void InitializeSystem()
        {
            SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
            SystemEvents.DisplaySettingsChanging += SystemEvents_DisplaySettingsChanging;

            // 出力関係を外に出す
            Debug.Listeners.Clear();
            Debug.Listeners.Add(new LogListener(CommonData.Logger, LogKind.Debug));

            //Trace.Listeners.Clear();
            //Trace.Listeners.Add(new LogListener(CommonData.Logger, LogKind.Trace));
        }

        void UninitializeSystem()
        {
            SystemEvents.SessionSwitch -= SystemEvents_SessionSwitch;
            SystemEvents.SessionEnding -= SystemEvents_SessionEnding;
            SystemEvents.DisplaySettingsChanging -= SystemEvents_DisplaySettingsChanging;
        }

        [Obsolete]
        void InitializeStatic()
        {
            //LauncherListDisplayImageConverter.LauncherIconCaching = CommonData.LauncherIconCaching;
            //LauncherListDisplayImageConverter.NonProcess = CommonData.NonProcess;
            //LauncherListDisplayImageConverter.AppSender = CommonData.AppSender;
        }

        /// <summary>
        /// メッセージウィンドウ作成
        /// </summary>
        void CreateMessage()
        {
            MessageWindow = new MessageWindow();
            MessageWindow.SetCommonData(CommonData, null);
            MessageWindow.Show();
        }

        /// <summary>
        /// ログの生成。
        /// </summary>
        void CreateLogger(FixedSizeCollectionModel<LogItemModel> logItems)
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                LoggingWindow = new LoggingWindow();
                LoggingWindow.SetCommonData(CommonData, logItems);

                var appLogger = (AppLogger)CommonData.Logger;
                appLogger.LogCollector = Logging;
                if(logItems == null) {
                    //var appLogger = (AppLogger)CommonData.Logger;
                    //appLogger.LogCollector = Logging;
                    if(appLogger.IsStock) {
                        // 溜まったログをViewにドバー
                        foreach(var logItem in appLogger.StockItems) {
                            appLogger.LogCollector.AddLog(logItem);
                        }
                        appLogger.IsStock = false;
                    }
                }
            }
        }

        FixedSizeCollectionModel<LogItemModel> RemoveLogger()
        {
            var resultItems = Logging.LogItems;

            LoggingWindow.Close();
            LoggingWindow = null;

            return resultItems;
        }

        void ResetLogger()
        {
            var logItems = RemoveLogger();
            CreateLogger(logItems);
        }

        /// <summary>
        /// ツールバーの生成。
        /// </summary>
        void CreateToolbar()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                LauncherToolbarWindows = new List<LauncherToolbarWindow>();

                foreach(var screen in Screen.AllScreens.OrderBy(s => !s.Primary)) {
                    //var toolbar = new LauncherToolbarWindow();
                    //toolbar.SetCommonData(CommonData, screen);
                    CommonData.AppSender.SendCreateWindow(WindowKind.LauncherToolbar, screen, null);
                }
            }

            OnPropertyChanged("LauncherToolbars");
        }

        void RemoveToolbar()
        {
            foreach(var window in LauncherToolbarWindows.Where(w => w.IsLoaded).ToArray()) {
                window.Close();
            }
            LauncherToolbarWindows.Clear();
            LauncherToolbarWindows = null;
        }

        internal void ResetToolbar()
        {
            CommonData.Logger.Debug("toolbar: reset");
            if(ResetToolbarRunning) {
                CommonData.Logger.Debug("toolbar-reset: skip");
                return;
            }
            CommonData.Logger.Debug("toolbar-reset: start");

            ResetToolbarRunning = true;

            Dispatcher.CurrentDispatcher.Invoke(new Action(() => {
                RemoveToolbar();
                CreateToolbar();
                PrevResetToolbar = DateTime.Now;
                ResetToolbarRunning = false;
                CommonData.Logger.Debug("toolbar-reset: end");
            }), DispatcherPriority.SystemIdle);
        }

        void CreateNote()
        {
            using(var timeLogger = CommonData.NonProcess.CreateTimeLogger()) {
                NoteWindows = new List<NoteWindow>();

                foreach(var noteItem in CommonData.NoteIndexSetting.Items.Where(n => n.IsVisible)) {
                    var window = CreateNoteWindow(noteItem, false);
                }
            }
        }

        void RemoveNote()
        {
            foreach(var window in NoteWindows.ToArray()) {
                window.Close();
            }
            NoteWindows.Clear();
            NoteWindows = null;
        }

        void ResetNote()
        {
            RemoveNote();
            CreateNote();
        }

        void CreateTemplate()
        {
            TemplateWindow = new TemplateWindow();
            TemplateWindow.SetCommonData(CommonData, null);
        }

        void RemoveTemplate()
        {
            TemplateWindow.Close();
            TemplateWindow = null;
        }

        void ResetTemplate()
        {
            RemoveTemplate();
            CreateTemplate();
        }

        void CreateClipboard()
        {
            CommonData.ClipboardIndexSetting.Items.LimitSize = CommonData.MainSetting.Clipboard.SaveCount;

            ClipboardWindow = new ClipboardWindow();
            ClipboardWindow.SetCommonData(CommonData, null);
        }

        void RemoveClipboard()
        {
            ClipboardWindow.Close();
            ClipboardWindow = null;
        }

        void ResetClipboard()
        {
            RemoveClipboard();
            CreateClipboard();
        }

        void CreateCommandWindow()
        {
            CommandWindow = new CommandWindow();
            CommandWindow.SetCommonData(CommonData, null);
        }

        void RemoveCommonWindow()
        {
            CommandWindow.Close();
            CommandWindow = null;
        }

        void ResetCommandWindow()
        {
            RemoveCommonWindow();
            CreateCommandWindow();
        }

        /// <summary>
        /// ディスプレイ数に変更があった。
        /// </summary>
        void ChangedScreenCount()
        {
            CommonData.Logger.Information(CommonData.Language["log/screen/change-count"]);
            ResetToolbar();

            CommonData.AppSender.SendApplicationCommand(ApplicationCommand.MemoryGarbageCollect, this, ApplicationCommandArg.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <param name="size"></param>
        /// <param name="appendIndex"></param>
        /// <returns></returns>
        NoteWindow CreateNoteItem([PixelKind(Px.Logical)] Point point, [PixelKind(Px.Logical)] Size size, bool appendIndex)
        {
            var noteItem = new NoteIndexItemModel() {
                WindowLeft = point.X,
                WindowTop = point.Y,
                WindowWidth = size.Width,
                WindowHeight = size.Height,
                IsVisible = true,
                ForeColor = CommonData.MainSetting.Note.ForeColor,
                BackColor = CommonData.MainSetting.Note.BackColor,
            };
            //CommonData.MainSetting.Note.Font.DeepCloneTo(noteItem.Font);
            noteItem.Font = (FontModel)CommonData.MainSetting.Note.Font.DeepClone();
            //TODO: 外部化
            switch(CommonData.MainSetting.Note.NoteTitle) {
                case NoteTitle.Timestamp:
                    {
                        noteItem.Name = CommonData.Language["note/title/timestamp"];
                    }
                    break;

                case NoteTitle.DefaultCaption:
                    {
                        //TODO: ユニークはまぁ優先度下げ下げ
                        var map = new Dictionary<string, string>() {
                            { LanguageKey.noteTitleCount, CommonData.NoteIndexSetting.Items.Count.ToString() },
                        };

                        noteItem.Name = CommonData.Language["note/title/default", map];
                    }
                    break;
                default:
                    throw new NotImplementedException();
            }


            SettingUtility.InitializeNoteIndexItem(noteItem, null, CommonData.NonProcess);
            
            var window = CreateNoteWindow(noteItem, appendIndex);
            WindowsUtility.ShowNoActiveForeground(window.Handle);

            return window;
        }

        NoteWindow CreateNoteWindow(NoteIndexItemModel noteItem, bool appendIndex)
        {
            var window = (NoteWindow)CommonData.AppSender.SendCreateWindow(WindowKind.Note, noteItem, null);
            if(appendIndex) {
                SettingUtility.UpdateUniqueGuid(noteItem, CommonData.NoteIndexSetting.Items);
                CommonData.NoteIndexSetting.Items.Add(noteItem);
            }

            return window;
        }

        void ResetCache(bool isRefresh)
        {
            CommonData.LauncherIconCaching.Clear();

            if(isRefresh) {
                foreach(var viewModel in LauncherToolbars) {
                    viewModel.Refresh();
                }
            }

            //InitializeStatic();
        }

        void StopIndexTimer()
        {
            foreach(var timer in IndexSaveTimers.Values.Where(t => t.IsEnabled)) {
                timer.Stop();
            }
        }

        void ResetSetting()
        {
            ResetCache(false);

            InitializeSetting(Constants.ApplicationVersionNumber);
            InitializeStatus();
            CallPropertyChangeHotkey();

            MessageWindow.SetCommonData(CommonData, null);
            ResetLogger();

            ResetToolbar();
            ResetNote();

            ResetTemplate();
            ResetClipboard();

            ResetCommandWindow();

            // バインドの無理やり付け替え
            if(HasView) {
                var temp = new MainWorkerViewModel(new ContentTypeTextNet.Pe.PeMain.Data.VariableConstants(), new Logger()) {
                    CommonData = this.CommonData,
                    LauncherToolbarWindows = new List<LauncherToolbarWindow>(),
                    NoteWindows = new List<NoteWindow>(),
                    TemplateWindow = this.TemplateWindow,
                    ClipboardWindow = this.ClipboardWindow,
                    LoggingWindow = this.LoggingWindow,
                };
                var stock = View.DataContext;
                View.DataContext = temp;
                View.DataContext = stock;

                temp.CommonData = null;
                temp = null;
                View.ContextMenu.IsOpen = false;
            }
        }

        static void ResetCulture(INonProcess nonProcess)
        {
            try {
                var cultureInfo = new CultureInfo(nonProcess.Language.CultureCode);
                CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
                CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            } catch(CultureNotFoundException ex) {
                nonProcess.Logger.Trace(ex);
            }

        }

        IEnumerable<NoteViewModel> GetEnabledNoteItems()
        {
            return NoteShowItems
                .Where(n => !n.IsLocked)
                .Where(n => !n.IsCompacted)
            ;
        }

        WindowItemCollectionModel SaveWindowItem(WindowSaveType type)
        {
            var windowList = AppUtility.GetSystemWindowList(false);
            var windowCollection = new WindowItemCollectionModel();
            foreach(var window in windowList) {
                windowCollection.Add(window);
            }

            var nameMap = new Dictionary<string, string>() {
                { LanguageKey.windowCollectionCount, windowCollection.Count.ToString() },
            };
            windowCollection.Name = CommonData.Language["window-collection/name", nameMap];

            switch(type) {
                case WindowSaveType.Temporary:
                    WindowSaveData.TemporaryItem = windowCollection;
                    break;

                case WindowSaveType.Timer:
                    WindowSaveData.TimerItems.Add(windowCollection);
                    OnPropertyChanged("WindowTimerItems");
                    break;

                case WindowSaveType.System:
                    WindowSaveData.SystemItems.Add(windowCollection);
                    OnPropertyChanged("WindowSystemItems");
                    break;
            }
            if(type != WindowSaveType.Temporary) {
                var map = new Dictionary<string, string>() {
                    { LanguageKey.logWindowSaveType, LanguageUtility.GetTextFromEnum(type, CommonData.Language) },
                };
                CommonData.Logger.Information(CommonData.Language["log/window/save", map], windowCollection);
            }

            return windowCollection;
        }

        Task<WindowItemCollectionModel> SaveWindowItemAsync(WindowSaveType type)
        {
            return Task.Run(() => SaveWindowItem(type));
        }

        void SwitchShellHideFile()
        {
            SystemEnvironmentUtility.SetHiddenFileShow(!IsVisibledShellHideFile);
            SystemEnvironmentUtility.RefreshShell();
            OnPropertyChanged("IsVisibledShellHideFile");
        }

        void SwitchShellExtension()
        {
            SystemEnvironmentUtility.SetExtensionShow(!IsVisibledShellExtension);
            SystemEnvironmentUtility.RefreshShell();
            OnPropertyChanged("IsVisibledShellExtension");
        }

        void CompactNoteItems()
        {
            foreach(var vm in GetEnabledNoteItems()) {
                vm.IsCompacted = true;
            }
        }

        void HideNoteItems()
        {
            foreach(var window in NoteWindows.Where(n => !n.ViewModel.IsLocked).ToArray()) {
                window.UserClose();
            }
        }

        void MoveFrontNoteItems()
        {
            foreach(var window in NoteWindows) {
                Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                    WindowsUtility.ShowNoActiveForeground(window.Handle);
                }), DispatcherPriority.SystemIdle);
            }
        }

        void SwitchShowClipboardWindow()
        {
            Debug.Assert(Clipboard != null);
            Clipboard.IsVisible = !Clipboard.IsVisible;
            if(Clipboard.IsVisible) {
                ClipboardWindow.Dispatcher.BeginInvoke(new Action(() => {
                    WindowsUtility.ShowActive(ClipboardWindow.Handle);
                }), DispatcherPriority.SystemIdle);
            }
        }

        void SwitchShowTemplateWindow()
        {
            Debug.Assert(Template != null);

            Template.IsVisible = !Template.IsVisible;
            if(Template.IsVisible) {
                TemplateWindow.Dispatcher.BeginInvoke(new Action(() => {
                    WindowsUtility.ShowActive(TemplateWindow.Handle);
                }), DispatcherPriority.SystemIdle);
            }
        }

        void CallPropertyChangeNoteMenu()
        {
            var propertyNames = new[] {
                "NoteShowItems",
                "NoteHiddenItems",
            };
            CallOnPropertyChange(propertyNames);
        }

        void CallPropertyChangeSystemEnvironment()
        {
            var propertyNames = new[] {
                "IsVisibledShellHideFile",
                "IsVisibledShellExtension",
            };
            CallOnPropertyChange(propertyNames);
        }

        void CallPropertyChangeHotkey()
        {
            var propertyNames = new[] {
                nameof(CreateNoteHotKey),
                nameof(CompactNoteItemsHotKey),
                nameof(HideNoteItemsHotKey),
                nameof(FrontNoteItemsHotKey),
                nameof(SwitchTemplateWindowHotKey),
                nameof(ShowCommandWindowHotKey),
                nameof(SwitchShellHideFileHotKey),
                nameof(SwitchShellExtensionHotKey),
                nameof(SwitchClipboardWindowHotKey),
            };
            CallOnPropertyChange(propertyNames);
        }

        void ShowCommandWindow()
        {
            var devicePosition = MouseUtility.GetDevicePosition();
            // TODO: 論理座標！
            var logicalPosition = devicePosition;
            //CommandWindow.Visibility = Visibility.Visible;
            Command.WindowLeft = logicalPosition.X;
            Command.WindowTop = logicalPosition.Y;
            Command.Visibility = Visibility.Visible;

            CommandWindow.Dispatcher.BeginInvoke(new Action(() => {
                WindowsUtility.ShowActive(CommandWindow.Handle);
            }), DispatcherPriority.SystemIdle);
        }

        Updater CheckUpdate(bool force)
        {

            var updateData = new Updater(CommonData.VariableConstants.UserArchiveDirectoryPath, CommonData.MainSetting.RunningInformation.CheckUpdateRC, CommonData);
            CommonData.Logger.Debug(CommonData.Language["log/update/state"], string.Format("force = {0}, setting = {1}", force, CommonData.MainSetting.RunningInformation.CheckUpdateRelease));
            if(force || !IsPause && this.CommonData.MainSetting.RunningInformation.CheckUpdateRelease) {
                var updateInfo = updateData.Check();
            }
            return updateData;
        }

        /// <summary>
        /// アップデートを実行するか確認する。
        /// </summary>
        /// <param name="force">強制的に確認を行うか。</param>
        /// <param name="updateData">アップデート情報。</param>
        void ConfirmUpdate(bool force, Updater updateData)
        {
            if(force || !IsPause && CommonData.MainSetting.RunningInformation.CheckUpdateRelease) {
                if(updateData != null && updateData.Information != null) {
                    if(updateData.Information.IsUpdate) {
                        ShowUpdateDialog(updateData);
                    } else if(updateData.Information.IsError) {
                        CommonData.Logger.Warning(CommonData.Language["log/update/error"], updateData.Information.Log);
                    } else {
                        CommonData.Logger.Information(CommonData.Language["log/update/newest"], updateData.Information.Log);
                    }
                } else {
                    CommonData.Logger.Error(CommonData.Language["log/update/error"], "info is null");
                }
            } else if(IsPause) {
                CommonData.Logger.Information(CommonData.Language["log/update/check-stop"], "IsPause => true");
            }
        }

        /// <summary>
        /// アップデートチェックを非同期で行い、アップデートが存在すればアップデート確認を行う。
        /// </summary>
        public void CheckUpdateProcessAsync()
        {
#if !DISABLED_UPDATE_CHECK
            Task.Run(() => {
                // ネットワーク接続可能か？
                var nic = NetworkInterface.GetIsNetworkAvailable();
                if(nic) {
                    Thread.Sleep(Constants.updateWaitTime);
                    return CheckUpdate(false);
                } else {
                    return null;
                }
            }).ContinueWith(t => {
                if(t.Result != null) {
                    ConfirmUpdate(false, t.Result);
                } else {
                    CommonData.Logger.Information(CommonData.Language["log/update/check-stop"], CommonData.Language["log/update/nic"]);
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
#else
			if(this._commonData.Logger != null) {
				CommonData.Logger.Debug("update: check", "DISABLED_UPDATE_CHECK");
			}
#endif
        }

        /// <summary>
        /// アップデートチェックを同期的に行い、アップデートが存在すればアップデート確認を行う。
        /// </summary>
        /// <param name="force">強制的に確認を行うか。</param>
        void CheckUpdateProcessWait(bool force)
        {
            var updateData = CheckUpdate(force);
            ConfirmUpdate(force, updateData);
        }

        /// <summary>
        /// アップデートダイアログ表示。
        /// </summary>
        /// <param name="updateData"></param>
        void ShowUpdateDialog(Updater updateData)
        {
            try {
                PausingBasicAction(() => {
                    var window = new UpdateConfirmWindow();
                    window.SetCommonData(CommonData, updateData);
                    window.ShowDialog();
                    // 情報ダイアログの通知方法と合わせる
                    if(updateData.ApprovalUpdate) {
                        SaveSetting();
                        if(updateData.Execute()) {
                            ExitApplication(false, false);
                        }
                    }
                });
            } catch(Exception ex) {
                CommonData.Logger.Error(ex);
            }
        }

        void PausingBasicAction(Action action)
        {
            try {
                IsPause = true;
                StopIndexTimer();
                action();
            } finally {
                IsPause = false;
            }
        }

        void PuaseOutputLog(int frame = 2, [CallerFilePath] string callerFile = "", [CallerLineNumber] int callerLine = -1, [CallerMemberName] string callerMember = "")
        {
            CommonData.Logger.Debug(CommonData.Language["log/pause/skip"], null, frame, callerFile, callerLine, callerMember);
        }

        #endregion

        #region ViewModelBase

        protected override void Dispose(bool disposing)
        {
            UninitializeSystem();

            if(!IsDisposed) {
                if(IndexBodyCaching != null) {
                    IndexBodyCaching.Dispose();
                    IndexBodyCaching = null;
                }
                if(CommonData != null) {
                    CommonData.Dispose();
                    CommonData = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region IAppSender

        #region IAppSender-Send*

        public void SendAppendWindow(Window window)
        {
            ReceiveAppendWindow(window);
        }

        public Window SendCreateWindow(WindowKind kind, object extensionData, Window parent)
        {
            return ReceiveCreateWindow(kind, extensionData, parent);
        }

        public void SendRefreshView(WindowKind kind, Window fromView)
        {
            ReceiveRefreshView(kind, fromView);
        }

        public void SendRemoveIndex(IndexKind indexKind, Guid guid, Timing timing)
        {
            ReceiveRemoveIndex(indexKind, guid, timing);
        }

        public void SendSaveIndex(IndexKind indexKind, Timing timing)
        {
            ReceiveSaveIndex(indexKind, timing);
        }

        public IndexBodyItemModelBase SendLoadIndexBody(IndexKind indexKind, Guid guid)
        {
            return ReceiveLoadIndexBody(indexKind, guid);
        }

        public void SendSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid, Timing timing)
        {
            ReceiveSaveIndexBody(indexBody, guid, timing);
        }

        public void SendDeviceChanged(Data.ChangedDevice changedDevice)
        {
            ReceiveDeviceChanged(changedDevice);
        }

        public void SendClipboardChanged()
        {
            ReceiveClipboardChanged();
        }

        public void SendInputHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
        {
            ReceiveInputHotKey(hotKeyId, hotKeyModel);
        }

        public void SendInformationTips(string title, string message, LogKind logKind)
        {
            ReceiveInformationTips(title, message, logKind);
        }

        public void SendApplicationCommand(ApplicationCommand applicationCommand, object sender, ApplicationCommandArg arg)
        {
            CheckUtility.DebugEnforceNotNull(arg);

            ReceiveApplicationCommand(applicationCommand, sender, arg);
        }

        public void SendUserInformation()
        {
            ReceiveUserInformation();
        }

        #endregion

        #region IAppSender-Implement

        void ReceiveAppendWindow(Window window)
        {
            window.Closed += Window_Closed;

            var windowKind = window as IHavingWindowKind;
            if(windowKind != null) {
                switch(windowKind.WindowKind) {
                    case WindowKind.LauncherToolbar:
                        {
                            var toolbarWindow = (LauncherToolbarWindow)window;
                            LauncherToolbarWindows.Add(toolbarWindow);
                        }
                        break;

                    case WindowKind.LauncherExecute:
                    case WindowKind.LauncherCustomize:
                        {
                            OtherWindows.Add(window);
                        }
                        break;

                    case WindowKind.LauncherStream:
                        {
                            var streamWindow = (LauncherItemStreamWindow)window;
                            StreamWindows.Add(streamWindow);
                        }
                        break;

                    case WindowKind.Note:
                        {
                            var noteWindow = (NoteWindow)window;
                            NoteWindows.Add(noteWindow);

                            CallPropertyChangeNoteMenu();
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            } else {
                OtherWindows.Add(window);
            }
        }

        void RemoveWindow(Window window)
        {
            var havingWindwKind = window as IHavingWindowKind;
            if(havingWindwKind != null) {
                switch(havingWindwKind.WindowKind) {
                    case WindowKind.LauncherToolbar:
                        {
                            var toolbarWindow = (LauncherToolbarWindow)window;
                            LauncherToolbarWindows.Remove(toolbarWindow);
                        }
                        break;

                    case WindowKind.LauncherExecute:
                    case WindowKind.LauncherCustomize:
                        {
                            OtherWindows.Remove(window);
                        }
                        break;

                    case WindowKind.LauncherStream:
                        {
                            var streamWindow = (LauncherItemStreamWindow)window;
                            StreamWindows.Remove(streamWindow);
                        }
                        break;

                    case WindowKind.Note:
                        {
                            var noteWindow = (NoteWindow)window;
                            NoteWindows.Remove(noteWindow);
                            var viewModel = noteWindow.ViewModel;
                            ClearIndex(IndexKind.Note, viewModel.Model.Id, IndexBodyCaching.NoteItems);

                            CallPropertyChangeNoteMenu();
                            break;
                        }

                    default:
                        throw new NotImplementedException();
                }
            } else {
                OtherWindows.Remove(window);
            }
        }

        Window ReceiveCreateWindow(WindowKind kind, object extensionData, Window parent)
        {
            CommonDataWindow window = null;

            switch(kind) {
                case WindowKind.LauncherToolbar:
                    {
                        window = new LauncherToolbarWindow();
                        window.SetCommonData(CommonData, (ScreenModel)extensionData);
                    }
                    break;

                case WindowKind.LauncherExecute:
                    {
                        window = new LauncherItemExecuteWindow();
                        window.SetCommonData(CommonData, extensionData);
                    }
                    break;

                case WindowKind.LauncherCustomize:
                    {
                        window = new LauncherItemCustomizeWindow();
                        window.SetCommonData(CommonData, extensionData);
                    }
                    break;

                case WindowKind.LauncherStream:
                    {
                        window = new LauncherItemStreamWindow();
                        window.SetCommonData(CommonData, extensionData);
                    }
                    break;

                case WindowKind.Note:
                    {
                        var noteItem = (NoteIndexItemModel)extensionData;
                        if(!noteItem.IsVisible) {
                            CommonData.Logger.Debug("hidden -> show", noteItem);
                            noteItem.IsVisible = true;
                        }
                        window = new NoteWindow();
                        window.SetCommonData(CommonData, noteItem);
                    }
                    break;

                case WindowKind.Screen:
                    {
                        window = new ScreenWindow();
                        window.SetCommonData(CommonData, extensionData);
                    }
                    break;

                case WindowKind.HtmlViewer:
                    {
                        window = new HtmlViewerWindow();
                        window.SetCommonData(CommonData, extensionData);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }

            CommonData.AppSender.SendAppendWindow(window);
            return window;
        }

        void ReceiveRefreshView(WindowKind kind, Window fromView)
        {
            switch(kind) {
                case WindowKind.LauncherToolbar:
                    {
                        foreach(var toolbar in LauncherToolbars) {
                            toolbar.Refresh();
                        }
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ClearIndex<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyPairItemCollection<TIndexBody> cachingItems)
            where TIndexBody : IndexBodyItemModelBase
        {
            var index = cachingItems.IndexOf(guid);
            if(index != -1) {
                var pair = cachingItems[index];
                Debug.Assert(pair.Id == guid);
                cachingItems.RemoveAt(index);
                CommonData.Logger.Debug("cache dispose: " + pair.Id.ToString(), pair.Body);
                pair.Body.Dispose();
            }
        }

        void RemoveIndex<TItemModel, TIndexBody>(IndexKind indexKind, Guid guid, IndexItemCollectionModel<TItemModel> items, IndexBodyPairItemCollection<TIndexBody> cachingItems)
            where TItemModel : IndexItemModelBase
            where TIndexBody : IndexBodyItemModelBase
        {
            ClearIndex(indexKind, guid, cachingItems);

            items.Remove(guid);

            // ボディ部のファイルも削除する。
            IndexItemUtility.RemoveBody(indexKind, guid, cachingItems.Archive, CommonData.NonProcess);

            CommonData.AppSender.SendSaveIndex(indexKind, Timing.Delay);
        }

        void ReceiveRemoveIndex(IndexKind indexKind, Guid guid, Timing timing)
        {
            switch(indexKind) {
                case IndexKind.Note:
                    {
                        RemoveIndex(indexKind, guid, CommonData.NoteIndexSetting.Items, IndexBodyCaching.NoteItems);
                    }
                    break;

                case IndexKind.Template:
                    {
                        RemoveIndex(indexKind, guid, CommonData.TemplateIndexSetting.Items, IndexBodyCaching.TemplateItems);
                    }
                    break;

                case IndexKind.Clipboard:
                    {
                        RemoveIndex(indexKind, guid, CommonData.ClipboardIndexSetting.Items, IndexBodyCaching.ClipboardItems);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void SaveIndex_Impl<TIndexSetting>(IndexKind indexKind, TIndexSetting indexSetting, FileType fileType, string filePath)
            where TIndexSetting : ModelBase
        {
            var path = Environment.ExpandEnvironmentVariables(filePath);
            AppUtility.SaveSetting(path, indexSetting, fileType, true, CommonData.Logger);
        }

        void SaveIndex(IndexKind indexKind)
        {
            switch(indexKind) {
                case IndexKind.Note:
                    SaveIndex_Impl(indexKind, CommonData.NoteIndexSetting, Constants.fileTypeNoteIndex, CommonData.VariableConstants.UserSettingNoteIndexFilePath);
                    break;

                case IndexKind.Template:
                    SaveIndex_Impl(indexKind, CommonData.TemplateIndexSetting, Constants.fileTypeTemplateIndex, CommonData.VariableConstants.UserSettingTemplateIndexFilePath);
                    break;

                case IndexKind.Clipboard:
                    SaveIndex_Impl(indexKind, CommonData.ClipboardIndexSetting, Constants.fileTypeClipboardIndex, CommonData.VariableConstants.UserSettingClipboardIndexFilePath);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ReceiveSaveIndex(IndexKind indexKind, Timing timing)
        {
            lock(IndexSaveTimers) {
                var targetTimer = IndexSaveTimers[indexKind];
                if(targetTimer.IsEnabled) {
                    targetTimer.Stop();
                    Debug.WriteLine(string.Format("delay stop: {0}", indexKind));
                }

                if(timing == Timing.Instantly) {
                    SaveIndex(indexKind);
                } else {
                    Debug.Assert(timing == Timing.Delay);
                    CommonData.Logger.Debug(string.Format("delay start: {0}", indexKind), targetTimer.Interval);
                    targetTimer.Start();
                }
            }
        }

        void AppendCachingItems<TIndexBody>(Guid guid, TIndexBody indexBody, IndexBodyPairItemCollection<TIndexBody> cachingItems)
            where TIndexBody : IndexBodyItemModelBase
        {
            if(!cachingItems.Any(p => p.Id == guid)) {
                var pairItem = new IndexBodyPairItem<TIndexBody>(guid, indexBody);
                cachingItems.Add(pairItem);
                if(cachingItems.StockItems.Any()) {
                    var removedPairList = cachingItems.StockItems.ToArray();
                    cachingItems.StockItems.Clear();
                    foreach(var removePair in removedPairList) {
                        CommonData.Logger.Debug("cache dispose: " + removePair.Id.ToString(), removePair.Body);
                        removePair.Dispose();
                    }
                }
            }
        }

        IndexBodyItemModelBase GetIndexBody<TIndexBody>(IndexKind indexKind, Guid guid, IndexBodyPairItemCollection<TIndexBody> cachingItems)
            where TIndexBody : IndexBodyItemModelBase, new()
        {
            var body = cachingItems.GetFromId(guid);
            if(body != null) {
                CommonData.Logger.Debug("load cache: " + guid.ToString(), body.DisplayText);
                return body;
            }
            //var fileType = IndexItemUtility.GetBodyFileType(indexKind);
            //var path = IndexItemUtility.GetBodyFilePath(indexKind, guid, CommonData.VariableConstants);
            //var result = AppUtility.LoadSetting<TIndexBody>(path, fileType, CommonData.Logger);
            var result = IndexItemUtility.LoadBody<TIndexBody>(indexKind, guid, cachingItems.Archive, CommonData.NonProcess);

            AppendCachingItems(guid, result, cachingItems);
            return result;
        }

        public IndexBodyItemModelBase ReceiveLoadIndexBody(IndexKind indexKind, Guid guid)
        {
            switch(indexKind) {
                case IndexKind.Note:
                    return GetIndexBody<NoteBodyItemModel>(indexKind, guid, IndexBodyCaching.NoteItems);

                case IndexKind.Template:
                    return GetIndexBody<TemplateBodyItemModel>(indexKind, guid, IndexBodyCaching.TemplateItems);

                case IndexKind.Clipboard:
                    return GetIndexBody<ClipboardBodyItemModel>(indexKind, guid, IndexBodyCaching.ClipboardItems);

                default:
                    throw new NotImplementedException();
            }
        }

        void SaveIndexBody<TIndexBody>(IndexBodyItemModelBase indexBody, Guid guid, IndexBodyPairItemCollection<TIndexBody> cachingItems, Timing timing)
            where TIndexBody : IndexBodyItemModelBase
        {
            //var fileType = IndexItemUtility.GetIndexBodyFileType(indexBody.IndexKind);
            //var path = IndexItemUtility.GetIndexBodyFilePath(indexBody.IndexKind, guid, CommonData.VariableConstants);
            var bodyItem = (TIndexBody)indexBody;
            //AppUtility.SaveSetting(path, bodyItem, fileType, CommonData.Logger);
            //AppendCachingItems(guid, bodyItem, cachingItems);
            IndexItemUtility.SaveBody(bodyItem, guid, cachingItems.Archive, IndexBodyKind.File, CommonData.NonProcess);
            AppendCachingItems(guid, bodyItem, cachingItems);
        }

        void ReceiveSaveIndexBody(IndexBodyItemModelBase indexBody, Guid guid, Timing timing)
        {
            switch(indexBody.IndexKind) {
                case IndexKind.Note:
                    SaveIndexBody<NoteBodyItemModel>(indexBody, guid, IndexBodyCaching.NoteItems, timing);
                    break;

                case IndexKind.Template:
                    SaveIndexBody<TemplateBodyItemModel>(indexBody, guid, IndexBodyCaching.TemplateItems, timing);
                    break;

                case IndexKind.Clipboard:
                    SaveIndexBody<ClipboardBodyItemModel>(indexBody, guid, IndexBodyCaching.ClipboardItems, timing);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ReceiveDeviceChanged(ChangedDevice changedDevice)
        {
            //CommonData.Logger.Information("catch: changed device");
            // TODO: まだ作ってないので暫定的に。
            var Initialized = true;

            // デバイス状態が変更されたか
            if(changedDevice.DBT == DBT.DBT_DEVNODES_CHANGED && Initialized && !IsPause) {
                // デバイス変更前のスクリーン数が異なっていればディスプレイの抜き差しが行われたと判定する
                // 現在生成されているツールバーの数が前回ディスプレイ数となる

                // 変更通知から現在数をAPIでまともに取得する
                var rawScreenCount = NativeMethods.GetSystemMetrics(SM.SM_CMONITORS);
                bool changedScreenCount = LauncherToolbars.Count() != rawScreenCount;

                Task.Run(() => {
                    // Forms で取得するディスプレイ数の合計値は少し遅れる
                    const int waitMax = Constants.screenCountChangeRetryCount;
                    int waitCount = 0;

                    var managedScreenCount = Screen.AllScreens.Count();
                    while(rawScreenCount != managedScreenCount) {
                        if(waitMax < ++waitCount) {
                            // タイムアウト
                            break;
                        }
                        Thread.Sleep(Constants.screenCountChangeWaitTime);
                        managedScreenCount = Screen.AllScreens.Count();
                    }

                }).ContinueWith(t => {
                    if(changedScreenCount) {
                        ChangedScreenCount();
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        void ReceiveClipboardChanged()
        {
            if(!CommonData.MainSetting.Clipboard.IsEnabled) {
                return;
            }

            var seq = NativeMethods.GetClipboardSequenceNumber();
            if(this._clipboardPreviousSequenceNumber == seq) {
                return;
            }
            this._clipboardPreviousSequenceNumber = seq;

            var now = DateTime.Now;
            if(now - this._clipboardPreviousTime <= CommonData.MainSetting.Clipboard.WaitTime) {
                // 待ち時間中のため取り込まない
                var map = new Dictionary<string, string>() {
                    { LanguageKey.logClipboardWaitTimePrev, this._clipboardPreviousTime.ToDetailTimestampString() },
                    { LanguageKey.logClipboardWaitTimeCurrent, now.ToDetailTimestampString() },
                    { LanguageKey.logClipboardWaitTimeSetting, CommonData.MainSetting.Clipboard.WaitTime.ToString() },
                    { LanguageKey.logClipboardWaitTimeWait, (now - this._clipboardPreviousTime).ToString() },
                };
                var message = CommonData.Language["log/clipboard/prev-time/message"];
                var detail = CommonData.Language["log/clipboard/prev-time/detail", map];
                CommonData.Logger.Information(message, detail);
                return;
            }
            this._clipboardPreviousTime = now;

            var clipboardData = ClipboardUtility.GetClipboardDataDefault(CommonData.MainSetting.Clipboard.CaptureType, MessageWindow.Handle, CommonData.NonProcess);
            if(clipboardData == null || clipboardData.Type == ClipboardType.None) {
                CommonData.NonProcess.Logger.Trace(CommonData.NonProcess.Language["log/clipboard/capture/not-support"]);
                return;
            }

            Task.Run(() => {
                var notify = new ClipboardCaptureNotifyData();

                // 制限サイズによるフィルタリング
                ClipboardUtility.FilterLimitSize(clipboardData, CommonData.MainSetting.Clipboard.LimitSize, CommonData.NonProcess);
                if(clipboardData.Type == ClipboardType.None) {
                    notify.EmptyFromFiltered = true;
                    return notify;
                }

                // ハッシュ算出
                clipboardData.Hash.Type = HashType.SHA1;
                clipboardData.Hash.Code = ClipboardUtility.CalculateHashCode(clipboardData.Hash.Type, clipboardData.Type, clipboardData.Body);

                if(Clipboard.IndexItems.Any()) {
                    if(CommonData.MainSetting.Clipboard.DuplicationCount == 0) {
                        // 範囲チェックを行わないのであれば無条件で追加
                        return notify;
                    }

                    // 指定範囲内に同じデータがあれば追加しない
                    var clipboardItems = CommonData.ClipboardIndexSetting.Items.Reverse();
                    if(CommonData.MainSetting.Clipboard.DuplicationCount != Constants.clipboardDuplicationCount.minimum) {
                        clipboardItems = clipboardItems.Take(CommonData.MainSetting.Clipboard.DuplicationCount);
                    }
                    notify.DuplicationItem = clipboardItems.FirstOrDefault(c => clipboardData.Hash.IsEqual(c.Hash));
                    return notify;
                }
                return notify;
            }).ContinueWith(t => {
                if(t.Result.EmptyFromFiltered) {
                    CommonData.Logger.Information(CommonData.Language["log/clipboard/filter/empty"]);
                    return;
                }
                var dupItem = t.Result.DuplicationItem;
                if(dupItem == null) {
                    try {
                        //this._commonData.MainSetting.Clipboard.HistoryItems.Insert(0, clipboardItem);
                        //Clipboard.IndexItems.Insert();
                        //var body = ReceiveGetIndexBody(IndexKind.Template)
                        var displayText = DisplayTextUtility.MakeClipboardName(clipboardData, CommonData.NonProcess);
                        var index = new ClipboardIndexItemModel() {
                            Name = displayText,
                            Type = clipboardData.Type,
                            Hash = clipboardData.Hash,
                        };
                        SettingUtility.InitializeClipboardIndexItem(index, null, CommonData.NonProcess);
                        SettingUtility.UpdateUniqueGuid(index, Clipboard.IndexPairList.ModelList);
                        Clipboard.IndexPairList.Add(index, null);
                        index.History.Update();
                        CommonData.AppSender.SendSaveIndex(IndexKind.Clipboard, Timing.Delay);
                        CommonData.AppSender.SendSaveIndexBody(clipboardData.Body, index.Id, Timing.Delay);
                        if(!ClipboardWindow.IsActive && ClipboardWindow.IsVisible) {
                            ClipboardWindow.listItems.SelectedItem = ClipboardWindow.listItems.Items[0];
                            ClipboardWindow.listItems.ScrollIntoView(ClipboardWindow.listItems.SelectedItem);
                        }
                    } catch(Exception ex) {
                        CommonData.Logger.Error(ex);
                    }
                } else {
                    if(Clipboard.DuplicationMoveHead) {
                        CommonData.Logger.Information(CommonData.Language["log/clipboard/dup-item/move"], dupItem);

                        Clipboard.IndexPairList.Remove(dupItem);
                        var nowTime = DateTime.Now;
                        dupItem.Sort = nowTime;
                        dupItem.History.Update(nowTime);
                        var item = Clipboard.IndexPairList.Add(dupItem, null);
                        Clipboard.SelectedViewModel = item.ViewModel;
                        CommonData.AppSender.SendSaveIndex(IndexKind.Clipboard, Timing.Delay);
                    } else {
                        CommonData.Logger.Information(CommonData.Language["log/clipboard/dup-item/ignore"], dupItem);
                    }
                }

                t.Dispose();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        void ReceiveInputHotKey(HotKeyId hotKeyId, HotKeyModel hotKeyModel)
        {
            if(IsPause) {
                PuaseOutputLog();
                return;
            }

            switch(hotKeyId) {
                case HotKeyId.ShowCommand:
                    ShowCommandWindow();
                    CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/command/show/title"], CommonData.Language["notify/info/command/show/message"], LogKind.Information);
                    break;

                case HotKeyId.HiddenFile:
                    {
                        SwitchShellHideFile();
                        string message;
                        if(SystemEnvironmentUtility.IsHiddenFileShow()) {
                            message = "notify/info/hiddenfile/message/show";
                        } else {
                            message = "notify/info/hiddenfile/message/hide";
                        }
                        CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/hiddenfile/title"], CommonData.Language[message], LogKind.Information);
                    }
                    break;

                case HotKeyId.Extension:
                    {
                        SwitchShellExtension();
                        string message;
                        if(SystemEnvironmentUtility.IsExtensionShow()) {
                            message = "notify/info/extension/message/show";
                        } else {
                            message = "notify/info/extension/message/hide";
                        }
                        CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/extension/title"], CommonData.Language[message], LogKind.Information);
                    }
                    break;

                case HotKeyId.CreateNote:
                    {
                        var devicePoint = MouseUtility.GetDevicePosition();
                        // TODO: 論理座標取れてない！
                        var logcalPoint = devicePoint;
                        var noteSize = Constants.noteDefualtSize;
                        var window = CreateNoteItem(logcalPoint, noteSize, true);
                        CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/note/create/title"], CommonData.Language["notify/info/note/create/message"], LogKind.Information);
                        //WindowsUtility.ShowNoActive(window.Handle);
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => {
                            WindowsUtility.ShowActive(window.Handle);
                        }), DispatcherPriority.SystemIdle);
                    }
                    break;

                case HotKeyId.HideNote:
                    HideNoteItems();
                    CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/note/hide/title"], CommonData.Language["notify/info/note/hide/message"], LogKind.Information);
                    break;

                case HotKeyId.CompactNote:
                    CompactNoteItems();
                    CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/note/compact/title"], CommonData.Language["notify/info/note/compact/message"], LogKind.Information);
                    break;

                case HotKeyId.ShowFrontNote:
                    MoveFrontNoteItems();
                    CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/note/front/title"], CommonData.Language["notify/info/note/front/message"], LogKind.Information);
                    break;

                case HotKeyId.SwitchClipboardShow:
                    {
                        SwitchShowClipboardWindow();
                        string message;
                        if(Clipboard.IsVisible) {
                            message = "notify/info/clipboard/message/show";
                        } else {
                            message = "notify/info/clipboard/message/hide";
                        }
                        CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/clipboard/title"], CommonData.Language[message], LogKind.Information);
                    }
                    break;

                case HotKeyId.SwitchTemplateShow:
                    {
                        SwitchShowTemplateWindow();
                        string message;
                        if(Template.IsVisible) {
                            message = "notify/info/template/message/show";
                        } else {
                            message = "notify/info/template/message/hide";
                        }
                        CommonData.AppSender.SendInformationTips(CommonData.Language["notify/info/template/title"], CommonData.Language[message], LogKind.Information);
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        void ReceiveInformationTips(string title, string message, LogKind logKind)
        {
            if(HasView) {
                var map = new Dictionary<LogKind, BalloonIcon>() {
                    { LogKind.None, BalloonIcon.None },
                    { LogKind.Information, BalloonIcon.Info },
                    { LogKind.Warning, BalloonIcon.Warning },
                    { LogKind.Error, BalloonIcon.Error },
                };
                switch(CommonData.MainSetting.General.Notification) {
                    case Notification.System:
                        View.ShowBalloonTip(title, message, map[logKind]);
                        break;

                    case Notification.Silent:
                        throw new NotImplementedException(nameof(Notification.Silent));

                    case Notification.None:
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
            var action = new Dictionary<LogKind, LogPutDelegate>() {
                { LogKind.None, CommonData.Logger.Trace },
                { LogKind.Information, CommonData.Logger.Information },
                { LogKind.Warning, CommonData.Logger.Warning },
                { LogKind.Error, CommonData.Logger.Error },
            };
            action[logKind](title, message);
        }

        void ReceiveApplicationCommand(ApplicationCommand applicationCommand, object sender, ApplicationCommandArg arg)
        {
            Debug.Assert(arg != null);

            switch(applicationCommand) {
                case ApplicationCommand.MemoryGarbageCollect:
                    {
                        var prevTime = DateTime.Now;
                        var prevUsingMemory = GC.GetTotalMemory(false);
                        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        var gcUsingMemory = GC.GetTotalMemory(true);
                        var gcTime = DateTime.Now;
                        var detail = new[] {
                            prevTime.ToString() + " " + prevUsingMemory.ToString(),
                            "GC",
                            gcTime.ToString() + " " + gcUsingMemory.ToString(),
                        };
                        CommonData.Logger.Debug(applicationCommand.ToString(), string.Join(Environment.NewLine, detail));
                    }
                    break;

                default:
                    throw new NotImplementedException();
            }
        }

        async void ReceiveUserInformation()
        {
            // TODO: 許可されているか
            if(!CommonData.MainSetting.RunningInformation.SendPersonalInformation) {
                return;
            }

            CommonData.Logger.Trace(CommonData.Language["log/privacy/send"]);

            // ネットワーク接続可能か？
            var nic = NetworkInterface.GetIsNetworkAvailable();
            if(!nic) {
                CommonData.Logger.Information(CommonData.Language["log/privacy/nic"]);
                return;
            }

            using(var ui = new UserInformationSender(new Uri(Constants.UriUserInformation), CommonData.MainSetting.RunningInformation)) {
                CommonData.Logger.Information(
                    CommonData.Language["log/privacy/send/start"],
                    await ui.SendData.ReadAsStringAsync()
                );
                try {
                    var response = await ui.SendAync();
                    if(response.StatusCode == HttpStatusCode.OK) {
                        // ログ出力用に生の文字列を取得する(ストリーム→データ変換した方が楽だけど生じゃなくなる)
                        var result = await response.Content.ReadAsStringAsync();
                        using(var stream = new MemoryStream(Encoding.UTF8.GetBytes(result))) {
                            var model = SerializeUtility.LoadJsonDataFromStream<ResponseDataModel>(stream);
                            string langKey;
                            LogPutDelegate logPut;
                            if(model.Success) {
                                langKey = "log/privacy/send/end/ok";
                                logPut = CommonData.Logger.Information;
                            } else {
                                langKey = "log/privacy/send/end/ng";
                                logPut = CommonData.Logger.Warning;
                            }
                            var map = new Dictionary<string, string>() {
                            { LanguageKey.logPrivacySendDataId, model.UserDataId },
                            { LanguageKey.logPrivacySendRecvData, model.ToString() },
                            { LanguageKey.logPrivacySendRecvRaw, result },
                        };
                            logPut(CommonData.Language[langKey], CommonData.Language["log/privacy/send/end/detail", map]);
                        }
                    } else {
                        CommonData.Logger.Error(CommonData.Language["log/privacy/send/failure"], response.Headers);
                    }
                } catch(Exception ex) {
                    CommonData.Logger.Warning(CommonData.Language["log/privacy/send/failure"], ex);
                }
            }
        }

        #endregion

        #endregion

        #region IClipboardWatcher

        public void ClipboardWatchingChange(bool watch)
        {
            if(watch) {
                MessageWindow.RegistClipboardListener();
            } else {
                MessageWindow.UnregistClipboardListener();
            }
        }

        public bool ClipboardWatching { get { return MessageWindow.ClipboardListenerRegisted; } }

        public bool ClipboardEnabledApplicationCopy { get { return CommonData.MainSetting.Clipboard.IsEnabledApplicationCopy; } }

        public bool UsingClipboard
        {
            get { return CommonData.MainSetting.Clipboard.UsingClipboard; }
            set { SetPropertyValue(CommonData.MainSetting.Clipboard, value); }
        }

        #endregion

        #region IHavingView

        public TaskbarIcon View { get; private set; }

        public bool HasView { get { return HavingViewUtility.GetHasView(this); } }

        #endregion

        void Timer_Tick(object sender, EventArgs e)
        {
            if(IsPause) {
                PuaseOutputLog();
                return;
            }

            var timer = (DispatcherTimer)sender;
            timer.Stop();
            try {
                if(timer == WindowSaveTimer) {
                    if(CommonData.MainSetting.WindowSave.IsEnabled) {
                        SaveWindowItemAsync(WindowSaveType.Timer);
                    }
                }
            } finally {
                timer.Start();
            }
        }

        void SystemEvents_DisplaySettingsChanging(object sender, EventArgs e)
        {
            if(CommonData.MainSetting.WindowSave.IsEnabled) {
                SaveWindowItem(WindowSaveType.System);
            }

            CommonData.Logger.Information(CommonData.Language["log/screen/change-setting"]);
            ResetToolbar();
        }

        void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
        {
            CommonData.Logger.Information(CommonData.Language["log/session/switch"], e);
            if(e.Reason == SessionSwitchReason.ConsoleConnect || e.Reason == SessionSwitchReason.SessionUnlock) {
                ResetToolbar();
                if(e.Reason == SessionSwitchReason.SessionUnlock) {
                    CheckUpdateProcessAsync();
                    CommonData.AppSender.SendUserInformation();
                }
            } else if(e.Reason == SessionSwitchReason.ConsoleDisconnect) {
                SaveSetting();
            }
        }

        void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            CommonData.Logger.Information(CommonData.Language["log/session/ending"], e);
            SaveSetting();
        }

        void Window_Closed(object sender, EventArgs e)
        {
            RemoveWindow(sender as Window);
        }

        void View_PreviewTrayContextMenuOpen(object sender, RoutedEventArgs e)
        {
            if(IsPause) {
                e.Handled = true;
            } else {
                LanguageUtility.RecursiveSetLanguage(((TaskbarIcon)sender).ContextMenu, CommonData.Language);
            }
        }

        private void IndexTimer_Tick(object sender, EventArgs e)
        {
            var timer = (IndexDispatcherTimer)sender;
            timer.Stop();
            CommonData.Logger.Debug(string.Format("delay save: {0}", timer.IndexKind));
            SaveIndex(timer.IndexKind);
        }

        private void Current_Exit(object sender, ExitEventArgs e)
        {
            Application.Current.Exit -= Current_Exit;
            SystemEnvironmentUtility.ResetUsingBrowserVersionForExecutingAssembly();
        }
    }
}
