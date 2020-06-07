using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Bridge.Models;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Data;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using ContentTypeTextNet.Pe.Main.Models.Manager;
using ContentTypeTextNet.Pe.Main.Models.Manager.Setting;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public sealed class DatabaseCommandPack : TApplicationPackBase<IDatabaseCommands, DatabaseCommands>
    {
        public DatabaseCommandPack(DatabaseCommands main, DatabaseCommands file, DatabaseCommands temporary, IDatabaseCommonStatus commonStatus)
            : base(main, file, temporary)
        {
            CommonStatus=commonStatus;
        }

        #region property

        public IDatabaseCommonStatus CommonStatus { get; }

        #endregion
    }

    /// <summary>
    /// 各設定項目の親。
    /// </summary>
    public abstract class SettingEditorElementBase : ElementBase
    {
        protected SettingEditorElementBase(ISettingNotifyManager settingNotifyManager, IClipboardManager clipboardManager, IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, IIdFactory idFactory, IDispatcherWrapper dispatcherWrapper, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            SettingNotifyManager = settingNotifyManager;
            ClipboardManager = clipboardManager;

            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;

            IdFactory = idFactory;
            DispatcherWrapper = dispatcherWrapper;

            SettingNotifyManager.LauncherItemRemoved += SettingNotifyManager_LauncherItemRemoved;
        }

        #region property

        protected ISettingNotifyManager SettingNotifyManager { get; }
        protected IClipboardManager ClipboardManager { get; }

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        protected IIdFactory IdFactory { get; }
        protected IDispatcherWrapper DispatcherWrapper { get; }

        public bool IsLoaded { get; private set; }
        #endregion

        #region function

        protected abstract void LoadImpl();

        public void Load()
        {
            if(IsLoaded) {
                throw new InvalidOperationException(nameof(IsLoaded));
            }

            LoadImpl();

            IsLoaded = true;
        }

        protected abstract void SaveImpl(DatabaseCommandPack commandPack);

        public void Save(DatabaseCommandPack commandPack)
        {
            if(!IsLoaded) {
                throw new InvalidOperationException(nameof(IsLoaded));
            }

            SaveImpl(commandPack);
        }

        protected virtual void ReceiveLauncherItemRemoved(Guid launcherItemId)
        { }

        #endregion


        #region ElementBase

        protected override void InitializeImpl()
        {
            //NOTE: 設定処理では初期かではなくページ切り替え処理であれこれ頑張る
        }

        protected override void Dispose(bool disposing)
        {
            if(!IsDisposed) {
                SettingNotifyManager.LauncherItemRemoved -= SettingNotifyManager_LauncherItemRemoved;
            }

            base.Dispose(disposing);
        }


        #endregion

        private void SettingNotifyManager_LauncherItemRemoved(object? sender, LauncherItemRemovedEventArgs e)
        {
            ReceiveLauncherItemRemoved(e.LauncherItemId);
        }


    }
}
