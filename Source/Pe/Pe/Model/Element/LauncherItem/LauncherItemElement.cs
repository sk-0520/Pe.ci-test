using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model;
using ContentTypeTextNet.Pe.Library.Shared.Library.Model.Database;
using ContentTypeTextNet.Pe.Library.Shared.Link.Model;
using ContentTypeTextNet.Pe.Main.Model.Applications;
using ContentTypeTextNet.Pe.Main.Model.Data;
using ContentTypeTextNet.Pe.Main.Model.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Model.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Model.Launcher;
using ContentTypeTextNet.Pe.Main.Model.Manager;

namespace ContentTypeTextNet.Pe.Main.Model.Element.LauncherItem
{
    public class LauncherItemElement : ElementBase
    {
        public LauncherItemElement(Guid launcherItemId, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, LauncherIconElement launcherIconElement, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            Icon = launcherIconElement;
        }

        #region property

        public Guid LauncherItemId { get; }

        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public LauncherItemKind Kind { get; private set; }
        public bool IsEnabledCommandLauncher { get; private set; }
        public string Comment { get; private set; }

        public LauncherIconElement Icon { get; }

        #endregion

        #region function

        void LoadLauncherItem()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, this);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;
            }
        }

        LauncherPathKind GetPathKind(string path)
        {
            if(File.Exists(path)) {
                return LauncherPathKind.File;
            }

            if(Directory.Exists(path)) {
                return LauncherPathKind.Directory;
            }

            return LauncherPathKind.Unknown;
        }

        public LauncherFileDetailData LoadFileDetail()
        {
            LauncherPathExecuteData pathData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                pathData = launcherFilesEntityDao.SelectPath(LauncherItemId);
            }

            var expandedPath = PathUtility.ExpandFilePath(pathData.Path);
            var kind = GetPathKind(expandedPath);
            var result = new LauncherFileDetailData() {
                PathData = pathData,
                Kind = kind,
            };
            switch(result.Kind) {
                case LauncherPathKind.File:
                    result.FileSystemInfo = new FileInfo(expandedPath);
                    break;

                case LauncherPathKind.Directory:
                    result.FileSystemInfo = new DirectoryInfo(expandedPath);
                    break;

                case LauncherPathKind.Unknown:
                    result.FileSystemInfo = new EmptyFileSystemInfo(expandedPath);
                    break;

                default:
                    throw new NotImplementedException();
            }

            return result;
        }

        LauncherExecuteResult ExecuteFile()
        {
            LauncherFileData fileData;
            IList<LauncherEnvironmentVariableItem> envItems;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                if(fileData.IsEnabledCustomEnvironmentVariable) {
                    var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                    envItems = launcherEnvVarsEntityDao.SelectItems(LauncherItemId).ToList();
                } else {
                    envItems = new List<LauncherEnvironmentVariableItem>();
                }
            }

            var launcherExecutor = new LauncherExecutor(Logger.Factory);
            var result = launcherExecutor.Execute(fileData, envItems);

            return result;
        }

        public LauncherExecuteResult Execute()
        {
            switch(Kind) {
                case LauncherItemKind.File:
                    return ExecuteFile();

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion

        #region ElementBase

        override protected void InitializeImpl()
        {
            LoadLauncherItem();
        }

        #endregion
    }
}
