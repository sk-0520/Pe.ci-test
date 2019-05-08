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

        LauncherFileDetailData ConvertFile(LauncherPathExecuteData data)
        {
            var result = new LauncherFileDetailData() {
                Raw = data,
                Option = data.Option,
            };

            var filePath = Environment.ExpandEnvironmentVariables(data.Path ?? string.Empty);
            if(Directory.Exists(filePath)) {
                result.FileSystem = new DirectoryInfo(filePath);
                result.FileIsDirectory = Directory.Exists(result.FileSystem.FullName);
            } else {
                result.FileSystem = new FileInfo(filePath);
            }

            var workDirPath = Environment.ExpandEnvironmentVariables(data.Path ?? string.Empty);
            if(result.FileIsDirectory || string.IsNullOrWhiteSpace(workDirPath)) {
                result.WorkDirectory = new DirectoryInfo(Path.GetDirectoryName(result.FileSystem.FullName));
            } else {
                result.WorkDirectory = new DirectoryInfo(workDirPath);
            }

            return result;
        }

        public LauncherFileDetailData LoadFileDetail()
        {
            LauncherPathExecuteData pathData;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                pathData = dao.SelectPath(LauncherItemId);
            }

            var result = ConvertFile(pathData);

            result.FileSystem.Refresh();
            result.WorkDirectory.Refresh();

            return result;
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
