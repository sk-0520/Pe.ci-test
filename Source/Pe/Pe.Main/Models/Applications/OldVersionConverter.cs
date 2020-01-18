using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Library.SharedLibrary.Define;
using ContentTypeTextNet.Library.SharedLibrary.Model;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Item;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Library.PeData.Setting.MainSettings;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PeMain;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.Logic;
using ContentTypeTextNet.Pe.PeMain.Logic.Utility;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class OldVersionConverter
    {
        #region define

        class ScreenData : IScreenData
        {
            public ScreenData(string name)
            {
                ScreenName = name;
            }
            public string ScreenName { get; }

            public long X => -1;

            public long Y => -1;

            public long Width => 0;

            public long Height => 0;
        }


        #endregion

        public OldVersionConverter(string oldSettingRootDirectoryPath, IDatabaseAccessor mainDatabaseAccessor, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            VariableConstants = new VariableConstants(oldSettingRootDirectoryPath);
            MainDatabaseAccessor = mainDatabaseAccessor;
            StatementLoader = statementLoader;
            IdFactory = idFactory;

            IndexBodyCaching = new IndexBodyCaching(
                -1,
                -1,
                VariableConstants
            );

        }


        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IIdFactory IdFactory { get; }

        VariableConstants VariableConstants { get; }
        IDatabaseAccessor MainDatabaseAccessor { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        IndexBodyCaching IndexBodyCaching { get; set; }

        #endregion

        #region function

        public bool ExistisOldSetting()
        {
            var oldSettingFilePath = Environment.ExpandEnvironmentVariables(VariableConstants.UserSettingMainSettingFilePath);
            Logger.LogInformation("旧設定ファイル: {0}", oldSettingFilePath);
            return !string.IsNullOrWhiteSpace(oldSettingFilePath) && File.Exists(oldSettingFilePath);
        }

        Guid CopyOrCreateFont(FontModel font, Guid srcFontId, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var fontsEntityDao = new FontsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

            var fontId = IdFactory.CreateFontId();
            if(!string.IsNullOrWhiteSpace(font.Family)) {
                var fontData = new FontData() {
                    FamilyName = font.Family,
                    IsBold = font.Bold,
                    IsItalic = font.Italic,
                    Size = font.Size,
                };
                fontsEntityDao.InsertFont(fontId, fontData, DatabaseCommonStatus.CreateCurrentAccount());
            } else {
                Logger.LogInformation("フォント未設定のため標準フォントを使用");
                fontsEntityDao.InsertCopyFont(srcFontId, fontId, DatabaseCommonStatus.CreateCurrentAccount());
            }

            return fontId;
        }

        void UpdateFont(FontModel font, Guid fontId, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            if(!string.IsNullOrWhiteSpace(font.Family)) {
                var fontsEntityDao = new FontsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
                var fontData = new FontData() {
                    FamilyName = font.Family,
                    IsBold = font.Bold,
                    IsItalic = font.Italic,
                    Size = font.Size,
                };
                fontsEntityDao.UpdateFont(fontId, fontData, DatabaseCommonStatus.CreateCurrentAccount());
            }

        }

        private IReadOnlyCollection<Guid> ImportLauncherItems(LauncherItemSettingModel launcherItemSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogWarning("[互換性破棄] " + nameof(LauncherItemFileDropMode) + ": {0}", launcherItemSetting.FileDropMode);
            Logger.LogInformation("ランチャーアイテム数: {0}", launcherItemSetting.Items.Count);

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

            var importedItems = new HashSet<Guid>(launcherItemSetting.Items.Count);
            var codes = new List<string>(launcherItemSetting.Items.Count);
            foreach(var item in launcherItemSetting.Items) {
                Logger.LogInformation("アイテム取り込み: {0}, {1}", item.Name, item.Id);
                if(item.LauncherKind != LauncherKind.File) {
                    Logger.LogWarning("[互換性破棄]　" + nameof(LauncherKind) + ": {0}", item.LauncherKind);
                }
                if(string.IsNullOrWhiteSpace(item.Command)) {
                    Logger.LogInformation("コマンド未設定のため取り込み対象外");
                    continue;
                }

                var rawCode = launcherFactory.ToCode(item.Command);
                var singleCode = launcherFactory.GetUniqueCode(rawCode, codes);
                codes.Add(singleCode);

                var launcherItemData = new LauncherItemOldImportData() {
                    LauncherItemId = item.Id,
                    Code = singleCode,
                    Name = item.Name,
                    Kind = LauncherItemKind.File, // 強制
                    IsEnabledCommandLauncher = item.IsCommandAutocomplete,
                    Comment = item.Comment ?? string.Empty,
                    ExecuteCount = item.History.ExecuteCount,
                    LastExecuteTimestamp = item.History.ExecuteTimestamp.ToUniversalTime(),
                };
                if(!string.IsNullOrWhiteSpace(item.Icon.Path)) {
                    launcherItemData.Icon.Path = item.Icon.Path;
                    launcherItemData.Icon.Index = item.Icon.Index;
                }

                launcherItemsEntityDao.InsertOldLauncherItem(launcherItemData, DatabaseCommonStatus.CreateCurrentAccount());

                var launcherFileData = new LauncherFileData() {
                    Path = item.Command,
                    Option = item.Option ?? string.Empty,
                    WorkDirectoryPath = item.WorkDirectoryPath ?? string.Empty,
                    IsEnabledCustomEnvironmentVariable = item.EnvironmentVariables.Edit,
                    IsEnabledStandardInputOutput = item.StdStream.OutputWatch,
                    RunAdministrator = item.Administrator,
                };
                launcherFilesEntityDao.InsertFileWithCustom(launcherItemData.LauncherItemId, launcherFileData, launcherFileData, DatabaseCommonStatus.CreateCurrentAccount());

                var histories = item.History.Options
                    .Select(i => (kind: LauncherHistoryKind.Option, value: i))
                    .Concat(item.History.WorkDirectoryPaths.Select(i => (kind: LauncherHistoryKind.Option, value: i)))
                    .Where(i => !string.IsNullOrWhiteSpace(i.value))
                    .ToList()
                ;
                foreach(var history in histories) {
                    launcherItemHistoriesEntityDao.InsertHistory(launcherItemData.LauncherItemId, history.kind, history.value, DatabaseCommonStatus.CreateCurrentAccount());
                }

                var envItems = item.EnvironmentVariables.Update
                    .Select(i => new LauncherEnvironmentVariableData() { Name = i.Id, Value = i.Value })
                    .Concat(item.EnvironmentVariables.Remove.Select(i => new LauncherEnvironmentVariableData() { Name = i, Value = string.Empty }))
                    .Where(i => !string.IsNullOrWhiteSpace(i.Name))
                    .ToList();
                ;
                if(0 < envItems.Count) {
                    launcherEnvVarsEntityDao.InsertEnvVarItems(launcherItemData.LauncherItemId, envItems, DatabaseCommonStatus.CreateCurrentAccount());
                }

                var tags = item.Tag.Items
                    .Where(i => !string.IsNullOrWhiteSpace(i))
                    .ToList()
                ;
                if(0 < tags.Count) {
                    launcherTagsEntityDao.InsertTags(launcherItemData.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());
                }

                importedItems.Add(launcherItemData.LauncherItemId);
            }

            return importedItems;
        }

        private IReadOnlyCollection<Guid> ImportGroups(LauncherGroupSettingModel launcherGroupSetting, IReadOnlyCollection<Guid> importedItems, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogInformation("グループ数: {0}", launcherGroupSetting.Groups);

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

            var importedGroups = new HashSet<Guid>(launcherGroupSetting.Groups.Count);

            foreach(var group in launcherGroupSetting.Groups) {
                if(group.GroupKind != GroupKind.LauncherItems) {
                    Logger.LogInformation("互換性なしグループのため破棄: {0}, {1}", group.GroupKind, group.Id);
                    continue;
                }

                var groupStep = launcherFactory.GroupItemStep;
                var sequence = launcherGroupsEntityDao.SelectMaxSequence() + groupStep;

                var launcherGroupData = new LauncherGroupData() {
                    LauncherGroupId = group.Id,
                    Kind = LauncherGroupKind.Normal,
                    Name = group.Name,
                    Sequence = sequence,
                    ImageColor = group.GroupIconColor,
                };
                var images = new Dictionary<LauncherGroupIconType, LauncherGroupImageName>() {
                    [LauncherGroupIconType.Folder] = LauncherGroupImageName.DirectoryNormal,
                    [LauncherGroupIconType.File] = LauncherGroupImageName.File,
                    [LauncherGroupIconType.Bookmark] = LauncherGroupImageName.Bookmark,
                    [LauncherGroupIconType.Builder] = LauncherGroupImageName.Builder,
                    [LauncherGroupIconType.Config] = LauncherGroupImageName.Config,
                    [LauncherGroupIconType.Gear] = LauncherGroupImageName.Gear,
                    [LauncherGroupIconType.LightBulb] = LauncherGroupImageName.Light,
                    [LauncherGroupIconType.Shortcut] = LauncherGroupImageName.Shortcut,
                    [LauncherGroupIconType.Storage] = LauncherGroupImageName.Storage,
                    [LauncherGroupIconType.User] = LauncherGroupImageName.User,
                };
                if(images.TryGetValue(group.GroupIconType, out var value)) {
                    launcherGroupData.ImageName = value;
                } else {
                    Logger.LogWarning("[互換性破棄]　" + nameof(LauncherGroupIconType) + ": {0}", group.GroupIconType);
                    launcherGroupData.ImageName = LauncherGroupImageName.DirectoryNormal;
                }

                launcherGroupsEntityDao.InsertNewGroup(launcherGroupData, DatabaseCommonStatus.CreateCurrentAccount());

                var currentMaxSequence = launcherGroupItemsEntityDao.SelectMaxSequence(launcherGroupData.LauncherGroupId);
                var launcherItemIds = group.LauncherItems
                    .Where(i => importedItems.Contains(i))
                    .ToList()
                ;
                launcherGroupItemsEntityDao.InsertNewItems(launcherGroupData.LauncherGroupId, launcherItemIds, currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());

                importedGroups.Add(launcherGroupData.LauncherGroupId);
            }

            return importedGroups;
        }

        private void ImportToolbars(ToolbarSettingModel toolbarSetting, IReadOnlyCollection<Guid> importedGroups, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var screenChecker = new ScreenChecker();
            var screens = Screen.AllScreens.ToHashSet();

            var hitToolbars = new Dictionary<IScreen, ToolbarItemModel>();
            foreach(var toolbar in toolbarSetting.Items) {
                Screen? dockScreen = null;
                foreach(var screen in screens) {
                    if(screenChecker.FindMaybe(screen, new ScreenData(toolbar.Id))) {
                        dockScreen = screen;
                        break;
                    }
                }
                if(dockScreen != null) {
                    hitToolbars.Add(dockScreen, toolbar);
                    screens.Remove(dockScreen);
                } else {
                    Logger.LogWarning("ツールバーの所属ディスプレイが不明のため設定引継ぎせず: {0}", toolbar.Id);
                }
            }

            var appLauncherToolbarSettingEntityDao = new AppLauncherToolbarSettingEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherToolbarsEntityDao = new LauncherToolbarsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var screensEntityDao = new ScreensEntityDao(commander, StatementLoader, implementation, LoggerFactory);

            foreach(var (screen, toolbar) in hitToolbars) {
                Logger.LogInformation("ツールバー取り込み: {0}", toolbar.Id);

                var fontId = CopyOrCreateFont(toolbar.Font, appLauncherToolbarSettingEntityDao.SelectAppLauncherToolbarSettingFontId(), commander, implementation);

                screensEntityDao.InsertScreen(screen, DatabaseCommonStatus.CreateCurrentAccount());

                var launcherToolbarsOldData = new LauncherToolbarsOldData() {
                    LauncherToolbarId = IdFactory.CreateLauncherToolbarId(),
                    LauncherGroupId = importedGroups.Contains(toolbar.DefaultGroupId) ? toolbar.DefaultGroupId : Guid.Empty,
                    IconBox = (IconBox)toolbar.IconScale,
                    FontId = fontId,
                    Screen = screen,
                    IsTopmost = toolbar.IsTopmost,
                    AutoHideTimeout = toolbar.HideWaitTime,
                    TextWidth = (int)toolbar.TextWidth,
                    IsVisible = toolbar.IsVisible,
                    IsAutoHide = toolbar.AutoHide,
                    IsIconOnly = !toolbar.TextVisible,
                };
                var positions = new Dictionary<DockType, AppDesktopToolbarPosition>() {
                    [DockType.Left] = AppDesktopToolbarPosition.Left,
                    [DockType.Top] = AppDesktopToolbarPosition.Top,
                    [DockType.Right] = AppDesktopToolbarPosition.Right,
                    [DockType.Bottom] = AppDesktopToolbarPosition.Bottom,
                };
                if(positions.TryGetValue(toolbar.DockType, out var toolbarPosition)) {
                    launcherToolbarsOldData.ToolbarPosition = toolbarPosition;
                } else {
                    Logger.LogWarning("[互換性破棄]　" + nameof(DockType) + ": {0}", toolbar.DockType);
                    launcherToolbarsOldData.ToolbarPosition = AppDesktopToolbarPosition.Right;
                }

                var directions = new Dictionary<ToolbarButtonPosition, LauncherToolbarIconDirection>() {
                    [ToolbarButtonPosition.Near] = LauncherToolbarIconDirection.LeftTop,
                    [ToolbarButtonPosition.Center] = LauncherToolbarIconDirection.Center,
                    [ToolbarButtonPosition.Far] = LauncherToolbarIconDirection.RightBottom,
                };
                launcherToolbarsOldData.IconDirection = directions[toolbar.ButtonPosition];


                launcherToolbarsEntityDao.InsertOldToolbar(launcherToolbarsOldData, DatabaseCommonStatus.CreateCurrentAccount());
            }
        }

        private void ImportNotes(NoteSettingModel noteSetting, NoteIndexSettingModel noteIndexSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var primaryScreen = Screen.PrimaryScreen;

            var appNoteSettingEntityDao = new AppNoteSettingEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var notesEntityDao = new NotesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var noteLayoutsEntityDao = new NoteLayoutsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var noteContentsEntityDao = new NoteContentsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

            var settingAppNoteSettingData = appNoteSettingEntityDao.SelectSettingNoteSetting();
            settingAppNoteSettingData.BackgroundColor = noteSetting.BackColor;
            settingAppNoteSettingData.ForegroundColor = noteSetting.ForeColor;
            settingAppNoteSettingData.IsTopmost = noteSetting.IsTopmost;
            settingAppNoteSettingData.TitleKind = noteSetting.NoteTitle switch
            {
                NoteTitle.DefaultCaption => NoteCreateTitleKind.Count,
                NoteTitle.Timestamp => NoteCreateTitleKind.Timestamp,
                _ => NoteCreateTitleKind.Count,
            };
            appNoteSettingEntityDao.UpdateSettingNoteSetting(settingAppNoteSettingData, DatabaseCommonStatus.CreateCurrentAccount());
            UpdateFont(noteSetting.Font, settingAppNoteSettingData.FontId, commander, implementation);

            foreach(var note in noteIndexSetting.Items) {
                Logger.LogInformation("ノート取り込み: {0}", note.Id);

                var fontId = CopyOrCreateFont(note.Font, appNoteSettingEntityDao.SelectAppNoteSettingFontId(), commander, implementation);

                var noteData = new NoteData() {
                    NoteId = note.Id,
                    FontId = fontId,
                    Title = note.Name,
                    ContentKind = note.NoteKind switch
                    {
                        NoteKind.Text => NoteContentKind.Plain,
                        NoteKind.Rtf => NoteContentKind.RichText,
                        _ => NoteContentKind.Plain,
                    },
                    BackgroundColor = note.BackColor,
                    ForegroundColor = note.ForeColor,
                    IsCompact = note.IsCompacted,
                    IsLocked = note.IsLocked,
                    IsVisible = note.IsVisible,
                    IsTopmost = note.IsTopmost,
                    LayoutKind = NoteLayoutKind.Absolute,
                    ScreenName = primaryScreen.DeviceName,
                    TextWrap = note.AutoLineFeed,
                };
                notesEntityDao.InsertOldNote(noteData, DatabaseCommonStatus.CreateCurrentAccount());

                var noteLayoutData = new NoteLayoutData() {
                    NoteId = note.Id,
                    LayoutKind = NoteLayoutKind.Absolute,
                    X = note.WindowLeft,
                    Y = note.WindowTop,
                    Width = note.WindowWidth,
                    Height = note.WindowHeight,
                };
                noteLayoutsEntityDao.InsertLayout(noteLayoutData, DatabaseCommonStatus.CreateCurrentAccount());

                var content = IndexItemUtility.LoadBody<NoteBodyItemModel>(IndexKind.Note, note.Id, IndexBodyCaching.NoteItems.Archive, VariableConstants);
                var noteContentData = new NoteContentData() {
                    NoteId = note.Id,
                    Content = note.NoteKind switch
                    {
                        NoteKind.Text => content.Text,
                        NoteKind.Rtf => content.Rtf,
                        _ => content.Text,
                    },
                };
                noteContentsEntityDao.InsertNewContent(noteContentData, DatabaseCommonStatus.CreateCurrentAccount());
            }
        }

        private void ImportStandardOutputInput(StreamSettingModel streamSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var appStandardInputOutputSettingEntityDao = new AppStandardInputOutputSettingEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var setting = appStandardInputOutputSettingEntityDao.SelectSettingStandardInputOutputSetting();

            setting.OutputBackgroundColor = streamSetting.OutputColor.BackColor;
            setting.OutputForegroundColor = streamSetting.OutputColor.ForeColor;
            setting.ErrorBackgroundColor = streamSetting.ErrorColor.BackColor;
            setting.ErrorForegroundColor = streamSetting.ErrorColor.ForeColor;

            appStandardInputOutputSettingEntityDao.UpdateSettingStandardInputOutputSetting(setting, DatabaseCommonStatus.CreateCurrentAccount());

            UpdateFont(streamSetting.Font, setting.FontId, commander, implementation);
        }

        private void ImportCommand(CommandSettingModel commandSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogWarning("[互換性破棄] " + nameof(commandSetting.FindFile) + ": {0}", commandSetting.FindFile);

            var appCommandSettingEntityDao = new AppCommandSettingEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var setting = appCommandSettingEntityDao.SelectSettingCommandSetting();

            setting.FindTag = commandSetting.FindTag;
            setting.HideWaitTime = commandSetting.HideTime;
            setting.IconBox = (IconBox)commandSetting.IconScale;
            setting.Width = commandSetting.WindowWidth;

            appCommandSettingEntityDao.UpdateSettingCommandSetting(setting, DatabaseCommonStatus.CreateCurrentAccount());

            UpdateFont(commandSetting.Font, setting.FontId, commander, implementation);
        }

        public void Execute()
        {
            var old = new MainWorkerViewModel();
            var setting = old.LoadSetting(VariableConstants);

            using(var transaction = MainDatabaseAccessor.BeginTransaction()) {
                var importedItems = ImportLauncherItems(setting.LauncherItemSetting, transaction, transaction.Implementation);
                var importedGroups = ImportGroups(setting.LauncherGroupSetting, importedItems, transaction, transaction.Implementation);
                ImportToolbars(setting.MainSetting.Toolbar, importedGroups, transaction, transaction.Implementation);
                ImportNotes(setting.MainSetting.Note, setting.NoteIndexSetting, transaction, transaction.Implementation);
                ImportStandardOutputInput(setting.MainSetting.Stream, transaction, transaction.Implementation);
                ImportCommand(setting.MainSetting.Command, transaction, transaction.Implementation);
                //transaction.Commit();
            }
        }


        #endregion
    }
}
