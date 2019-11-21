using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContentTypeTextNet.Pe.Core.Compatibility.Forms;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Element.LauncherIcon;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.LauncherItemCustomize
{
    public abstract class LauncherItemCustomizeElementBase : ElementBase, ILauncherItemId
    {
        #region variable

        bool _isVisible;

        #endregion

        public LauncherItemCustomizeElementBase(Guid launcherItemId, LauncherIconElement launcherIconElement, IClipboardManager clipboardManager, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            LauncherItemId = launcherItemId;
            ClipboardManager = clipboardManager;
            Icon = launcherIconElement;
        }

        #region property
        public LauncherIconElement Icon { get; }
        protected IClipboardManager ClipboardManager { get; }

        public string Name { get; protected set; } = string.Empty;
        public string Code { get; protected set; } = string.Empty;
        public LauncherItemKind Kind { get; protected set; }
        public bool IsEnabledCommandLauncher { get; protected set; }

        public IconData? IconData { get; protected set; }
        public string? Comment { get; protected set; }

        protected bool ViewCreated { get; set; }

        public bool IsVisible
        {
            get => this._isVisible;
            protected set => SetProperty(ref this._isVisible, value);
        }


        #endregion

        #region function
        public abstract LauncherFileData LoadFileData();

        public abstract IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems();


        public abstract IReadOnlyCollection<string> LoadTags();

        public abstract void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags);

        #endregion

        #region ElementBase
        #endregion

        #region ILauncherItemId
        public Guid LauncherItemId { get; }
        #endregion

        #region IViewShowStarter

        public bool CanStartShowView
        {
            get
            {
                if(ViewCreated) {
                    return false;
                }

                return IsVisible;
            }
        }

        public virtual void StartView()
        {
            ViewCreated = true;
        }

        #endregion

        #region IViewCloseReceiver

        public bool ReceiveViewUserClosing()
        {
            IsVisible = false;
            return true;
        }
        public bool ReceiveViewClosing()
        {
            return true;
        }

        public virtual void ReceiveViewClosed()
        {
            ViewCreated = false;
        }


        #endregion

    }

    public class LauncherItemCustomizeElement : LauncherItemCustomizeElementBase, IViewShowStarter, IViewCloseReceiver
    {
        public LauncherItemCustomizeElement(Guid launcherItemId, Screen screen, IOrderManager orderManager, IClipboardManager clipboardManager, INotifyManager notifyManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, LauncherIconElement launcherIconElement, ILoggerFactory loggerFactory)
            : base(launcherItemId, launcherIconElement, clipboardManager, loggerFactory)
        {
            OrderManager = orderManager;
            NotifyManager = notifyManager;
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property


        IOrderManager OrderManager { get; }
        INotifyManager NotifyManager { get; }
        IMainDatabaseBarrier MainDatabaseBarrier { get; }
        IFileDatabaseBarrier FileDatabaseBarrier { get; }
        IDatabaseStatementLoader StatementLoader { get; }

        #endregion

        #region function

        void LoadLauncherItem()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var launcherItemsDao = new LauncherItemsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var launcherItemData = launcherItemsDao.SelectLauncherItem(LauncherItemId);

                Name = launcherItemData.Name;
                Code = launcherItemData.Code;
                Kind = launcherItemData.Kind;
                IconData = launcherItemData.Icon;
                IsEnabledCommandLauncher = launcherItemData.IsEnabledCommandLauncher;
                Comment = launcherItemData.Comment;
            }
        }

        #endregion

        #region LauncherItemCustomizeElementBase

        public override LauncherFileData LoadFileData()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherFilesEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectFile(LauncherItemId);
            }
        }

        public override IReadOnlyCollection<LauncherEnvironmentVariableData> LoadEnvironmentVariableItems()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherEnvVarsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectEnvVarItems(LauncherItemId).ToList();
            }
        }


        public override IReadOnlyCollection<string> LoadTags()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var dao = new LauncherTagsEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                return dao.SelectTags(LauncherItemId).ToList();
            }
        }

        public override void SaveFile(LauncherItemData launcherItemData, LauncherFileData launcherFileData, IEnumerable<LauncherEnvironmentVariableData> environmentVariableItems, IEnumerable<string> tags)
        {
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

            NotifyManager.SendLauncherItemChanged(LauncherItemId);
        }

        protected override void InitializeImpl()
        {
            LoadLauncherItem();
        }

        public override void StartView()
        {
            var windowItem = OrderManager.CreateCustomizeLauncherItemWindow(this);
            windowItem.Window.Show();

            base.StartView();
        }
        public override void ReceiveViewClosed()
        {
            NotifyManager.SendCustomizeLauncherItemExited(LauncherItemId);

            base.ReceiveViewClosed();
        }

        #endregion


    }
}
