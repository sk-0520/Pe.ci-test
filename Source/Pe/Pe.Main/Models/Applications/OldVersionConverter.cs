using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models.Data;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Library.PeData.Define;
using ContentTypeTextNet.Pe.Library.PeData.Setting;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Launcher;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.PeMain.Data;
using ContentTypeTextNet.Pe.PeMain.ViewModel;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Applications
{
    public class OldVersionConverter
    {
        public OldVersionConverter(string oldSettingRootDirectoryPath, IDatabaseAccessor mainDatabaseAccessor, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            Logger = LoggerFactory.CreateLogger(GetType());

            VariableConstants = new VariableConstants(oldSettingRootDirectoryPath);
            MainDatabaseAccessor = mainDatabaseAccessor;
            StatementLoader = statementLoader;
            IdFactory = idFactory;
        }


        #region property

        ILoggerFactory LoggerFactory { get; }
        ILogger Logger { get; }
        IIdFactory IdFactory { get; }

            VariableConstants VariableConstants { get; }
        IDatabaseAccessor MainDatabaseAccessor { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        #endregion

        #region function

        public bool ExistisOldSetting()
        {
            var oldSettingFilePath = Environment.ExpandEnvironmentVariables(VariableConstants.UserSettingMainSettingFilePath);
            Logger.LogInformation("旧設定ファイル: {0}", oldSettingFilePath);
            return !string.IsNullOrWhiteSpace(oldSettingFilePath) && File.Exists(oldSettingFilePath);
        }

        private IReadOnlyCollection<Guid> ImportLauncherItems(LauncherItemSettingModel launcherItemSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogInformation("[互換性破棄] " + nameof(LauncherItemFileDropMode) + ": {0}", launcherItemSetting.FileDropMode);
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
                    Logger.LogInformation("[互換性破棄]　" + nameof(LauncherKind) + ": {0}", item.LauncherKind);
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

        private void ImportGroups(LauncherGroupSettingModel launcherGroupSetting, IReadOnlyCollection<Guid> importedItems, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogInformation("グループ数: {0}", launcherGroupSetting.Groups);

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherGroupsEntityDao = new LauncherGroupsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherGroupItemsEntityDao = new LauncherGroupItemsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

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
                    Logger.LogInformation("[互換性破棄]　" + nameof(LauncherGroupIconType) + ": {0}", group.GroupIconType);
                    launcherGroupData.ImageName = LauncherGroupImageName.DirectoryNormal;
                }

                launcherGroupsEntityDao.InsertNewGroup(launcherGroupData, DatabaseCommonStatus.CreateCurrentAccount());

                var currentMaxSequence = launcherGroupItemsEntityDao.SelectMaxSequence(launcherGroupData.LauncherGroupId);
                var launcherItemIds = group.LauncherItems
                    .Where(i => importedItems.Contains(i))
                    .ToList()
                ;
                launcherGroupItemsEntityDao.InsertNewItems(launcherGroupData.LauncherGroupId, launcherItemIds, currentMaxSequence + launcherFactory.GroupItemsStep, launcherFactory.GroupItemsStep, DatabaseCommonStatus.CreateCurrentAccount());
            }
        }

        public void Execute()
        {
            var old = new MainWorkerViewModel();
            var setting = old.LoadSetting(VariableConstants);

            using(var transaction = MainDatabaseAccessor.BeginTransaction()) {
                var importedItems = ImportLauncherItems(setting.LauncherItemSetting, transaction, transaction.Implementation);
                ImportGroups(setting.LauncherGroupSetting, importedItems, transaction, transaction.Implementation);

                //transaction.Commit();
            }
        }

        #endregion
    }
}
