using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ContentTypeTextNet.Pe.Library.Shared.Library.Compatibility.Forms;
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
        #region variable

        bool _nowCustomize;

        #endregion

        public LauncherItemElement(Guid launcherItemId, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IDatabaseStatementLoader statementLoader, LauncherIconElement launcherIconElement, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;

            OrderManager = orderManager;
            ClipboardManager = clipboardManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            StatementLoader = statementLoader;

            Icon = launcherIconElement;
        }

        #region property

        public Guid LauncherItemId { get; }

        protected IOrderManager OrderManager { get; }
        protected IClipboardManager ClipboardManager { get; }
        protected INotifyManager NotifyManager { get; }
        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public LauncherItemKind Kind { get; private set; }
        public bool IsEnabledCommandLauncher { get; private set; }
        public string Comment { get; private set; }

        public LauncherIconElement Icon { get; }

        public virtual bool NowCustomizing
        {
            get => this._nowCustomize;
            private set => SetProperty(ref this._nowCustomize, value);
        }

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
            LauncherExecutePathData pathData;
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

        IList<LauncherEnvironmentVariableItem> GetEnvironmentVariableItems(IDatabaseCommander commander, IDatabaseImplementation implementation)
        {
            var launcherEnvVarsEntityDao = new LauncherEnvVarsEntityDao(commander, StatementLoader, implementation, Logger.Factory);
            return launcherEnvVarsEntityDao.SelectItems(LauncherItemId).ToList();
        }

        ILauncherExecuteResult ExecuteFile(Screen screen)
        {
            LauncherFileData fileData;
            IList<LauncherEnvironmentVariableItem> envItems;
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                fileData = launcherFilesEntityDao.SelectFile(LauncherItemId);
                if(fileData.IsEnabledCustomEnvironmentVariable) {
                    envItems = GetEnvironmentVariableItems(commander, commander.Implementation);
                } else {
                    envItems = new List<LauncherEnvironmentVariableItem>();
                }
            }

            var launcherExecutor = new LauncherExecutor(OrderManager, Logger.Factory);
            var result = launcherExecutor.Execute(Kind, fileData, fileData, envItems, screen);

            return result;
        }

        public ILauncherExecuteResult Execute(Screen screen)
        {
            try {
                ILauncherExecuteResult result;
                switch(Kind) {
                    case LauncherItemKind.File:
                        result = ExecuteFile(screen);
                        break;

                    default:
                        throw new NotImplementedException();
                }

                Debug.Assert(result != null);

                using(var commander = MainDatabaseBarrier.WaitWrite()) {
                    var dao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                    dao.UpdateIncrement(LauncherItemId, DatabaseCommonStatus.CreateCurrentAccount());
                    commander.Commit();
                }

                return result;

            } catch(Exception ex) {
                Logger.Error(ex);
                return LauncherExecuteResult.Error(ex);
            }
        }

        public ILauncherExecuteResult ExecuteExtends(Screen screen)
        {
            throw new NotImplementedException();
        }

        ILauncherExecutePathParameter GetExecutePath()
        {
            Debug.Assert(Kind == LauncherItemKind.File || Kind == LauncherItemKind.Directory);

            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherFilesEntityDao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, Logger.Factory);
                return launcherFilesEntityDao.SelectPath(LauncherItemId);
            }
        }

        public ILauncherExecuteResult OpenParentDirectory()
        {
            if(!(Kind == LauncherItemKind.File || Kind == LauncherItemKind.Directory)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, Logger.Factory);
            var result = launcherExecutor.OpenParentDirectory(Kind, pathData);

            return result;

        }

        public ILauncherExecuteResult OpenWorkingDirectory()
        {
            if(!(Kind == LauncherItemKind.File || Kind == LauncherItemKind.Directory)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, Logger.Factory);
            var result = launcherExecutor.OpenWorkingDirectory(Kind, pathData);

            return result;

        }

        public void CopyExecutePath()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.Path;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyParentDirectory()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = Path.GetDirectoryName(pathData.Path);
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyOption()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.Option;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void CopyWorkingDirectory()
        {
            var pathData = GetExecutePath();
            var data = new DataObject();
            var value = pathData.WorkDirectoryPath;
            data.SetText(value, TextDataFormat.UnicodeText);
            ClipboardManager.Set(data);
        }

        public void OpenCustomizeView(Screen screen)
        {
            if(NowCustomizing) {
                Logger.Warning($"現在編集中: {LauncherItemId}");
                return;
            }

            //TODO: 確定時の処理
            NowCustomizing = true;
            var element = OrderManager.CreateCustomizeLauncherItemElement(LauncherItemId, Icon, screen);
            element.StartView();
        }

        public void ShowProperty()
        {
            if(!(Kind == LauncherItemKind.File || Kind == LauncherItemKind.Directory)) {
                throw new InvalidOperationException($"{Kind}");
            }

            var pathData = GetExecutePath();

            var launcherExecutor = new LauncherExecutor(OrderManager, Logger.Factory);
            launcherExecutor.ShowProperty(pathData);
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
