using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

        private void ImportLauncherItems(LauncherItemSettingModel launcherItemSetting, IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            Logger.LogInformation("[互換性破棄] " + nameof(LauncherItemFileDropMode) + ": {0}", launcherItemSetting.FileDropMode);
            Logger.LogInformation("ランチャーアイテム: {0}", launcherItemSetting.Items.Count);

            var launcherFactory = new LauncherFactory(IdFactory, LoggerFactory);

            var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherItemHistoriesEntityDao = new LauncherItemHistoriesEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, implementation, LoggerFactory);
            var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, StatementLoader, implementation, LoggerFactory);

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
            }
        }


        public void Execute()
        {
            var old = new MainWorkerViewModel();
            var setting = old.LoadSetting(VariableConstants);

            using(var transaction = MainDatabaseAccessor.BeginTransaction()) {
                ImportLauncherItems(setting.LauncherItemSetting, transaction, transaction.Implementation);

                //transaction.Commit();
            }
        }

        #endregion
    }
}
