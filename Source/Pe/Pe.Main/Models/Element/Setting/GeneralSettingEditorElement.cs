using System;
using System.Collections.Generic;
using System.Text;
using ContentTypeTextNet.Pe.Core.Models.Database;
using ContentTypeTextNet.Pe.Main.Models.Applications;
using ContentTypeTextNet.Pe.Main.Models.Database.Dao.Entity;
using ContentTypeTextNet.Pe.Main.Models.Logic;
using Microsoft.Extensions.Logging;

namespace ContentTypeTextNet.Pe.Main.Models.Element.Setting
{
    public abstract class GeneralSettingEditorElementBase : ElementBase
    {
        public GeneralSettingEditorElementBase(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            MainDatabaseBarrier = mainDatabaseBarrier;
            FileDatabaseBarrier = fileDatabaseBarrier;
            StatementLoader = statementLoader;
        }

        #region property

        protected IMainDatabaseBarrier MainDatabaseBarrier { get; }
        protected IFileDatabaseBarrier FileDatabaseBarrier { get; }
        protected IDatabaseStatementLoader StatementLoader { get; }
        #endregion

        #region function

        public void Save(DatabaseCommandPack commandPack)
        {
            SaveImpl(commandPack);
        }

        protected abstract void SaveImpl(DatabaseCommandPack commandPack);

        #endregion
    }

    public class AppExecuteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppExecuteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        public string UserId { get; set; } = string.Empty;
        public bool SendUsageStatistics { get; set; }

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
            using(var commander = MainDatabaseBarrier.WaitRead()) {
                var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commander, StatementLoader, commander.Implementation, LoggerFactory);
                var setting = appExecuteSettingEntityDao.SelectSettingExecuteSetting();
                UserId = setting.UserId;
                SendUsageStatistics = setting.SendUsageStatistics;
            }

            var userIdManager = new UserIdManager(LoggerFactory);
            if(!userIdManager.IsValidUserId(UserId)) {
                UserId = userIdManager.CreateFromEnvironment();
            }
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
            var appExecuteSettingEntityDao = new AppExecuteSettingEntityDao(commandPack.Main.Commander, StatementLoader, commandPack.Main.Implementation, LoggerFactory);
            appExecuteSettingEntityDao.UpdateSettingExecuteSetting(SendUsageStatistics, UserId, commandPack.CommonStatus);
        }

        #endregion
    }


    public class AppGeneralSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppGeneralSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property

        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {
        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }


    public class AppUpdateSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppUpdateSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }



    public class AppCommandSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppCommandSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }



    public class AppNoteSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppNoteSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }



    public class AppStandardInputOutputSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppStandardInputOutputSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }


    public class AppWindowSettingEditorElement : GeneralSettingEditorElementBase
    {
        public AppWindowSettingEditorElement(IMainDatabaseBarrier mainDatabaseBarrier, IFileDatabaseBarrier fileDatabaseBarrier, IDatabaseStatementLoader statementLoader, ILoggerFactory loggerFactory)
            : base(mainDatabaseBarrier, fileDatabaseBarrier, statementLoader, loggerFactory)
        { }

        #region property
        #endregion

        #region function
        #endregion

        #region GeneralSettingEditorBase

        protected override void InitializeImpl()
        {

        }

        protected override void SaveImpl(DatabaseCommandPack commandPack)
        {
        }

        #endregion
    }

}
