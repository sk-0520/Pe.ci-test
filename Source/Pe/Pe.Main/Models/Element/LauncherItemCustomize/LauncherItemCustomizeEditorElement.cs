using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public class LauncherItemCustomizeEditorElement : ElementBase, ILauncherItemId
    {
        public LauncherItemCustomizeEditorElement(Guid launcherItemId, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ClipboardManager = clipboardManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }
        IClipboardManager ClipboardManager { get; }

        public string Name { get; set; } = string.Empty;
        public string Code { get; private set; } = string.Empty;
        public LauncherItemKind Kind { get; private set; }
        public bool IsEnabledCommandLauncher { get; set; }

        public IconData IconData { get; set; } = new IconData();
        public string Comment { get; set; } = string.Empty;

        public ObservableCollection<string> TagItems { get; } = new ObservableCollection<string>();

        #region file

        public LauncherFileData? File { get; private set; }
        public ObservableCollection<LauncherEnvironmentVariableData> EnvironmentVariableItems { get; } = new ObservableCollection<LauncherEnvironmentVariableData>();

        #endregion

        #endregion


        #region function

        void LoadLauncherItem()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IconData = launcherItemData.Icon;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;

                var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var tagItems = launcherTagsEntityDao.SelectTags(LauncherItemId);
                TagItems.SetRange(tagItems);

                switch(Kind) {
                    case LauncherItemKind.File: {
                            var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                            File = launcherFilesEntityDao.SelectFile(LauncherItemId);

                            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                            var environmentVariableItems = launcherEnvVarsEntityDao.SelectEnvVarItems(LauncherItemId).ToList();
                            EnvironmentVariableItems.SetRange(environmentVariableItems);
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public LauncherFileData LoadFileData()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFile(LauncherItemId);
            }
        }

        public IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectEnvVarItems(LauncherItemId).ToList();
            }
        }


        public IReadOnlyCollection<string> LoadTags()
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectTags(LauncherItemId).ToList();
            }
        }

        public void Save()
        {

        }

        public void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags)
        {
            ThrowIfDisposed();

            using(var commander = MainDatabaseBarrier.WaitWrite()) {
                var launcherItemsEntityDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherMergeEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherTagsEntityDao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);

                launcherItemsEntityDao.UpdateCustomizeLauncherItem(launcherItemData, DatabaseCommonStatus.CreateCurrentAccount());
                launcherFilesEntityDao.UpdateCustomizeLauncherFile(launcherItemData.LauncherItemId, launcherFileData, launcherFileData, DatabaseCommonStatus.CreateCurrentAccount());

                launcherMergeEnvVarsEntityDao.DeleteEnvVarItemsByLauncherItemId(launcherItemData.LauncherItemId);
                launcherMergeEnvVarsEntityDao.InsertEnvVarItems(launcherItemData.LauncherItemId, environmentVariableItems, DatabaseCommonStatus.CreateCurrentAccount());

                launcherTagsEntityDao.DeleteTagByLauncherItemId(launcherItemData.LauncherItemId);
                launcherTagsEntityDao.InsertTags(launcherItemData.LauncherItemId, tags, DatabaseCommonStatus.CreateCurrentAccount());

                commander.Commit();
            }
            using(var commander = FileDatabaseBarrier.WaitWrite()) {
                var launcherItemIconsEntityDao = new LauncherItemIconsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                launcherItemIconsEntityDao.DeleteAllSizeImageBinary(launcherItemData.LauncherItemId);

                commander.Commit();
            }
        }

        #endregion

        #region ElementBase

        protected override void InitializeImpl()
        {
            LoadLauncherItem();
        }

        #endregion

        #region ILauncherItemId
        public Guid LauncherItemId { get; }
        #endregion
    }
}
